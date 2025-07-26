# Week 3: Working with Data

## Day 1: Collections and Generics

### Learning Objectives

- Understand different collection types in C#
- Learn how to use generics for type-safe programming
- Practice manipulating data in collections

### Resources

- [Collections in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/collections)
- [Generics in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/)
- [List<T> Class](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)
- [Dictionary<TKey,TValue> Class](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2)

### Hands-on Tasks

1. Collection Types Exercise:
   - Create a console application that demonstrates:
     - Arrays and their limitations
     - Lists and their advantages
     - Dictionaries for key-value pair storage
     - Sets for unique collections

2. Generic Methods and Classes:
   - Create your own generic class (e.g., a simple Stack<T> or Queue<T>)
   - Implement generic methods that work with different data types
   - Demonstrate the benefits of generics over using object type

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
