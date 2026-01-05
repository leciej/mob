using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Services.ActivityLog;
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

        // Mapster
        TypeAdapterConfig.GlobalSettings.Scan(
            Assembly.GetExecutingAssembly()
        );

        // MediatR
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<Program>()
        );

        // ✅ HttpContext (WYMAGANE dla ActivityLog)
        builder.Services.AddHttpContextAccessor();

        // ✅ Activity Log Service
        builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

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

        // =========================
        // MIGRACJE + SEED ADMINA
        // =========================
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            try
            {
                db.Database.Migrate();

                // ADMIN MUSI ISTNIEĆ I MIEĆ ID = 1
                var admin = db.Users.FirstOrDefault(u => u.Id == 1);

                if (admin == null)
                {
                    admin = new User
                    {
                        Id = 1, // ⬅️ WYMUSZONE
                        Name = "Admin",
                        Surname = "System",
                        Login = "admin",
                        Email = "admin@admin.pl",
                        Password = "admin",
                        Role = "ADMIN"
                    };

                    db.Users.Add(admin);
                    db.SaveChanges();
                }
                else if (admin.Role != "ADMIN")
                {
                    // ZABEZPIECZENIE: ID=1 ZAWSZE ADMIN
                    admin.Role = "ADMIN";
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger =
                    scope.ServiceProvider.GetRequiredService<
                        ILogger<Program>
                    >();
                logger.LogError(
                    ex,
                    "Błąd podczas migracji lub seeda admina"
                );
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
