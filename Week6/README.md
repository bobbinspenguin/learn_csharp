# Week 6: Data Access and Entity Framework Core

## Day 1: Entity Framework Core

### Learning Objectives

- Understand Object-Relational Mapping (ORM) concepts
- Learn Entity Framework Core fundamentals
- Practice creating and querying databases using EF Core

### Resources

- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Getting Started with EF Core](https://docs.microsoft.com/en-us/ef/core/get-started/)
- [DbContext Class](https://docs.microsoft.com/en-us/ef/core/dbcontext-configuration/)
- [Migrations in EF Core](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

### Hands-on Tasks

1. EF Core Basics:
   - Create a console application that:
     - Sets up Entity Framework Core with SQLite or SQL Server
     - Defines entity classes and DbContext
     - Implements code-first approach with migrations
     - Performs basic CRUD operations

2. Data Relationships:
   - Create a project that demonstrates:
     - One-to-many relationships
     - Many-to-many relationships
     - Navigation properties
     - Eager loading vs. lazy loading

3. Advanced EF Core Features:
   - Create an application that uses:
     - Complex queries with LINQ and EF Core
     - Raw SQL queries when needed
     - Transactions and concurrency handling
     - Performance optimization techniques

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
