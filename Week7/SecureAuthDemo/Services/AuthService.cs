using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SecureAuthDemo.Data;
using SecureAuthDemo.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecureAuthDemo.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
        Task<UserProfileDto?> GetUserProfileAsync(string userId);
        Task<bool> IsEmailUniqueAsync(string email);
        Task LogAuditAsync(string userId, string action, string entity, string entityId, object? oldValues = null, object? newValues = null);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration,
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if email already exists
                if (!await IsEmailUniqueAsync(registerDto.Email))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                // Validate password strength
                var passwordValidation = ValidatePasswordStrength(registerDto.Password);
                if (!passwordValidation.IsValid)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = passwordValidation.Message
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
                    // Assign default role
                    await _userManager.AddToRoleAsync(user, "User");

                    // Log audit
                    await LogAuditAsync(user.Id, "Register", "User", user.Id, null, new { Email = user.Email, FullName = user.FullName });

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
                    // Update last login
                    user.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    // Log audit
                    await LogAuditAsync(user.Id, "Login", "User", user.Id);

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

                // Log failed login attempt
                if (user != null)
                {
                    await LogAuditAsync(user.Id, "LoginFailed", "User", user.Id);
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
                    ValidateLifetime = false, // Don't validate expiry for refresh
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

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                var passwordValidation = ValidatePasswordStrength(changePasswordDto.NewPassword);
                if (!passwordValidation.IsValid)
                    return false;

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    await LogAuditAsync(userId, "PasswordChanged", "User", userId);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return null;

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

        private static (bool IsValid, string Message) ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password is required");

            if (password.Length < 8)
                return (false, "Password must be at least 8 characters long");

            if (!password.Any(char.IsUpper))
                return (false, "Password must contain at least one uppercase letter");

            if (!password.Any(char.IsLower))
                return (false, "Password must contain at least one lowercase letter");

            if (!password.Any(char.IsDigit))
                return (false, "Password must contain at least one number");

            if (!password.Any(c => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c)))
                return (false, "Password must contain at least one special character");

            return (true, "Password is valid");
        }

        public async Task LogAuditAsync(string userId, string action, string entity, string entityId, object? oldValues = null, object? newValues = null)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var httpContext = _httpContextAccessor.HttpContext;

                var auditLog = new AuditLog
                {
                    UserId = userId,
                    UserEmail = user?.Email ?? "Unknown",
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
                // Log audit failures silently to avoid breaking the main flow
            }
        }
    }
}
