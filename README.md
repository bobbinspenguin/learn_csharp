# C# Learning Repository - Complete 8-Week Curriculum

This repository contains a comprehensive 8-week learning path for mastering C# and .NET development, featuring 15+ hands-on projects progressing from basic syntax to enterprise-level applications. Each week includes complete sample code, detailed explanations, and practical exercises.

## Repository Structure

```
Week1/                   # C# Fundamentals (✅ Complete - 3 Projects)
├── HelloWorld/          # Basic C# console application
├── BasicSyntaxPractice/ # Variables, data types, operators
└── LoopsExercise/      # Interactive loop demonstrations

Week2/                   # Object-Oriented Programming (✅ Complete - 4 Projects)
├── ClassBasics/        # Basic class concepts and encapsulation
├── InheritanceDemo/    # Inheritance and polymorphism
├── InterfacesAndAbstracts/ # Interfaces and abstract classes
└── ExceptionHandlingDemo/ # Banking system with error handling

Week3/                   # Collections and Data (✅ Complete - 2 Projects)
├── CollectionsDemo/    # Lists, dictionaries, sets, queues, stacks
└── LinqDemo/          # Comprehensive LINQ operations and queries

Week4/                   # Advanced C# Features (✅ Complete - 2 Projects)
├── AsyncProgrammingDemo/ # async/await, Task, cancellation tokens
└── LambdaExpressionsDemo/ # Lambda expressions, delegates, closures

Week5/                   # Web API Development (✅ Complete - 1 Project)
└── BasicWebAPI/        # ASP.NET Core Web API with CRUD, Swagger, validation

Week6/                   # Entity Framework & Databases (✅ Complete - 1 Project)  
└── EntityFrameworkDemo/ # EF Core with SQLite, migrations, relationships

Week7/                   # Security & Authentication (✅ Complete - 1 Project)
└── SecureAuthDemo/     # JWT authentication, roles, audit logging

Week8/                   # Enterprise Integration (✅ Complete - 1 Project)
└── ComprehensiveDemo/  # Complete e-commerce application with all features
```

## Complete Sample Projects Overview

### Week 1: C# Fundamentals (3 Interactive Projects)
- **HelloWorld**: Basic console I/O and program structure
- **BasicSyntaxPractice**: Variables, operators, control structures with interactive demos
- **LoopsExercise**: Multiplication tables, sum calculators, number validation

### Week 2: Object-Oriented Programming (4 Comprehensive Projects)
- **ClassBasics**: Simple class creation and object instantiation
- **InheritanceDemo**: Person/Student/Teacher hierarchy with polymorphism
- **InterfacesAndAbstracts**: Shape interfaces and Animal abstract classes  
- **ExceptionHandlingDemo**: Banking system with custom exceptions and error handling

### Week 3: Collections and Data (2 Data-Focused Projects)
- **CollectionsDemo**: Arrays, Lists, Dictionaries, HashSets, Queues, Stacks, and custom generics
- **LinqDemo**: Filtering, projection, grouping, joins, aggregations with real datasets

### Week 4: Advanced C# Features (2 Modern C# Projects)
- **AsyncProgrammingDemo**: Web scraping, file I/O, concurrent operations with cancellation
- **LambdaExpressionsDemo**: Functional programming, delegates, closures, expression trees

### Week 5: Web API Development (1 Complete REST API)
- **BasicWebAPI**: Full CRUD operations, validation, pagination, Swagger documentation, error handling

### Week 6: Entity Framework & Databases (1 Database Project)
- **EntityFrameworkDemo**: Complete school management system with EF Core, SQLite, migrations, relationships, and CRUD operations

### Week 7: Security & Authentication (1 Security-Focused Project)
- **SecureAuthDemo**: JWT authentication, role-based authorization, password hashing, audit logging, rate limiting, and security headers

### Week 8: Enterprise Integration (1 Comprehensive Application)
- **ComprehensiveDemo**: Full-featured e-commerce platform with:
  - Complete domain models (Products, Orders, Users, Categories, Reviews, etc.)
  - Entity Framework with SQLite database
  - Repository pattern and business services
  - JWT authentication and authorization
  - Full REST API with comprehensive endpoints
  - Security features and audit trails
  - Swagger documentation and health checks
  - Structured logging with Serilog
  - Rate limiting and performance optimization

## Getting Started

### Prerequisites

- Install .NET 8 SDK
- Install Visual Studio, VS Code, or JetBrains Rider

### Quick Start

```bash
# Clone the repository
git clone <repository-url>
cd learn_csharp

# Restore all dependencies
dotnet restore

# Build the entire solution
dotnet build
```

### Running Projects

**Console Applications (Weeks 1-4):**
```bash
# Example: Week 1 HelloWorld
cd Week1/HelloWorld
dotnet run

# Example: Week 2 Banking System
cd Week2/ExceptionHandlingDemo
dotnet run
```

