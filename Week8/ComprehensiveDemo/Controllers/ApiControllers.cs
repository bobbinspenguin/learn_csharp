using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComprehensiveDemo.Services;
using ComprehensiveDemo.Models;
using System.Security.Claims;

namespace ComprehensiveDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var result = await _authService.RegisterAsync(registerDto);
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var result = await _authService.LoginAsync(loginDto);
            
            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken()
        {
            var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var result = await _authService.RefreshTokenAsync(token);
            
            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<UserProfileDto> { Success = false, Message = "User not authenticated" });
            }

            var profile = await _authService.GetUserProfileAsync(userId);
            if (profile == null)
            {
                return NotFound(new ApiResponse<UserProfileDto> { Success = false, Message = "User profile not found" });
            }

            return Ok(new ApiResponse<UserProfileDto> { Success = true, Data = profile, Message = "Profile retrieved successfully" });
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckEmailUnique([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new ApiResponse<bool> { Success = false, Message = "Email is required" });
            }

            var isUnique = await _authService.IsEmailUniqueAsync(email);
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = isUnique,
                Message = isUnique ? "Email is available" : "Email is already taken"
            });
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? categoryId = null,
            [FromQuery] bool? isFeatured = null)
        {
            var result = await _productService.GetProductsAsync(page, pageSize, categoryId, isFeatured);
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetProduct(int id)
        {
            var result = await _productService.GetProductAsync(id);
            
            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var result = await _productService.CreateProductAsync(productDto);
            
            if (result.Success)
                return CreatedAtAction(nameof(GetProduct), new { id = result.Data!.Id }, result);

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(int id, [FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var result = await _productService.UpdateProductAsync(id, productDto);
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetLowStockProducts()
        {
            var result = await _productService.GetLowStockProductsAsync();
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? status = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            // Regular users can only see their own orders
            string? filterUserId = null;
            if (!userRoles.Contains("Admin") && !userRoles.Contains("Manager"))
            {
                filterUserId = userId;
            }

            var result = await _orderService.GetOrdersAsync(page, pageSize, filterUserId, status);
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrder(int id)
        {
            var result = await _orderService.GetOrderAsync(id);
            
            if (!result.Success)
                return NotFound(result);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            // Check if user can access this order
            if (!userRoles.Contains("Admin") && !userRoles.Contains("Manager") && result.Data!.CustomerEmail != User.FindFirst(ClaimTypes.Email)?.Value)
            {
                return Forbid();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var result = await _orderService.CreateOrderAsync(userId, orderDto);
            
            if (result.Success)
                return CreatedAtAction(nameof(GetOrder), new { id = result.Data!.Id }, result);

            return BadRequest(result);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var result = await _orderService.UpdateOrderStatusAsync(id, statusDto.Status);
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public DashboardController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("stats")]
        public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetDashboardStats()
        {
            var result = await _orderService.GetDashboardStatsAsync();
            
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AdminController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet("audit-logs")]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuditLog>>>> GetAuditLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? userId = null,
            [FromQuery] string? action = null)
        {
            try
            {
                var auditLogs = await _auditService.GetAuditLogsAsync(page, pageSize, userId, action);
                
                return Ok(new ApiResponse<IEnumerable<AuditLog>>
                {
                    Success = true,
                    Data = auditLogs,
                    Message = $"Retrieved {auditLogs.Count()} audit logs",
                    PageNumber = page,
                    PageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<IEnumerable<AuditLog>>
                {
                    Success = false,
                    Message = "Failed to retrieve audit logs",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }

    // Additional DTOs
    public class UpdateOrderStatusDto
    {
        [Required]
        [RegularExpression("^(Pending|Processing|Shipped|Delivered|Cancelled)$", ErrorMessage = "Invalid status")]
        public string Status { get; set; } = string.Empty;
    }
}
