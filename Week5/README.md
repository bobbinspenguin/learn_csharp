# Week 5: Web API Development ✅ Complete

This week contains **1 comprehensive Web API project** that demonstrates modern ASP.NET Core development practices.

## Sample Project Overview

### BasicWebAPI - Complete REST API Implementation
**Location**: `Week5/BasicWebAPI/`

**Key Features**:
- **Full CRUD Operations**: Create, Read, Update, Delete for Products and Users
- **ASP.NET Core Web API**: Modern web API framework with .NET 8
- **Swagger/OpenAPI Integration**: Interactive API documentation and testing
- **Data Validation**: Model validation with DataAnnotations and custom validators  
- **Error Handling**: Global exception handling and consistent error responses
- **Pagination**: Efficient data pagination for large datasets
- **Sorting and Filtering**: Advanced query capabilities
- **Dependency Injection**: Service container and IoC patterns
- **Repository Pattern**: Data access abstraction layer
- **Response Formatting**: Consistent API response structure
- **HTTP Status Codes**: Proper REST API status code usage
- **CORS Configuration**: Cross-origin resource sharing setup
- **Health Checks**: API health monitoring endpoints
- **Logging**: Structured logging with built-in .NET logging

**API Endpoints**:
- `GET /api/products` - List products with pagination and filtering
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update existing product
- `DELETE /api/products/{id}` - Delete product
- `GET /api/users` - List users with pagination
- `POST /api/users` - Create new user
- `GET /health` - Health check endpoint

**Run**: `cd Week5/BasicWebAPI && dotnet run`

**Access Swagger UI**: Navigate to `https://localhost:7xxx/swagger` after running

## Learning Objectives Covered

✅ **ASP.NET Core Web API**: Understanding modern web API development  
✅ **RESTful Services**: Implementing REST architectural principles  
✅ **HTTP Methods**: Proper usage of GET, POST, PUT, DELETE  
✅ **Routing**: Attribute-based routing and route templates  
✅ **Model Binding**: Request data binding and validation  
✅ **Dependency Injection**: Service registration and IoC container  
✅ **OpenAPI/Swagger**: API documentation and testing tools  
✅ **Error Handling**: Global exception handling and error responses  
✅ **Data Validation**: Input validation and business rule enforcement  
✅ **Repository Pattern**: Data access layer abstraction  
✅ **CORS**: Cross-origin resource sharing configuration  
✅ **Health Checks**: API monitoring and diagnostics

3. .NET CLI Exercise:
   - Practice using the dotnet CLI to:
     - Create new projects
     - Build and run applications
     - Manage packages with NuGet
     - Run tests and generate reports

## Day 2: Introduction to API Development

### Learning Objectives

- Understand REST principles and API design concepts
- Learn how to create APIs using ASP.NET Core
- Explore the basics of Azure Functions and serverless computing

### Resources

- [Create web APIs with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [REST API Tutorial](https://restfulapi.net/)
- [Introduction to Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview)

### Hands-on Tasks

1. First API with ASP.NET Core:
   - Create an ASP.NET Core Web API project
   - Implement basic CRUD operations for a resource
   - Use proper HTTP methods and status codes
   - Test your API using Postman or Swagger

2. API Models and DTOs:
   - Create a project that:
     - Uses proper model classes
     - Implements Data Transfer Objects (DTOs)
     - Uses AutoMapper for object mapping
     - Validates input using Data Annotations or FluentValidation

3. First Azure Function:
   - Create a simple HTTP-triggered Azure Function
   - Implement basic CRUD operations using function triggers
   - Test the function locally using the Azure Functions Core Tools
   - Understand the differences between APIs and Azure Functions

### Code Samples

You'll find the following examples in the subdirectories:
- `DotNetExploration/`: Examples of different .NET projects
- `CrossPlatformDemo/`: Cross-platform application example
- `FirstWebApi/`: Basic ASP.NET Core Web API
- `FirstAzureFunction/`: Simple HTTP-triggered Azure Function

### Reflection Questions

1. What are the key differences between traditional web APIs and serverless functions?
2. How does the .NET ecosystem support cross-platform development?
3. What challenges did you encounter when designing your first API?
