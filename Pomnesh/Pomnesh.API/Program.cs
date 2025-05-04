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

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<DapperContext>();

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
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI();
            // }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            // Middlewares
            app.UseSerilogRequestLogging();
            //app.UseMiddleware<PerformanceMiddleware>();
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