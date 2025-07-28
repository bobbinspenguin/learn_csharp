using BasicWebAPI.Models;

namespace BasicWebAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(CreateProductRequest request);
        Task<Product?> UpdateProductAsync(int id, UpdateProductRequest request);
        Task<bool> DeleteProductAsync(int id);
        Task<PaginatedResult<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, string? category = null);
    }

    public class ProductService : IProductService
    {
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Description = "High performance laptop", Price = 999.99m, Category = "Electronics", Stock = 50 },
            new Product { Id = 2, Name = "Mouse", Description = "Wireless optical mouse", Price = 29.99m, Category = "Electronics", Stock = 200 },
            new Product { Id = 3, Name = "Keyboard", Description = "Mechanical gaming keyboard", Price = 79.99m, Category = "Electronics", Stock = 150 },
            new Product { Id = 4, Name = "Monitor", Description = "24-inch LED monitor", Price = 299.99m, Category = "Electronics", Stock = 75 },
            new Product { Id = 5, Name = "Chair", Description = "Ergonomic office chair", Price = 199.99m, Category = "Furniture", Stock = 30 },
            new Product { Id = 6, Name = "Desk", Description = "Standing desk", Price = 399.99m, Category = "Furniture", Stock = 20 },
            new Product { Id = 7, Name = "Book", Description = "Programming book", Price = 24.99m, Category = "Education", Stock = 100 },
            new Product { Id = 8, Name = "Pen", Description = "Ballpoint pen", Price = 2.99m, Category = "Office", Stock = 500 }
        };

        private static int _nextId = 9;

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var activeProducts = _products.Where(p => p.IsActive).ToList();
            return Task.FromResult<IEnumerable<Product>>(activeProducts);
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            return Task.FromResult(product);
        }

        public Task<Product> CreateProductAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Id = _nextId++,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category,
                Stock = request.Stock,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product?> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (product == null)
                return Task.FromResult<Product?>(null);

            if (!string.IsNullOrEmpty(request.Name))
                product.Name = request.Name;
            
            if (!string.IsNullOrEmpty(request.Description))
                product.Description = request.Description;
            
            if (request.Price.HasValue)
                product.Price = request.Price.Value;
            
            if (!string.IsNullOrEmpty(request.Category))
                product.Category = request.Category;
            
            if (request.Stock.HasValue)
                product.Stock = request.Stock.Value;
            
            if (request.IsActive.HasValue)
                product.IsActive = request.IsActive.Value;

            product.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult<Product?>(product);
        }

        public Task<bool> DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return Task.FromResult(false);

            // Soft delete
            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult(true);
        }

        public Task<PaginatedResult<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, string? category = null)
        {
            var query = _products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new PaginatedResult<Product>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages
            };

            return Task.FromResult(result);
        }
    }
}
