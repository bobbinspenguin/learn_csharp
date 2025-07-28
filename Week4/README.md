# Week 4: Advanced C# Features ✅ Complete

This week contains **2 advanced sample projects** that demonstrate modern C# programming concepts and patterns.

## Sample Projects Overview

### 1. AsyncProgrammingDemo - Modern Asynchronous Programming
**Location**: `Week4/AsyncProgrammingDemo/`

**Key Features**:
- **Async/Await Patterns**: Complete implementation of asynchronous operations
- **HTTP Client Operations**: Web scraping and API calls with async patterns
- **File I/O Operations**: Asynchronous file reading and writing
- **Producer-Consumer Pattern**: Queue-based async processing
- **Cancellation Tokens**: Graceful cancellation of long-running operations
- **Task Parallel Library**: Concurrent execution and coordination
- **Error Handling**: Exception management in async contexts
- **Performance Analysis**: Timing comparisons between sync and async operations
- **Interactive Examples**: Menu-driven exploration of async concepts

**Run**: `cd Week4/AsyncProgrammingDemo && dotnet run`

### 2. LambdaExpressionsDemo - Functional Programming in C#
**Location**: `Week4/LambdaExpressionsDemo/`

**Key Features**:
- **Lambda Expression Syntax**: From simple to complex lambda expressions
- **Delegates and Func/Action**: Understanding functional programming concepts
- **Closures**: Variable capture and scope in lambda expressions
- **Expression Trees**: Compile-time code generation and analysis
- **LINQ Integration**: Advanced LINQ operations with lambda expressions
- **Performance Comparison**: Lambda vs traditional methods performance analysis
- **Practical Applications**: Real-world scenarios using functional programming
- **Custom Delegates**: Creating and using custom delegate types
- **Interactive Demonstrations**: Hands-on examples with timing analysis

**Run**: `cd Week4/LambdaExpressionsDemo && dotnet run`

## Learning Objectives Covered

✅ **Asynchronous Programming**: Understanding async/await, Task, and parallel processing  
✅ **Lambda Expressions**: Functional programming concepts and syntax  
✅ **Delegates**: Function pointers and callback mechanisms  
✅ **Closures**: Variable capture and lexical scoping  
✅ **Expression Trees**: Compile-time code generation and analysis  
✅ **Cancellation Tokens**: Graceful cancellation of operations  
✅ **Producer-Consumer Patterns**: Queue-based asynchronous processing  
✅ **Performance Optimization**: Async vs sync performance characteristics  
✅ **Error Handling**: Exception management in async and functional contexts
     - Custom sorting and filtering with lambda expressions

3. Practical LINQ Project:
   - Create an application that:
     - Reads data from a file or creates sample data
     - Performs complex queries using LINQ
     - Formats and displays the results in a meaningful way
     - Demonstrates how LINQ improves code readability

## Day 2: Asynchronous Programming

### Learning Objectives

- Understand the principles of asynchronous programming
- Learn about the Task-based Asynchronous Pattern (TAP)
- Practice using async/await for asynchronous operations

### Resources

- [Asynchronous Programming in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/)
- [Async and Await in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/async-and-await)
- [Task-based Asynchronous Pattern](https://docs.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)

### Hands-on Tasks

1. Asynchronous Basics:
   - Create a console application that demonstrates:
     - Difference between synchronous and asynchronous code
     - Using Task and Task<T> objects
     - Basic async/await pattern
     - Handling exceptions in asynchronous code

2. Parallel Programming:
   - Create a program that:
     - Uses Parallel.ForEach for parallel operations
     - Compares performance between sequential and parallel execution
     - Handles concurrency issues properly

3. Asynchronous Web API Client:
   - Create an application that:
     - Makes asynchronous HTTP requests to a public API
     - Processes the data without blocking the main thread
     - Implements proper error handling for network operations
     - Shows progress updates during long-running operations

### Code Samples

You'll find the following examples in the subdirectories:
- `LinqBasics/`: Fundamental LINQ operations
- `AdvancedLinq/`: Complex LINQ queries and operations
- `AsyncBasics/`: Introduction to async/await
- `ParallelDemo/`: Parallel programming examples
- `AsyncApiClient/`: Asynchronous web API client

### Reflection Questions

1. How does LINQ improve code readability and maintainability compared to traditional loops?
2. What are the advantages of asynchronous programming in different types of applications?
3. What challenges did you face when implementing parallel operations?
