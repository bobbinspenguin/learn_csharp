using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComprehensiveDemo.Models
{
    // User and Identity Models
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;

        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public List<Order> Orders { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public CustomerProfile? CustomerProfile { get; set; }
    }

    public class AppRole : IdentityRole
    {
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // Business Domain Models
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category> SubCategories { get; set; } = new();
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public List<Product> Products { get; set; } = new();
    }

    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SalePrice { get; set; }
        
        public int StockQuantity { get; set; }
        public int ReorderLevel { get; set; } = 10;
        
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign keys
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        
        // Navigation properties
        public List<OrderItem> OrderItems { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
    }

    public class CustomerProfile
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public List<Address> Addresses { get; set; } = new();
    }

    public class Address
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Street { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;
        
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Foreign key
        public int CustomerProfileId { get; set; }
        public CustomerProfile CustomerProfile { get; set; } = null!;
    }

    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(20)]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal SubTotal { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxAmount { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal ShippingAmount { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        
        // Shipping address
        [Required]
        [StringLength(200)]
        public string ShippingStreet { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ShippingCity { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ShippingState { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string ShippingZipCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ShippingCountry { get; set; } = string.Empty;
        
        // Navigation properties
        public List<OrderItem> OrderItems { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        public int Quantity { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }
    }

    public class Review
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;
        
        public int Rating { get; set; } // 1-5 stars
        
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;
        
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // Audit and System Models
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }

    // DTOs for API
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateProductDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string SKU { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalePrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; } = 10;

        [Required]
        public int CategoryId { get; set; }

        public bool IsFeatured { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();

        [Required]
        public AddressDto ShippingAddress { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class AddressDto
    {
        [Required]
        [StringLength(200)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;
    }

    public class ReviewDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReviewDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Comment { get; set; } = string.Empty;
    }

    // Statistics and Reports
    public class DashboardStatsDto
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int LowStockProducts { get; set; }
        public int PendingOrders { get; set; }
        public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();
        public List<RecentOrderDto> RecentOrders { get; set; } = new();
    }

    public class TopSellingProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RecentOrderDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public int? TotalCount { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
