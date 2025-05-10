using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure;
using Pomnesh.Infrastructure.Interfaces;
using Pomnesh.Infrastructure.Repositories;
using FluentMigrator.Runner;
using FluentValidation.AspNetCore;
using Pomnesh.API.Middlewares;
using Pomnesh.API.Validators;
using Pomnesh.Application.Interfaces;
using MigrationRunner = Pomnesh.Infrastructure.Migrations.MigrationRunner;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;

namespace Pomnesh.API;

public abstract class Program
{
    [Obsolete("Obsolete")]
    public static void Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting web application");
            var builder = WebApplication.CreateBuilder(args);

            // Add Serilog to the builder
            builder.Host.UseSerilog();

            // Add rate limiting services
            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            builder.Services.AddInMemoryRateLimiting();

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<DapperContext>();

            // Add JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? 
                            Environment.GetEnvironmentVariable("JWT_KEY") ?? 
                            throw new InvalidOperationException("JWT Key not found in configuration or environment variables")))
                };
            });

            // Database repos
            builder.Services.AddScoped<IBaseRepository<Attachment>, AttachmentRepository>();
            builder.Services.AddScoped<IBaseRepository<ChatContext>, ChatContextRepository>();
            builder.Services.AddScoped<IBaseRepository<Recollection>, RecollectionRepository>();
            builder.Services.AddScoped<IBaseRepository<User>, UserRepository>();

            // Backend services
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IChatContextService, ChatContextService>();
            builder.Services.AddScoped<IRecollectionService, RecollectionService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            
            // Validators
            builder.Services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    // Auto-registers all validators in the same assembly as this one
                    fv.RegisterValidatorsFromAssemblyContaining<AttachmentCreateRequestValidator>();

                    // Optional: disable [Required], [MaxLength], etc. if you want full FluentValidation control
                    fv.DisableDataAnnotationsValidation = true;
                });
            
            // Migrations
            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres() 
                    .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(MigrationRunner).Assembly).For.Migrations());

            builder.Services.AddScoped<MigrationRunner>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI();
            // }

            app.UseHttpsRedirection();

            // Add rate limiting middleware
            app.UseIpRateLimiting();

            // Add authentication middleware before authorization
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // Middlewares
            app.UseSerilogRequestLogging();
            app.UseMiddleware<ApiExceptionMiddleware>();

            // Run Migrations on Startup
            using (var scope = app.Services.CreateScope())
            {
                var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
                migrationRunner.Run();
            }
            
            Log.Information("Web application started");
            app.Run();
            
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}