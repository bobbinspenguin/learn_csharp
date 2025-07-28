using Microsoft.AspNetCore.Mvc;
using BasicWebAPI.Models;
using BasicWebAPI.Services;

namespace BasicWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of all active products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Product>>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Getting all products");
                var products = await _productService.GetAllProductsAsync();
                
                return Ok(new ApiResponse<IEnumerable<Product>>
                {
                    Success = true,
                    Data = products,
                    Message = "Products retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all products");
                return StatusCode(500, new ApiResponse<IEnumerable<Product>>
                {
                    Success = false,
                    Message = "Internal server error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Get products with pagination
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <param name="category">Optional category filter</param>
        /// <returns>Paginated list of products</returns>
        [HttpGet("paginated")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<Product>>), 200)]
        public async Task<ActionResult<ApiResponse<PaginatedResult<Product>>>> GetProductsPaginated(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] string? category = null)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                _logger.LogInformation("Getting products page {PageNumber}, size {PageSize}, category {Category}", 
                    pageNumber, pageSize, category);

                var result = await _productService.GetProductsPaginatedAsync(pageNumber, pageSize, category);
                
                return Ok(new ApiResponse<PaginatedResult<Product>>
                {
                    Success = true,
                    Data = result,
                    Message = "Products retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated products");
                return StatusCode(500, new ApiResponse<PaginatedResult<Product>>
                {
                    Success = false,
                    Message = "Internal server error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 404)]
        public async Task<ActionResult<ApiResponse<Product>>> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("Getting product with ID {ProductId}", id);
                var product = await _productService.GetProductByIdAsync(id);
                
                if (product == null)
                {
                    return NotFound(new ApiResponse<Product>
                    {
                        Success = false,
                        Message = $"Product with ID {id} not found"
                    });
                }

                return Ok(new ApiResponse<Product>
                {
                    Success = true,
                    Data = product,
                    Message = "Product retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product {ProductId}", id);
                return StatusCode(500, new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Internal server error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="request">Product creation request</param>
        /// <returns>Created product</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Product>), 201)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 400)]
        public async Task<ActionResult<ApiResponse<Product>>> CreateProduct([FromBody] CreateProductRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors
                    });
                }

                // Basic validation
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return BadRequest(new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Product name is required"
                    });
                }

                if (request.Price <= 0)
                {
                    return BadRequest(new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Price must be greater than zero"
                    });
                }

                _logger.LogInformation("Creating new product: {ProductName}", request.Name);
                var product = await _productService.CreateProductAsync(request);

                return CreatedAtAction(
                    nameof(GetProduct), 
                    new { id = product.Id }, 
                    new ApiResponse<Product>
                    {
                        Success = true,
                        Data = product,
                        Message = "Product created successfully"
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Internal server error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="request">Product update request</param>
        /// <returns>Updated product</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 404)]
        [ProducesResponseType(typeof(ApiResponse<Product>), 400)]
        public async Task<ActionResult<ApiResponse<Product>>> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            try
            {
                if (request.Price.HasValue && request.Price <= 0)
                {
                    return BadRequest(new ApiResponse<Product>
                    {
                        Success = false,
                        Message = "Price must be greater than zero"
                    });
                }

                _logger.LogInformation("Updating product with ID {ProductId}", id);
                var product = await _productService.UpdateProductAsync(id, request);

                if (product == null)
                {
                    return NotFound(new ApiResponse<Product>
                    {
                        Success = false,
                        Message = $"Product with ID {id} not found"
                    });
                }

                return Ok(new ApiResponse<Product>
                {
                    Success = true,
                    Data = product,
                    Message = "Product updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", id);
                return StatusCode(500, new ApiResponse<Product>
                {
                    Success = false,
                    Message = "Internal server error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Delete a product (soft delete)
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Deletion result</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Deleting product with ID {ProductId}", id);
                var success = await _productService.DeleteProductAsync(id);

                if (!success)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Product with ID {id} not found"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Product deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", id);
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Internal server error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        /// <summary>
        /// Get product categories
        /// </summary>
        /// <returns>List of unique categories</returns>
        [HttpGet("categories")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<string>>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<string>>>> GetCategories()
        {
            try
            {
                _logger.LogInformation("Getting product categories");
                var products = await _productService.GetAllProductsAsync();
                var categories = products.Select(p => p.Category).Distinct().OrderBy(c => c);

                return Ok(new ApiResponse<IEnumerable<string>>
                {
                    Success = true,
                    Data = categories,
                    Message = "Categories retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, new ApiResponse<IEnumerable<string>>
                {
                    Success = false,
                    Message = "Internal server error",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
