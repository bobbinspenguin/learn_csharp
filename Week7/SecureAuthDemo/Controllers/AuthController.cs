using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAuthDemo.Data;
using SecureAuthDemo.Models;
using SecureAuthDemo.Services;
using System.Security.Claims;

namespace SecureAuthDemo.Controllers
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

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken()
        {
            var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            var result = await _authService.RefreshTokenAsync(token);
            
            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
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

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<object> { Success = false, Message = "User not authenticated" });
            }

            var success = await _authService.ChangePasswordAsync(userId, changePasswordDto);
            
            if (success)
            {
                return Ok(new ApiResponse<object> { Success = true, Message = "Password changed successfully" });
            }

            return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to change password" });
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
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public DocumentsController(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<DocumentDto>>>> GetDocuments([FromQuery] string? accessLevel = null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            var query = _context.SecureDocuments
                .Include(d => d.Owner)
                .Where(d => !d.IsDeleted);

            // Apply access control
            if (!userRoles.Contains("Admin"))
            {
                query = query.Where(d =>
                    d.OwnerId == userId || // Own documents
                    d.AccessLevel == "Public" || // Public documents
                    (d.AccessLevel == "Internal" && userRoles.Contains("Manager")) // Internal for managers
                );
            }

            if (!string.IsNullOrEmpty(accessLevel))
            {
                query = query.Where(d => d.AccessLevel == accessLevel);
            }

            var documents = await query
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Content = d.Content,
                    AccessLevel = d.AccessLevel,
                    OwnerName = d.Owner.FullName,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt,
                    CanEdit = d.OwnerId == userId || userRoles.Contains("Admin"),
                    CanDelete = d.OwnerId == userId || userRoles.Contains("Admin")
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<DocumentDto>>
            {
                Success = true,
                Data = documents,
                Message = $"Retrieved {documents.Count} documents"
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<DocumentDto>>> GetDocument(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            var document = await _context.SecureDocuments
                .Include(d => d.Owner)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (document == null)
            {
                return NotFound(new ApiResponse<DocumentDto> { Success = false, Message = "Document not found" });
            }

            // Check access permissions
            if (!CanAccessDocument(document, userId, userRoles))
            {
                return Forbid();
            }

            var documentDto = new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                AccessLevel = document.AccessLevel,
                OwnerName = document.Owner.FullName,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                CanEdit = document.OwnerId == userId || userRoles.Contains("Admin"),
                CanDelete = document.OwnerId == userId || userRoles.Contains("Admin")
            };

            return Ok(new ApiResponse<DocumentDto>
            {
                Success = true,
                Data = documentDto,
                Message = "Document retrieved successfully"
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<DocumentDto>>> CreateDocument([FromBody] CreateDocumentDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<DocumentDto>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

            // Validate access level permissions
            if (!CanCreateDocumentWithAccessLevel(createDto.AccessLevel, User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()))
            {
                return Forbid("Insufficient permissions to create document with this access level");
            }

            var document = new SecureDocument
            {
                Title = createDto.Title,
                Content = createDto.Content,
                AccessLevel = createDto.AccessLevel,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SecureDocuments.Add(document);
            await _context.SaveChangesAsync();

            // Log audit
            await _authService.LogAuditAsync(userId, "Create", "Document", document.Id.ToString(), null, new { document.Title, document.AccessLevel });

            var documentDto = new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                AccessLevel = document.AccessLevel,
                OwnerName = User.Identity!.Name!,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                CanEdit = true,
                CanDelete = true
            };

            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, new ApiResponse<DocumentDto>
            {
                Success = true,
                Data = documentDto,
                Message = "Document created successfully"
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<DocumentDto>>> UpdateDocument(int id, [FromBody] UpdateDocumentDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<DocumentDto>
                {
                    Success = false,
                    Message = "Invalid input data",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            var document = await _context.SecureDocuments
                .Include(d => d.Owner)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (document == null)
            {
                return NotFound(new ApiResponse<DocumentDto> { Success = false, Message = "Document not found" });
            }

            // Check edit permissions
            if (document.OwnerId != userId && !userRoles.Contains("Admin"))
            {
                return Forbid();
            }

            // Validate access level permissions
            if (!CanCreateDocumentWithAccessLevel(updateDto.AccessLevel, userRoles))
            {
                return Forbid("Insufficient permissions to set this access level");
            }

            var oldValues = new { document.Title, document.Content, document.AccessLevel };

            document.Title = updateDto.Title;
            document.Content = updateDto.Content;
            document.AccessLevel = updateDto.AccessLevel;
            document.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Log audit
            await _authService.LogAuditAsync(userId, "Update", "Document", document.Id.ToString(), oldValues, new { document.Title, document.Content, document.AccessLevel });

            var documentDto = new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                AccessLevel = document.AccessLevel,
                OwnerName = document.Owner.FullName,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt,
                CanEdit = true,
                CanDelete = document.OwnerId == userId || userRoles.Contains("Admin")
            };

            return Ok(new ApiResponse<DocumentDto>
            {
                Success = true,
                Data = documentDto,
                Message = "Document updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteDocument(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            var document = await _context.SecureDocuments
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);

            if (document == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Document not found" });
            }

            // Check delete permissions
            if (document.OwnerId != userId && !userRoles.Contains("Admin"))
            {
                return Forbid();
            }

            document.IsDeleted = true;
            document.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Log audit
            await _authService.LogAuditAsync(userId, "Delete", "Document", document.Id.ToString(), new { document.Title, document.AccessLevel }, null);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Document deleted successfully"
            });
        }

        private static bool CanAccessDocument(SecureDocument document, string userId, List<string> userRoles)
        {
            // Admin can access everything
            if (userRoles.Contains("Admin"))
                return true;

            // Owner can access their own documents
            if (document.OwnerId == userId)
                return true;

            // Public documents are accessible to everyone
            if (document.AccessLevel == "Public")
                return true;

            // Internal documents are accessible to managers and above
            if (document.AccessLevel == "Internal" && userRoles.Contains("Manager"))
                return true;

            // Confidential documents are only for admins and owners (already checked above)
            return false;
        }

        private static bool CanCreateDocumentWithAccessLevel(string accessLevel, List<string> userRoles)
        {
            return accessLevel switch
            {
                "Public" or "Private" => true, // Anyone can create public or private documents
                "Internal" => userRoles.Contains("Manager") || userRoles.Contains("Admin"),
                "Confidential" => userRoles.Contains("Admin"),
                _ => false
            };
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("audit-logs")]
        public async Task<ActionResult<ApiResponse<List<AuditLog>>>> GetAuditLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? userId = null,
            [FromQuery] string? action = null)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(al => al.UserId == userId);

            if (!string.IsNullOrEmpty(action))
                query = query.Where(al => al.Action.Contains(action));

            var totalRecords = await query.CountAsync();
            var auditLogs = await query
                .OrderByDescending(al => al.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new ApiResponse<List<AuditLog>>
            {
                Success = true,
                Data = auditLogs,
                Message = $"Retrieved {auditLogs.Count} of {totalRecords} audit logs"
            });
        }

        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<List<object>>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.CreatedAt,
                    u.LastLoginAt,
                    u.IsActive
                })
                .OrderBy(u => u.LastName)
                .ToListAsync();

            return Ok(new ApiResponse<List<object>>
            {
                Success = true,
                Data = users.Cast<object>().ToList(),
                Message = $"Retrieved {users.Count} users"
            });
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<ApiResponse<object>>> GetStatistics()
        {
            var stats = new
            {
                TotalUsers = await _context.Users.CountAsync(),
                ActiveUsers = await _context.Users.CountAsync(u => u.IsActive),
                TotalDocuments = await _context.SecureDocuments.CountAsync(d => !d.IsDeleted),
                DocumentsByAccessLevel = await _context.SecureDocuments
                    .Where(d => !d.IsDeleted)
                    .GroupBy(d => d.AccessLevel)
                    .Select(g => new { AccessLevel = g.Key, Count = g.Count() })
                    .ToListAsync(),
                RecentLogins = await _context.Users
                    .Where(u => u.LastLoginAt.HasValue)
                    .CountAsync(u => u.LastLoginAt.Value >= DateTime.UtcNow.AddDays(-7)),
                AuditLogCount = await _context.AuditLogs.CountAsync()
            };

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Data = stats,
                Message = "Statistics retrieved successfully"
            });
        }
    }
}
