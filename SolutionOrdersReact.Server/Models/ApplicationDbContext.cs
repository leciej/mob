using Microsoft.EntityFrameworkCore;

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

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<GalleryItem> GalleryItems { get; set; }

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

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Items)
                    .HasForeignKey(e => e.IdCategory)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UnitOfMeasurement)
                    .WithMany(u => u.Items)
                    .HasForeignKey(e => e.IdUnitOfMeasurement)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ✅ NOWE ZAMÓWIENIA (MINIMALNE)

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasMany(e => e.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);
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

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UnitOfMeasurement>().HasData(
                new UnitOfMeasurement { IdUnitOfMeasurement = 1, Name = "szt", Description = "Sztuki", IsActive = true },
                new UnitOfMeasurement { IdUnitOfMeasurement = 2, Name = "kg", Description = "Kilogramy", IsActive = true },
                new UnitOfMeasurement { IdUnitOfMeasurement = 3, Name = "l", Description = "Litry", IsActive = true }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { IdCategory = 1, Name = "Elektronika", Description = "Urządzenia elektroniczne", IsActive = true },
                new Category { IdCategory = 2, Name = "Żywność", Description = "Produkty spożywcze", IsActive = true }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client { IdClient = 1, Name = "Jan Kowalski", Adress = "ul. Główna 1, Warszawa", PhoneNumber = "500-100-200", IsActive = true },
                new Client { IdClient = 2, Name = "Anna Nowak", Adress = "ul. Kwiatowa 5, Kraków", PhoneNumber = "600-200-300", IsActive = true }
            );

            modelBuilder.Entity<Worker>().HasData(
                new Worker { IdWorker = 1, FirstName = "Piotr", LastName = "Kowalczyk", Login = "pkowalczyk", Password = "haslo123", IsActive = true },
                new Worker { IdWorker = 2, FirstName = "Maria", LastName = "Wiśniewska", Login = "mwisnieska", Password = "haslo456", IsActive = true }
            );

            modelBuilder.Entity<Item>().HasData(
                new Item { IdItem = 1, Name = "Laptop Dell", Description = "Laptop Dell Inspiron 15", IdCategory = 1, Price = 3500, Quantity = 10, IdUnitOfMeasurement = 1, Code = "LAP001", IsActive = true },
                new Item { IdItem = 2, Name = "Monitor Samsung", Description = "Monitor 24 cale", IdCategory = 1, Price = 800, Quantity = 15, IdUnitOfMeasurement = 1, Code = "MON001", IsActive = true }
            );
        }
    }
}
