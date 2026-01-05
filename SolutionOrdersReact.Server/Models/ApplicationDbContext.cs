using Microsoft.EntityFrameworkCore;
using System;

namespace SolutionOrdersReact.Server.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : DbContext(options)
    {
        public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Item> Items { get; set; }

        // ORDERS = ZAMÓWIENIA (PO KLIKNIĘCIU "ZAMÓW")
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // KATALOG
        public DbSet<Product> Products { get; set; }
        public DbSet<GalleryItem> GalleryItems { get; set; }

        // SOCIAL
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GalleryRating> GalleryRatings { get; set; }

        // USERS
        public DbSet<User> Users { get; set; }

        // 🛒 KOSZYK (PRZED ZAMÓWIENIEM)
        public DbSet<CartItem> CartItems { get; set; }

        // 📜 ACTIVITY LOG
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UnitOfMeasurement>(entity =>
            {
                entity.HasKey(e => e.IdUnitOfMeasurement);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Adress).HasMaxLength(300);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<Worker>(entity =>
            {
                entity.HasKey(e => e.IdWorker);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Login).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.IdItem);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.FotoUrl).HasMaxLength(500);
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Source).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<GalleryItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Artist).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            /* =========================
               COMMENTS
               ========================= */
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Text).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            /* =========================
               GALLERY RATINGS ⭐
               ========================= */
            modelBuilder.Entity<GalleryRating>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Value).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.GalleryItem)
                    .WithMany()
                    .HasForeignKey(e => e.GalleryItemId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.GalleryItemId })
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Surname).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Login).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasIndex(e => e.Login).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            /* =========================
               🛒 CART ITEMS
               ========================= */
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TargetType)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(300)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => new { e.UserId, e.TargetType, e.TargetId });
            });

            /* =========================
               📜 ACTIVITY LOG / EVENT LOG
               ========================= */
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.EventType)
                    .HasConversion<string>()
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.Property(e => e.TargetType)
                    .HasMaxLength(64);

                entity.Property(e => e.TargetId)
                    .HasMaxLength(64);

                entity.Property(e => e.Message)
                    .HasMaxLength(500);

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(64);

                entity.Property(e => e.UserAgent)
                    .HasMaxLength(256);

                entity.Property(e => e.Path)
                    .HasMaxLength(256);

                entity.Property(e => e.CorrelationId)
                    .HasMaxLength(64);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => new { e.UserId, e.CreatedAt });
                entity.HasIndex(e => new { e.TargetType, e.TargetId, e.CreatedAt });
            });

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Surname = "System",
                    Login = "admin",
                    Email = "admin@demo.pl",
                    Password = "admin",
                    Role = "ADMIN",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}
