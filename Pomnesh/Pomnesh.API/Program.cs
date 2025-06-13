using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;

namespace Pomnesh.API;

public abstract class Program
{
    [Obsolete("Obsolete")]
    public static void Main(string[] args)
    {
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

            builder.Host.UseSerilog();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(origin => 
                        origin.StartsWith("https://localhost:") ||
                        origin.StartsWith("http://localhost:") ||
                        origin == "https://pomnesh.hps-2.ru" ||
                        origin == "http://pomnesh.hps-2.ru" ||
                        origin == "https://prod-app53200850-da895392cd3f.pages-ac.vk-apps.com" ||
                        origin.EndsWith(".pages-ac.vk-apps.com"))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy("login", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy("register", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 3,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy("validate", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.AddPolicy("refresh", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1)
                        }));

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<DapperContext>();

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
                                               throw new InvalidOperationException("JWT Key not found in configuration or environment variables"))),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddScoped<IBaseRepository<Attachment>, AttachmentRepository>();
            builder.Services.AddScoped<IBaseRepository<ChatContext>, ChatContextRepository>();
            builder.Services.AddScoped<IBaseRepository<Recollection>, RecollectionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBaseRepository<User>>(sp => sp.GetRequiredService<IUserRepository>());

            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IChatContextService, ChatContextService>();
            builder.Services.AddScoped<IRecollectionService, RecollectionService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<AttachmentCreateRequestValidator>();
                    fv.DisableDataAnnotationsValidation = true;
                });

            builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(MigrationRunner).Assembly).For.Migrations());

            builder.Services.AddScoped<MigrationRunner>();

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

            // ✅ CORS должен быть до всего
            app.UseCors();

            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseMiddleware<JwtMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<ApiExceptionMiddleware>();

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
