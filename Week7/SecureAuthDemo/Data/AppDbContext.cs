using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureAuthDemo.Models;

namespace SecureAuthDemo.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SecureDocument> SecureDocuments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure SecureDocument
            builder.Entity<SecureDocument>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.AccessLevel).IsRequired().HasMaxLength(20);
                entity.Property(e => e.OwnerId).IsRequired();
                
                entity.HasOne(e => e.Owner)
                      .WithMany()
                      .HasForeignKey(e => e.OwnerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.AccessLevel);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => new { e.OwnerId, e.IsDeleted });
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

            // Configure Identity tables
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

            // Seed admin user
            var adminUserId = Guid.NewGuid().ToString();
            builder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = adminUserId,
                    FirstName = "System",
                    LastName = "Administrator",
                    UserName = "admin@securedemo.com",
                    NormalizedUserName = "ADMIN@SECUREDEMO.COM",
                    Email = "admin@securedemo.com",
                    NormalizedEmail = "ADMIN@SECUREDEMO.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEOatVJsm7J1Z5Q8Q5Y6M0X8Q2Z8K7L9M3N4O5P6Q7R8S9T0U1V2W3X4Y5Z6A7B8C9D0E1F2G", // Password123!
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Assign admin role to admin user
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().HasData(
                new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                }
            );
        }
    }
}
