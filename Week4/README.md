# Week 4: Advanced C# Features

## Day 1: LINQ and Lambda Expressions

### Learning Objectives

- Understand LINQ (Language Integrated Query) fundamentals
- Learn about lambda expressions and their syntax
- Practice writing queries to manipulate data collections

### Resources

- [LINQ in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/)
- [Lambda Expressions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions)
- [LINQ Query Syntax](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/query-syntax-and-method-syntax-in-linq)

### Hands-on Tasks

1. LINQ Basics Exercise:
   - Create a console application that demonstrates:
     - LINQ query syntax vs. method syntax
     - Basic operations (where, select, orderby)
     - Filtering, sorting, and projecting data
     - Working with different data sources

2. Advanced LINQ Operations:
   - Create a program that uses:
     - Grouping and aggregation functions
     - Joining multiple data sources
     - Set operations (union, intersection)
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