**Web Applications (Weeks 5-8):**
```bash
# Week 5: Basic Web API (visit https://localhost:7xxx/swagger)
cd Week5/BasicWebAPI
dotnet run

# Week 6: Entity Framework Demo
cd Week6/EntityFrameworkDemo
dotnet run

# Week 7: Secure Auth Demo
cd Week7/SecureAuthDemo
dotnet run

# Week 8: Comprehensive E-commerce Demo
cd Week8/ComprehensiveDemo
dotnet run
```

**Open in IDE:**
- Visual Studio: Open `CSharpLearning.sln`
- VS Code: Open `learn_csharp.code-workspace`

## Learning Features

✨ **Interactive Learning**: Each project includes menu-driven demos to explore concepts hands-on  
📚 **Progressive Complexity**: Builds from basic syntax to enterprise-level applications  
🔧 **Real-World Examples**: Banking systems, e-commerce platforms, school management, auth systems  
💡 **Best Practices**: Industry-standard patterns, error handling, security, and code organization  
📊 **Performance Analysis**: Timing comparisons, async patterns, and optimization techniques  
🛡️ **Security Focus**: JWT authentication, encryption, audit logging, and secure coding practices  

## Complete Learning Path Status

| Week | Topic | Projects | Status |
|------|-------|----------|---------|
| 1 | C# Fundamentals | 3 Interactive Projects | ✅ Complete |
| 2 | Object-Oriented Programming | 4 Comprehensive Projects | ✅ Complete |
| 3 | Collections & LINQ | 2 Data-Focused Projects | ✅ Complete |
| 4 | Advanced C# Features | 2 Modern C# Projects | ✅ Complete |
| 5 | Web API Development | 1 Complete REST API | ✅ Complete |
| 6 | Entity Framework & Databases | 1 Database Project | ✅ Complete |
| 7 | Security & Authentication | 1 Security Project | ✅ Complete |
| 8 | Enterprise Integration | 1 Enterprise Application | ✅ Complete |

**Total: 15+ Complete Sample Projects** covering the entire spectrum from basic C# syntax to enterprise-level applications.

## Key Technologies Covered

- **.NET 8**: Latest framework with modern C# features
- **ASP.NET Core**: Web API development and middleware
- **Entity Framework Core**: Database operations and migrations  
- **SQLite**: Lightweight database for development
- **JWT Authentication**: Secure token-based authentication
- **Swagger/OpenAPI**: API documentation and testing
- **Serilog**: Structured logging and monitoring
- **xUnit**: Unit testing framework
- **Async/Await**: Modern asynchronous programming
- **LINQ**: Language-integrated query for data manipulation

## Learning Objectives Achieved

By completing all 8 weeks, you will have:

- ✅ Mastered C# fundamentals and syntax
- ✅ Built object-oriented applications with proper design patterns
- ✅ Implemented data structures and LINQ queries
- ✅ Created asynchronous and high-performance applications
- ✅ Developed REST APIs with proper validation and documentation
- ✅ Integrated databases using Entity Framework Core
- ✅ Implemented security features and authentication systems
- ✅ Built a complete enterprise-level application

## Next Steps

This curriculum provides a solid foundation for:
- **Azure Functions Development**: Serverless computing
- **Microservices Architecture**: Distributed systems
- **Advanced Security**: OAuth, OIDC, and enterprise auth
- **Cloud Integration**: Azure services and deployment
- **Performance Optimization**: Caching, profiling, and scaling

---

**Happy Learning! 🚀** Each project is designed to be both educational and practical, providing you with real-world coding experience that directly applies to professional development.
| 4 | Advanced Features | 2 Projects | ✅ Complete |
| 5 | Web API Development | 1 Project | ✅ Complete |
| 6 | Entity Framework | Learning Guide | 📖 Ready |
| 7 | Security & Auth | Learning Guide | 📖 Ready |
| 8 | Final Project | Learning Guide | 📖 Ready |

## Key Learning Outcomes

By completing this course, you'll master:
- ✅ C# language fundamentals and modern syntax features
- ✅ Object-oriented programming with inheritance and polymorphism  
- ✅ Collections, generics, and LINQ for data manipulation
- ✅ Asynchronous programming with async/await patterns
- ✅ Lambda expressions and functional programming concepts
- ✅ RESTful API development with ASP.NET Core
- 📚 Entity Framework for database operations (Week 6)
- 📚 Authentication and security best practices (Week 7)
- 📚 Complete project development lifecycle (Week 8)

## How to Use This Repository

1. **Sequential Learning**: Progress through weeks 1-5 for hands-on coding experience
2. **Interactive Exploration**: Use the menu systems in console apps to explore concepts
3. **Code Study**: Examine implementation details and comprehensive comments
4. **Practical Application**: Modify and extend the sample projects
5. **Conceptual Learning**: Review Weeks 6-8 README files for advanced topics

## Next Steps

This repository provides a solid foundation with extensive sample code for core C# and .NET concepts. The interactive projects demonstrate real-world applications and industry best practices. Use this as a springboard to explore advanced .NET technologies like Entity Framework Core, Azure Functions, and enterprise application patterns.
