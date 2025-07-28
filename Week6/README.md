# Week 6: Entity Framework and Database Integration ✅ Complete

This week contains **1 comprehensive database project** that demonstrates Entity Framework Core with complete CRUD operations and advanced database concepts.

## Sample Project Overview

### EntityFrameworkDemo - Complete School Management System
**Location**: `Week6/EntityFrameworkDemo/`

**Key Features**:
- **Entity Framework Core 8.0**: Latest ORM framework with .NET 8
- **SQLite Database**: Lightweight database for development and learning
- **Code-First Approach**: Database creation from C# model classes
- **Database Migrations**: Version control for database schema changes
- **Complete Domain Model**: Students, Courses, Enrollments, Teachers, Departments
- **Relationships**: One-to-Many, Many-to-Many, and Foreign Key relationships
- **CRUD Operations**: Full Create, Read, Update, Delete functionality
- **LINQ Integration**: Advanced querying with Entity Framework LINQ provider
- **Navigation Properties**: Efficient related data loading
- **Database Seeding**: Initial data population for testing
- **Connection String Management**: Database configuration and connection handling
- **Interactive Console Interface**: Menu-driven database operations
- **Data Validation**: Model validation and business rule enforcement
- **Performance Optimization**: Efficient queries and data loading strategies

**Domain Models**:
- **Student**: ID, Name, Email, EnrollmentDate, Enrollments
- **Course**: ID, Title, Credits, DepartmentId, Department, Enrollments
- **Enrollment**: StudentId, CourseId, EnrollmentDate, Grade
- **Teacher**: ID, Name, Email, DepartmentId, Department
- **Department**: ID, Name, Budget, Teachers, Courses

**Operations Available**:
- Add/Edit/Delete Students, Courses, Teachers, Departments
- Enroll students in courses
- View enrollment reports and statistics
- Search and filter data across all entities
- Generate academic reports

**Run**: `cd Week6/EntityFrameworkDemo && dotnet run`

## Learning Objectives Covered

✅ **Entity Framework Core**: Understanding modern ORM frameworks  
✅ **Code-First Development**: Creating databases from C# models  
✅ **Database Migrations**: Managing schema changes over time  
✅ **Relationships**: Implementing foreign keys and navigation properties  
✅ **LINQ with EF**: Advanced querying capabilities  
✅ **DbContext**: Database context configuration and usage  
✅ **Connection Strings**: Database connection management  
✅ **Data Seeding**: Populating databases with initial data  
✅ **Model Validation**: Ensuring data integrity and business rules  
✅ **Navigation Properties**: Efficient related data access  
✅ **Database Design**: Relational database design principles  
✅ **Performance**: Query optimization and efficient data loading

## Day 2: Working with APIs and Data

### Learning Objectives

- Connect APIs to databases using Entity Framework Core
- Implement repository pattern for data access
- Practice building a complete data-driven API

### Resources

- [ASP.NET Core with EF Core](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [API Data Access Best Practices](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)

### Hands-on Tasks

1. API with EF Core:
   - Create an ASP.NET Core Web API that:
     - Uses Entity Framework Core for data access
     - Implements proper repository pattern
     - Handles database operations efficiently
     - Returns appropriate status codes and responses

2. Azure Function with Database:
   - Create an Azure Function that:
     - Connects to a database using Entity Framework Core
     - Performs CRUD operations on data
     - Implements proper error handling for database operations
     - Uses dependency injection for the DbContext

3. API Features Implementation:
   - Enhance your API with:
     - Pagination for large data sets
     - Sorting and filtering options
     - Data validation and error handling
     - Proper response formatting

### Code Samples

You'll find the following examples in the subdirectories:
- `EfCoreBasics/`: Basic Entity Framework Core setup and usage
- `DataRelationships/`: Examples of different relationship types
- `ApiWithEfCore/`: Complete Web API with EF Core
- `FunctionWithDatabase/`: Azure Function with database access

### Reflection Questions

1. How does Entity Framework Core simplify database operations compared to raw SQL?
2. What are the trade-offs of using an ORM like Entity Framework Core?
3. What challenges did you encounter when connecting your API to a database?
