# Week 3: Collections and Data Structures in C# ✅ Complete

This week contains **2 comprehensive sample projects** that demonstrate advanced data manipulation and LINQ operations in C#.

## Sample Projects Overview

### 1. CollectionsDemo - Complete Collection Types Showcase
**Location**: `Week3/CollectionsDemo/`

**Key Features**:
- **Arrays**: Fixed-size collections with performance analysis
- **Lists**: Dynamic collections with CRUD operations
- **Dictionaries**: Key-value pair storage and lookups
- **HashSets**: Unique value collections and set operations
- **Queues**: First-In-First-Out (FIFO) data structures
- **Stacks**: Last-In-First-Out (LIFO) data structures
- **Custom Generics**: Implementation of generic Stack<T> and Queue<T>
- **Performance Comparisons**: Timing analysis of different operations
- **Interactive Menu**: Menu-driven exploration of all collection types

**Run**: `cd Week3/CollectionsDemo && dotnet run`

### 2. LinqDemo - Comprehensive LINQ Operations
**Location**: `Week3/LinqDemo/`

**Key Features**:
- **Real-World Data**: Students, products, and orders datasets
- **Filtering Operations**: Where clauses with complex conditions
- **Projection**: Select transformations and anonymous types
- **Grouping**: Group by operations with aggregations
- **Joins**: Inner joins between related datasets
- **Aggregations**: Sum, average, count, min, max operations
- **Sorting**: OrderBy and ThenBy with multiple criteria
- **Method Syntax vs Query Syntax**: Comparative examples
- **Performance Analysis**: Execution time measurements
- **Interactive Exploration**: Menu-driven LINQ demonstrations

**Run**: `cd Week3/LinqDemo && dotnet run`

## Learning Objectives Covered

✅ **Collection Types**: Understanding Arrays, Lists, Dictionaries, Sets, Queues, Stacks  
✅ **Generics**: Type-safe programming with generic classes and methods  
✅ **LINQ Fundamentals**: Language-Integrated Query for data manipulation  
✅ **Data Filtering**: Complex where clauses and conditional logic  
✅ **Data Projection**: Transforming data with Select operations  
✅ **Data Grouping**: Organizing data with GroupBy operations  
✅ **Data Joining**: Combining datasets with Join operations  
✅ **Performance Analysis**: Understanding collection performance characteristics  
✅ **Real-World Applications**: Student management and e-commerce scenarios

3. Collection Manipulation:
   - Create a program that:
     - Reads data from the user (e.g., names and scores)
     - Stores them in appropriate collections
     - Performs operations like sorting, filtering, and searching
     - Outputs the manipulated data

## Day 2: File I/O and Serialization

### Learning Objectives

- Learn different ways to read from and write to files
- Understand serialization and deserialization concepts
- Practice working with common file formats (JSON, XML)

### Resources

- [File and Stream I/O](https://docs.microsoft.com/en-us/dotnet/standard/io/)
- [JSON Serialization in .NET](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to)
- [XML Serialization in .NET](https://docs.microsoft.com/en-us/dotnet/standard/serialization/xml-serialization)

### Hands-on Tasks

1. File Operations Exercise:
   - Create a console application that:
     - Creates text files
     - Reads from text files
     - Appends to existing files
     - Handles file paths and directories

2. JSON Serialization Project:
   - Create a program that:
     - Defines a class structure (e.g., a Book class with properties)
     - Creates objects and serializes them to JSON
     - Reads the JSON back and deserializes it
     - Modifies the objects and saves them again

3. Data Processing Application:
   - Create an application that:
     - Reads data from a CSV or text file
     - Processes and transforms the data
     - Outputs the results to a different file format (JSON or XML)
     - Implements proper error handling for file operations

### Code Samples

You'll find the following examples in the subdirectories:
- `CollectionsDemo/`: Examples of various collection types
- `GenericsExample/`: Custom generic classes and methods
- `FileIOBasics/`: Basic file reading and writing
- `SerializationDemo/`: JSON and XML serialization examples

### Reflection Questions

1. What are the advantages of using generic collections over non-generic ones?
2. When would you choose serialization over simple file I/O for data storage?
3. What challenges did you encounter when working with file paths and directories?
