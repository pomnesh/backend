using Pomnesh.Application.Services;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure;
using Pomnesh.Infrastructure.Interfaces;
using Pomnesh.Infrastructure.Repositories;

namespace Pomnesh.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<DapperContext>();

        // Database repos
        builder.Services.AddScoped<IBaseRepository<Attachment>, AttachmentRepository>();
        builder.Services.AddScoped<IBaseRepository<ChatContext>, ChatContextRepository>();
        builder.Services.AddScoped<IBaseRepository<Recollection>, RecollectionRepository>();
        builder.Services.AddScoped<IBaseRepository<User>, UserRepository>();

        // Backend services
        builder.Services.AddScoped<AttachmentService>();
        builder.Services.AddScoped<ChatContextService>();
        builder.Services.AddScoped<RecollectionService>();
        builder.Services.AddScoped<UserService>();
        
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}