using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using Mapster;
using MediatR;
using System.Reflection;

namespace SolutionOrdersReact.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Controllers
        builder.Services.AddControllers();

        // DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            )
        );

        // Mapster – automatyczne ładowanie konfiguracji IRegister
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        // MediatR
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<Program>()
        );

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
        });

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Migracje
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var dbContext =
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger =
                    scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Błąd podczas migracji bazy danych");
            }
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowAll");
        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}
