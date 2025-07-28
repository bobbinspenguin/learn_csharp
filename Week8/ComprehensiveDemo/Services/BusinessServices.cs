using ComprehensiveDemo.Data;
using ComprehensiveDemo.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ComprehensiveDemo.Services
{
    // Repository Pattern Implementation
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }

    // Specialized Repositories
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync(int page = 1, int pageSize = 10, int? categoryId = null, bool? isFeatured = null);
        Task<ProductDto?> GetProductWithDetailsAsync(int id);
        Task<bool> IsSkuUniqueAsync(string sku, int? excludeId = null);
        Task<IEnumerable<Product>> GetLowStockProductsAsync();
        Task<IEnumerable<Product>> GetFeaturedProductsAsync();
    }

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ProductDto>> GetProductsWithDetailsAsync(int page = 1, int pageSize = 10, int? categoryId = null, bool? isFeatured = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .Where(p => p.IsActive);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (isFeatured.HasValue)
                query = query.Where(p => p.IsFeatured == isFeatured.Value);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Description = p.Description,
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    StockQuantity = p.StockQuantity,
                    IsActive = p.IsActive,
                    IsFeatured = p.IsFeatured,
                    CategoryName = p.Category.Name,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    ReviewCount = p.Reviews.Count,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductWithDetailsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .Where(p => p.Id == id && p.IsActive)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Description = p.Description,
                    Price = p.Price,
                    SalePrice = p.SalePrice,
                    StockQuantity = p.StockQuantity,
                    IsActive = p.IsActive,
                    IsFeatured = p.IsFeatured,
                    CategoryName = p.Category.Name,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    ReviewCount = p.Reviews.Count,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsSkuUniqueAsync(string sku, int? excludeId = null)
        {
            var query = _context.Products.Where(p => p.SKU == sku);
            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive && p.StockQuantity <= p.ReorderLevel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.IsFeatured)
                .ToListAsync();
        }
    }

    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<OrderDto>> GetOrdersWithDetailsAsync(int page = 1, int pageSize = 10, string? userId = null, string? status = null);
        Task<OrderDto?> GetOrderWithDetailsAsync(int id);
        Task<string> GenerateOrderNumberAsync();
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10);
    }

    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<OrderDto>> GetOrdersWithDetailsAsync(int page = 1, int pageSize = 10, string? userId = null, string? status = null)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(o => o.UserId == userId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.Status == status);

            return await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.User.FullName,
                    CustomerEmail = o.User.Email!,
                    SubTotal = o.SubTotal,
                    TaxAmount = o.TaxAmount,
                    ShippingAmount = o.ShippingAmount,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    ShippedAt = o.ShippedAt,
                    DeliveredAt = o.DeliveredAt,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductName = oi.Product.Name,
                        ProductSKU = oi.Product.SKU,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<OrderDto?> GetOrderWithDetailsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.User.FullName,
                    CustomerEmail = o.User.Email!,
                    SubTotal = o.SubTotal,
                    TaxAmount = o.TaxAmount,
                    ShippingAmount = o.ShippingAmount,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    CreatedAt = o.CreatedAt,
                    ShippedAt = o.ShippedAt,
                    DeliveredAt = o.DeliveredAt,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        ProductName = oi.Product.Name,
                        ProductSKU = oi.Product.SKU,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var count = await _context.Orders
                .Where(o => o.OrderNumber.StartsWith(today))
                .CountAsync();

            return $"{today}{(count + 1):D4}";
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 10)
        {
            return await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }

    // Business Services
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsAsync(int page = 1, int pageSize = 10, int? categoryId = null, bool? isFeatured = null);
        Task<ApiResponse<ProductDto>> GetProductAsync(int id);
        Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto productDto);
        Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, CreateProductDto productDto);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
        Task<ApiResponse<IEnumerable<Product>>> GetLowStockProductsAsync();
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IAuditService _auditService;

        public ProductService(IProductRepository productRepository, IAuditService auditService)
        {
            _productRepository = productRepository;
            _auditService = auditService;
        }

        public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsAsync(int page = 1, int pageSize = 10, int? categoryId = null, bool? isFeatured = null)
        {
            try
            {
                var products = await _productRepository.GetProductsWithDetailsAsync(page, pageSize, categoryId, isFeatured);
                return new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = true,
                    Data = products,
                    Message = $"Retrieved {products.Count()} products",
                    PageNumber = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ProductDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve products",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<ProductDto>> GetProductAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetProductWithDetailsAsync(id);
                if (product == null)
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }

                return new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = product,
                    Message = "Product retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Failed to retrieve product",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto productDto)
        {
            try
            {
                // Validate SKU uniqueness
                if (!await _productRepository.IsSkuUniqueAsync(productDto.SKU))
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success = false,
                        Message = "SKU already exists",
                        Errors = new List<string> { "A product with this SKU already exists" }
                    };
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    SKU = productDto.SKU,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    SalePrice = productDto.SalePrice,
                    StockQuantity = productDto.StockQuantity,
                    ReorderLevel = productDto.ReorderLevel,
                    CategoryId = productDto.CategoryId,
                    IsFeatured = productDto.IsFeatured,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdProduct = await _productRepository.AddAsync(product);
                var productDetails = await _productRepository.GetProductWithDetailsAsync(createdProduct.Id);

                await _auditService.LogAsync("Create", "Product", createdProduct.Id.ToString(), null, productDto);

                return new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = productDetails,
                    Message = "Product created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Failed to create product",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, CreateProductDto productDto)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }

                // Validate SKU uniqueness (excluding current product)
                if (!await _productRepository.IsSkuUniqueAsync(productDto.SKU, id))
                {
                    return new ApiResponse<ProductDto>
                    {
                        Success = false,
                        Message = "SKU already exists",
                        Errors = new List<string> { "A product with this SKU already exists" }
                    };
                }

                var oldValues = new
                {
                    existingProduct.Name,
                    existingProduct.SKU,
                    existingProduct.Description,
                    existingProduct.Price,
                    existingProduct.SalePrice,
                    existingProduct.StockQuantity,
                    existingProduct.CategoryId,
                    existingProduct.IsFeatured
                };

                existingProduct.Name = productDto.Name;
                existingProduct.SKU = productDto.SKU;
                existingProduct.Description = productDto.Description;
                existingProduct.Price = productDto.Price;
                existingProduct.SalePrice = productDto.SalePrice;
                existingProduct.StockQuantity = productDto.StockQuantity;
                existingProduct.ReorderLevel = productDto.ReorderLevel;
                existingProduct.CategoryId = productDto.CategoryId;
                existingProduct.IsFeatured = productDto.IsFeatured;
                existingProduct.UpdatedAt = DateTime.UtcNow;

                await _productRepository.UpdateAsync(existingProduct);
                var productDetails = await _productRepository.GetProductWithDetailsAsync(id);

                await _auditService.LogAsync("Update", "Product", id.ToString(), oldValues, productDto);

                return new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = productDetails,
                    Message = "Product updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Failed to update product",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }

                // Soft delete by setting IsActive to false
                product.IsActive = false;
                product.UpdatedAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(product);

                await _auditService.LogAsync("Delete", "Product", id.ToString(), new { product.Name, product.SKU }, null);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Product deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Failed to delete product",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<Product>>> GetLowStockProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetLowStockProductsAsync();
                return new ApiResponse<IEnumerable<Product>>
                {
                    Success = true,
                    Data = products,
                    Message = $"Retrieved {products.Count()} low stock products"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<Product>>
                {
                    Success = false,
                    Message = "Failed to retrieve low stock products",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }

    // Audit Service
    public interface IAuditService
    {
        Task LogAsync(string action, string entity, string entityId, object? oldValues = null, object? newValues = null);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(int page = 1, int pageSize = 50, string? userId = null, string? action = null);
    }

    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(string action, string entity, string entityId, object? oldValues = null, object? newValues = null)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
                var userEmail = httpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? "system@demo.com";

                var auditLog = new AuditLog
                {
                    UserId = userId,
                    UserEmail = userEmail,
                    Action = action,
                    Entity = entity,
                    EntityId = entityId,
                    OldValues = oldValues != null ? System.Text.Json.JsonSerializer.Serialize(oldValues) : null,
                    NewValues = newValues != null ? System.Text.Json.JsonSerializer.Serialize(newValues) : null,
                    Timestamp = DateTime.UtcNow,
                    IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    UserAgent = httpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown"
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();
            }
            catch
            {
                // Silently handle audit failures to avoid breaking main functionality
            }
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(int page = 1, int pageSize = 50, string? userId = null, string? action = null)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(al => al.UserId == userId);

            if (!string.IsNullOrEmpty(action))
                query = query.Where(al => al.Action.Contains(action));

            return await query
                .OrderByDescending(al => al.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
