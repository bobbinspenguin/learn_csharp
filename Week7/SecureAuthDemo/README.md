# Secure Authentication Demo

A comprehensive demonstration of security best practices in ASP.NET Core, featuring JWT authentication, role-based authorization, and audit logging.

## Features

### Authentication & Authorization
- JWT token-based authentication
- ASP.NET Core Identity integration
- Role-based access control (Admin, Manager, User)
- Password strength validation
- Account lockout protection
- Secure password hashing with Identity

### Security Features
- Input validation and sanitization
- SQL injection prevention through Entity Framework
- XSS protection with security headers
- CORS configuration
- HTTPS enforcement
- Security headers (HSTS, CSP, etc.)
- Global exception handling

### API Endpoints

#### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh-token` - Token refresh
- `POST /api/auth/change-password` - Password change
- `GET /api/auth/profile` - Get user profile
- `GET /api/auth/check-email` - Check email availability

#### Documents (Protected)
- `GET /api/documents` - List documents with access control
- `GET /api/documents/{id}` - Get specific document
- `POST /api/documents` - Create document
- `PUT /api/documents/{id}` - Update document
- `DELETE /api/documents/{id}` - Delete document

#### Admin (Admin Role Required)
- `GET /api/admin/audit-logs` - View audit logs
- `GET /api/admin/users` - List all users
- `GET /api/admin/statistics` - System statistics

### Access Levels
- **Public**: Accessible to all authenticated users
- **Internal**: Accessible to Managers and Admins
- **Private**: Accessible only to document owner and Admins
- **Confidential**: Accessible only to Admins

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQLite (included with .NET)

### Running the Application

1. Navigate to the project directory:
```bash
cd Week7/SecureAuthDemo
```

2. Restore packages:
```bash
dotnet restore
```

3. Run the application:
```bash
dotnet run
```

4. Open your browser and navigate to `https://localhost:5001` to access the Swagger UI.

### Default Admin Account
- Email: `admin@securedemo.com`
- Password: `Admin123!`

## Testing the API

### 1. Register a New User
```json
POST /api/auth/register
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}
```

### 2. Login
```json
POST /api/auth/login
{
  "email": "john@example.com",
  "password": "Password123!"
}
```

### 3. Create a Document (Requires Bearer Token)
```json
POST /api/documents
Authorization: Bearer YOUR_JWT_TOKEN

{
  "title": "My First Document",
  "content": "This is the content of my document.",
  "accessLevel": "Private"
}
```

## Security Best Practices Demonstrated

### 1. Authentication Security
- Strong password requirements
- JWT tokens with expiration
- Account lockout after failed attempts
- Secure password hashing

### 2. Authorization Security
- Role-based permissions
- Resource-level access control
- Principle of least privilege

### 3. Data Security
- Input validation on all endpoints
- Parameterized queries (via EF Core)
- Data sanitization
- Audit logging for all actions

### 4. API Security
- HTTPS enforcement
- Security headers
- CORS configuration
- Global exception handling

### 5. Database Security
- Connection string protection
- Proper entity relationships
- Soft delete implementation
- Audit trail maintenance

## Architecture

### Models
- `AppUser`: Extended Identity user with custom properties
- `AppRole`: Custom role with descriptions
- `SecureDocument`: Document entity with access control
- `AuditLog`: Audit trail for all actions

### Services
- `AuthService`: Handles authentication, authorization, and audit logging
- JWT token generation and validation
- Password strength validation

### Controllers
- `AuthController`: Authentication endpoints
- `DocumentsController`: CRUD operations with access control
- `AdminController`: Administrative functions

### Middleware
- Global exception handling
- Security headers
- JWT authentication
- CORS handling

## Learning Objectives Covered

1. **Authentication vs Authorization**: Clear separation and implementation
2. **JWT Implementation**: Token generation, validation, and refresh
3. **Role-Based Access Control**: Multi-level permission system
4. **Security Headers**: Protection against common attacks
5. **Input Validation**: Server-side validation and sanitization
6. **Audit Logging**: Complete audit trail for compliance
7. **Error Handling**: Secure error responses without information leakage

## Production Considerations

### Security Enhancements for Production
1. Use environment variables for JWT secrets
2. Implement refresh token rotation
3. Add rate limiting
4. Enable HTTPS everywhere
5. Use stronger password policies
6. Implement 2FA
7. Add API versioning
8. Use proper logging and monitoring

### Deployment Checklist
- [ ] Update JWT secret key
- [ ] Configure proper CORS origins
- [ ] Enable HTTPS
- [ ] Set up proper logging
- [ ] Configure database connections securely
- [ ] Implement health checks
- [ ] Set up monitoring and alerting

This project demonstrates enterprise-level security practices and can serve as a foundation for secure web applications.
