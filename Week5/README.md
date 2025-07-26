# Week 5: .NET Core and API Development

## Day 1: Understanding .NET Core and CLR

### Learning Objectives

- Understand the .NET ecosystem (.NET Framework vs .NET Core vs .NET 8)
- Learn about the Common Language Runtime (CLR) and its role
- Explore the .NET Standard and cross-platform capabilities

### Resources

- [Introduction to .NET](https://docs.microsoft.com/en-us/dotnet/core/introduction)
- [.NET Fundamentals](https://docs.microsoft.com/en-us/dotnet/fundamentals/)
- [Common Language Runtime (CLR) Overview](https://docs.microsoft.com/en-us/dotnet/standard/clr)
- [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

### Hands-on Tasks

1. .NET Exploration:
   - Create projects using different .NET versions
   - Compare project structures and capabilities
   - Explore the SDK tools using the dotnet CLI
   - Understand target frameworks and their implications

2. Cross-Platform Development:
   - Create a simple console application that:
     - Works across different operating systems
     - Uses platform-specific code when needed
     - Implements proper configuration for cross-platform deployment

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
