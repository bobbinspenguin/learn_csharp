using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ComprehensiveDemo.Data;
using ComprehensiveDemo.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ComprehensiveDemo.Services
{
    // Authentication Service
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<UserProfileDto?> GetUserProfileAsync(string userId);
        Task<bool> IsEmailUniqueAsync(string email);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAuditService _auditService;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            IAuditService auditService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _auditService = auditService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (!await IsEmailUniqueAsync(registerDto.Email))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                var user = new AppUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _auditService.LogAsync("Register", "User", user.Id, null, new { user.Email, user.FullName });

                    var token = await GenerateJwtTokenAsync(user);
                    var userProfile = await GetUserProfileAsync(user.Id);

                    return new AuthResponseDto
                    {
                        Success = true,
                        Message = "Registration successful",
                        Token = token.Token,
                        ExpiresAt = token.ExpiresAt,
                        User = userProfile
                    };
                }

                return new AuthResponseDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Registration failed: " + ex.Message
                };
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null || !user.IsActive)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    user.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    await _auditService.LogAsync("Login", "User", user.Id);

                    var token = await GenerateJwtTokenAsync(user);
                    var userProfile = await GetUserProfileAsync(user.Id);

                    return new AuthResponseDto
                    {
                        Success = true,
                        Message = "Login successful",
                        Token = token.Token,
                        ExpiresAt = token.ExpiresAt,
                        User = userProfile
                    };
                }

                if (result.IsLockedOut)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Account is locked due to multiple failed login attempts"
                    };
                }

                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Login failed: " + ex.Message
                };
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return new AuthResponseDto { Success = false, Message = "Invalid token" };
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || !user.IsActive)
                {
                    return new AuthResponseDto { Success = false, Message = "User not found or inactive" };
                }

                var newToken = await GenerateJwtTokenAsync(user);
                var userProfile = await GetUserProfileAsync(user.Id);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    Token = newToken.Token,
                    ExpiresAt = newToken.ExpiresAt,
                    User = userProfile
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Token refresh failed: " + ex.Message
                };
            }
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return null;

                var roles = await _userManager.GetRolesAsync(user);

                return new UserProfileDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    FullName = user.FullName,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    Roles = roles.ToList()
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user == null;
        }

        private async Task<(string Token, DateTime ExpiresAt)> GenerateJwtTokenAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Email, user.Email!),
                new("FirstName", user.FirstName),
                new("LastName", user.LastName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
    }

    // Order Service
    public interface IOrderService
    {
        Task<ApiResponse<IEnumerable<OrderDto>>> GetOrdersAsync(int page = 1, int pageSize = 10, string? userId = null, string? status = null);
        Task<ApiResponse<OrderDto>> GetOrderAsync(int id);
        Task<ApiResponse<OrderDto>> CreateOrderAsync(string userId, CreateOrderDto orderDto);
        Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int id, string status);
        Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            AppDbContext context,
            IAuditService auditService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _context = context;
            _auditService = auditService;
        }

        public async Task<ApiResponse<IEnumerable<OrderDto>>> GetOrdersAsync(int page = 1, int pageSize = 10, string? userId = null, string? status = null)
        {
            try
            {
                var orders = await _orderRepository.GetOrdersWithDetailsAsync(page, pageSize, userId, status);
                return new ApiResponse<IEnumerable<OrderDto>>
                {
                    Success = true,
                    Data = orders,
                    Message = $"Retrieved {orders.Count()} orders",
                    PageNumber = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<OrderDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve orders",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<OrderDto>> GetOrderAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(id);
                if (order == null)
                {
                    return new ApiResponse<OrderDto>
                    {
                        Success = false,
                        Message = "Order not found"
                    };
                }

                return new ApiResponse<OrderDto>
                {
                    Success = true,
                    Data = order,
                    Message = "Order retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Failed to retrieve order",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<OrderDto>> CreateOrderAsync(string userId, CreateOrderDto orderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate products and calculate totals
                decimal subtotal = 0;
                var orderItems = new List<OrderItem>();

                foreach (var itemDto in orderDto.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                    if (product == null)
                    {
                        return new ApiResponse<OrderDto>
                        {
                            Success = false,
                            Message = $"Product with ID {itemDto.ProductId} not found"
                        };
                    }

                    if (product.StockQuantity < itemDto.Quantity)
                    {
                        return new ApiResponse<OrderDto>
                        {
                            Success = false,
                            Message = $"Insufficient stock for product {product.Name}. Available: {product.StockQuantity}, Requested: {itemDto.Quantity}"
                        };
                    }

                    var unitPrice = product.SalePrice ?? product.Price;
                    var totalPrice = unitPrice * itemDto.Quantity;
                    subtotal += totalPrice;

                    orderItems.Add(new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = totalPrice
                    });

                    // Update stock
                    product.StockQuantity -= itemDto.Quantity;
                    await _productRepository.UpdateAsync(product);
                }

                // Calculate tax and shipping
                var taxRate = 0.08m; // 8% tax
                var taxAmount = subtotal * taxRate;
                var shippingAmount = subtotal > 100 ? 0 : 10; // Free shipping over $100
                var totalAmount = subtotal + taxAmount + shippingAmount;

                // Create order
                var order = new Order
                {
                    OrderNumber = await _orderRepository.GenerateOrderNumberAsync(),
                    UserId = userId,
                    SubTotal = subtotal,
                    TaxAmount = taxAmount,
                    ShippingAmount = shippingAmount,
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ShippingStreet = orderDto.ShippingAddress.Street,
                    ShippingCity = orderDto.ShippingAddress.City,
                    ShippingState = orderDto.ShippingAddress.State,
                    ShippingZipCode = orderDto.ShippingAddress.ZipCode,
                    ShippingCountry = orderDto.ShippingAddress.Country,
                    OrderItems = orderItems
                };

                var createdOrder = await _orderRepository.AddAsync(order);
                await transaction.CommitAsync();

                await _auditService.LogAsync("Create", "Order", createdOrder.Id.ToString(), null, orderDto);

                var orderDetails = await _orderRepository.GetOrderWithDetailsAsync(createdOrder.Id);
                return new ApiResponse<OrderDto>
                {
                    Success = true,
                    Data = orderDetails,
                    Message = "Order created successfully"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Failed to create order",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<OrderDto>> UpdateOrderStatusAsync(int id, string status)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return new ApiResponse<OrderDto>
                    {
                        Success = false,
                        Message = "Order not found"
                    };
                }

                var oldStatus = order.Status;
                order.Status = status;
                order.UpdatedAt = DateTime.UtcNow;

                if (status == "Shipped" && order.ShippedAt == null)
                    order.ShippedAt = DateTime.UtcNow;
                
                if (status == "Delivered" && order.DeliveredAt == null)
                    order.DeliveredAt = DateTime.UtcNow;

                await _orderRepository.UpdateAsync(order);
                await _auditService.LogAsync("StatusUpdate", "Order", id.ToString(), new { OldStatus = oldStatus }, new { NewStatus = status });

                var orderDetails = await _orderRepository.GetOrderWithDetailsAsync(id);
                return new ApiResponse<OrderDto>
                {
                    Success = true,
                    Data = orderDetails,
                    Message = "Order status updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Failed to update order status",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStatsAsync()
        {
            try
            {
                var stats = new DashboardStatsDto
                {
                    TotalProducts = await _context.Products.CountAsync(p => p.IsActive),
                    TotalOrders = await _context.Orders.CountAsync(),
                    TotalCustomers = await _context.Users.CountAsync(u => u.IsActive),
                    TotalRevenue = await _context.Orders.Where(o => o.Status == "Delivered").SumAsync(o => o.TotalAmount),
                    LowStockProducts = await _context.Products.CountAsync(p => p.IsActive && p.StockQuantity <= p.ReorderLevel),
                    PendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending"),
                    
                    TopSellingProducts = await _context.OrderItems
                        .Include(oi => oi.Product)
                        .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                        .Select(g => new TopSellingProductDto
                        {
                            ProductName = g.Key.Name,
                            TotalSold = g.Sum(oi => oi.Quantity),
                            Revenue = g.Sum(oi => oi.TotalPrice)
                        })
                        .OrderByDescending(p => p.TotalSold)
                        .Take(5)
                        .ToListAsync(),
                    
                    RecentOrders = await _context.Orders
                        .Include(o => o.User)
                        .OrderByDescending(o => o.CreatedAt)
                        .Take(5)
                        .Select(o => new RecentOrderDto
                        {
                            OrderNumber = o.OrderNumber,
                            CustomerName = o.User.FullName,
                            TotalAmount = o.TotalAmount,
                            Status = o.Status,
                            CreatedAt = o.CreatedAt
                        })
                        .ToListAsync()
                };

                return new ApiResponse<DashboardStatsDto>
                {
                    Success = true,
                    Data = stats,
                    Message = "Dashboard statistics retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<DashboardStatsDto>
                {
                    Success = false,
                    Message = "Failed to retrieve dashboard statistics",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }

    // Additional DTOs for Authentication
    public class RegisterDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserProfileDto? User { get; set; }
    }
}
