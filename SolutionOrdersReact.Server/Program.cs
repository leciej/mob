using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SolutionOrdersReact.Server.Models;
using SolutionOrdersReact.Server.Services.ActivityLog;
using SolutionOrdersReact.Server.Services.Cart;
using System.Reflection;

namespace SolutionOrdersReact.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        
        builder.Services.AddControllers();

        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            )
        );

        
        TypeAdapterConfig.GlobalSettings.Scan(
            Assembly.GetExecutingAssembly()
        );

        
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<Program>()
        );

        
        builder.Services.AddHttpContextAccessor();

        
        builder.Services.AddScoped<IActivityLogService, ActivityLogService>();

        
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

        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        
        
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            try
            {
                db.Database.Migrate();

                
                var admin = db.Users.FirstOrDefault(u => u.Id == 1);

                if (admin == null)
                {
                    admin = new User
                    {
                        Id = 1, 
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
