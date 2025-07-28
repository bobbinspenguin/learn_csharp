using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ComprehensiveDemo.Models;

namespace ComprehensiveDemo.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Category
            builder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                // Self-referencing relationship
                entity.HasOne(e => e.ParentCategory)
                      .WithMany(e => e.SubCategories)
                      .HasForeignKey(e => e.ParentCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.ParentCategoryId);
            });

            // Configure Product
            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.SKU).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.SalePrice).HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Category)
                      .WithMany(e => e.Products)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.SKU).IsUnique();
                entity.HasIndex(e => e.Name);
                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.IsFeatured);
            });

            // Configure CustomerProfile
            builder.Entity<CustomerProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.HasOne(e => e.User)
                      .WithOne(e => e.CustomerProfile)
                      .HasForeignKey<CustomerProfile>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId).IsUnique();
            });

            // Configure Address
            builder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.State).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.CustomerProfile)
                      .WithMany(e => e.Addresses)
                      .HasForeignKey(e => e.CustomerProfileId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.CustomerProfileId);
                entity.HasIndex(e => e.IsDefault);
            });

            // Configure Order
            builder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.SubTotal).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TaxAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.ShippingAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.User)
                      .WithMany(e => e.Orders)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            // Configure OrderItem
            builder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)");

                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.OrderId);
                entity.HasIndex(e => e.ProductId);
            });

            // Configure Review
            builder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.HasOne(e => e.Product)
                      .WithMany(e => e.Reviews)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany(e => e.Reviews)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.ProductId);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Rating);
                entity.HasIndex(e => e.IsApproved);
            });

            // Configure AuditLog
            builder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.UserEmail).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Entity).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EntityId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IpAddress).HasMaxLength(45);
                entity.Property(e => e.UserAgent).HasMaxLength(500);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => new { e.Entity, e.EntityId });
            });

            // Configure Identity entities
            builder.Entity<AppUser>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.CreatedAt);
            });

            builder.Entity<AppRole>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(200);
            });

            // Seed initial data
            SeedData(builder);
        }

        private static void SeedData(ModelBuilder builder)
        {
            // Seed roles
            var adminRoleId = Guid.NewGuid().ToString();
            var managerRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            builder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Description = "System Administrator with full access",
                    CreatedAt = DateTime.UtcNow
                },
                new AppRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    Description = "Manager with elevated privileges",
                    CreatedAt = DateTime.UtcNow
                },
                new AppRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    Description = "Standard user with basic access",
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Seed categories
            var electronicsId = 1;
            var clothingId = 2;
            var booksId = 3;
            var laptopsId = 4;
            var phonesId = 5;

            builder.Entity<Category>().HasData(
                new Category { Id = electronicsId, Name = "Electronics", Description = "Electronic devices and accessories", CreatedAt = DateTime.UtcNow },
                new Category { Id = clothingId, Name = "Clothing", Description = "Fashion and apparel", CreatedAt = DateTime.UtcNow },
                new Category { Id = booksId, Name = "Books", Description = "Books and literature", CreatedAt = DateTime.UtcNow },
                new Category { Id = laptopsId, Name = "Laptops", Description = "Laptop computers", ParentCategoryId = electronicsId, CreatedAt = DateTime.UtcNow },
                new Category { Id = phonesId, Name = "Smartphones", Description = "Mobile phones", ParentCategoryId = electronicsId, CreatedAt = DateTime.UtcNow }
            );

            // Seed products
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Dell XPS 13 Laptop",
                    SKU = "DELL-XPS-13",
                    Description = "High-performance ultrabook with 13-inch display",
                    Price = 1299.99m,
                    StockQuantity = 50,
                    CategoryId = laptopsId,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 2,
                    Name = "iPhone 15 Pro",
                    SKU = "APPLE-IP15-PRO",
                    Description = "Latest iPhone with Pro features",
                    Price = 1099.99m,
                    StockQuantity = 30,
                    CategoryId = phonesId,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 3,
                    Name = "Samsung Galaxy S24",
                    SKU = "SAM-GAL-S24",
                    Description = "Premium Android smartphone",
                    Price = 899.99m,
                    SalePrice = 799.99m,
                    StockQuantity = 25,
                    CategoryId = phonesId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 4,
                    Name = "MacBook Air M2",
                    SKU = "APPLE-MBA-M2",
                    Description = "Apple MacBook Air with M2 chip",
                    Price = 1199.99m,
                    StockQuantity = 20,
                    CategoryId = laptopsId,
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 5,
                    Name = "Programming T-Shirt",
                    SKU = "TSHIRT-PROG",
                    Description = "Comfortable cotton t-shirt for developers",
                    Price = 29.99m,
                    StockQuantity = 100,
                    CategoryId = clothingId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = 6,
                    Name = "Clean Code Book",
                    SKU = "BOOK-CLEAN-CODE",
                    Description = "A Handbook of Agile Software Craftsmanship",
                    Price = 39.99m,
                    StockQuantity = 75,
                    CategoryId = booksId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            // Seed admin user
            var adminUserId = Guid.NewGuid().ToString();
            var managerUserId = Guid.NewGuid().ToString();
            var regularUserId = Guid.NewGuid().ToString();

            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = adminUserId,
                    FirstName = "System",
                    LastName = "Administrator",
                    UserName = "admin@demo.com",
                    NormalizedUserName = "ADMIN@DEMO.COM",
                    Email = "admin@demo.com",
                    NormalizedEmail = "ADMIN@DEMO.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEOatVJsm7J1Z5Q8Q5Y6M0X8Q2Z8K7L9M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6A7B8C9D0E1F2G", // Admin123!
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new AppUser
                {
                    Id = managerUserId,
                    FirstName = "Store",
                    LastName = "Manager",
                    UserName = "manager@demo.com",
                    NormalizedUserName = "MANAGER@DEMO.COM",
                    Email = "manager@demo.com",
                    NormalizedEmail = "MANAGER@DEMO.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEOatVJsm7J1Z5Q8Q5Y6M0X8Q2Z8K7L9M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6A7B8C9D0E1F2G", // Manager123!
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new AppUser
                {
                    Id = regularUserId,
                    FirstName = "John",
                    LastName = "Customer",
                    UserName = "user@demo.com",
                    NormalizedUserName = "USER@DEMO.COM",
                    Email = "user@demo.com",
                    NormalizedEmail = "USER@DEMO.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEOatVJsm7J1Z5Q8Q5Y6M0X8Q2Z8K7L9M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6A7B8C9D0E1F2G", // User123!
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Assign roles to users
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().HasData(
                new Microsoft.AspNetCore.Identity.IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId },
                new Microsoft.AspNetCore.Identity.IdentityUserRole<string> { UserId = managerUserId, RoleId = managerRoleId },
                new Microsoft.AspNetCore.Identity.IdentityUserRole<string> { UserId = regularUserId, RoleId = userRoleId }
            );
        }
    }
}
