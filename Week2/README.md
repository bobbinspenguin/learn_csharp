# Week 2: Object-Oriented Programming in C# ✅ Complete

This week contains **4 fully implemented sample projects** that demonstrate comprehensive object-oriented programming concepts in C#.

## Sample Projects Overview

### 1. ClassBasics - Foundation OOP Concepts
**Location**: `Week2/ClassBasics/`
**Key Features**:
- Basic class creation and object instantiation
- Properties with getters and setters
- Constructor overloading
- Method implementation
- Encapsulation principles

**Run**: `cd Week2/ClassBasics && dotnet run`

### 2. InheritanceDemo - Hierarchy and Polymorphism
**Location**: `Week2/InheritanceDemo/`
**Key Features**:
- Person/Student/Teacher class hierarchy
- Method overriding and virtual methods
- Polymorphism demonstrations
- Base class constructors
- Interactive examples

**Run**: `cd Week2/InheritanceDemo && dotnet run`

### 3. InterfacesAndAbstracts - Advanced OOP Patterns
**Location**: `Week2/InterfacesAndAbstracts/`
**Key Features**:
- Interface implementations (IShape, IDrawable)
- Abstract classes and methods (Animal hierarchy)
- Multiple inheritance through interfaces
- Contract-based programming
- Real-world design patterns

**Run**: `cd Week2/InterfacesAndAbstracts && dotnet run`

### 4. ExceptionHandlingDemo - Banking System with Error Management
**Location**: `Week2/ExceptionHandlingDemo/`
**Key Features**:
- Complete banking system simulation
- Custom exception classes
- Try-catch-finally blocks
- Exception propagation
- Logging and error reporting
- Input validation and business rules

**Run**: `cd Week2/ExceptionHandlingDemo && dotnet run`

## Learning Objectives Covered

✅ **Classes and Objects**: Understanding the fundamentals of OOP  
✅ **Encapsulation**: Data hiding and property implementation  
✅ **Inheritance**: Class hierarchies and code reuse  
✅ **Polymorphism**: Method overriding and virtual methods  
✅ **Interfaces**: Contract-based programming  
✅ **Abstract Classes**: Template patterns for derived classes  
✅ **Exception Handling**: Robust error management and recovery  
✅ **Real-World Applications**: Banking system and shape calculations
   - Create derived classes like `Student` and `Teacher` that inherit from Person
   - Add unique properties and methods to each derived class
   - Override methods to demonstrate polymorphism

2. Create a console application to demonstrate:
   - Object creation and initialization
   - Method calls and property access
   - Use of constructors and access modifiers
   - Method overriding and virtual methods

## Day 2: Advanced OOP Concepts

### Learning Objectives

- Understand and implement interfaces in C#
- Learn about abstract classes and when to use them
- Implement proper exception handling in C#

### Resources

- [Interfaces in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/)
- [Abstract Classes in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)
- [Exception Handling in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/)

### Hands-on Tasks

1. Interface Implementation Exercise:
   - Create an `IShape` interface with methods like CalculateArea() and CalculatePerimeter()
   - Implement this interface in classes like Circle, Rectangle, and Triangle
   - Create a program that works with these shapes using the interface reference

2. Abstract Class Exercise:
   - Create an abstract `Animal` class with some concrete methods and abstract methods
   - Implement derived classes for different animals that implement the abstract methods
   - Demonstrate how abstract classes differ from interfaces

3. Exception Handling Project:
   - Create a simple banking application that:
     - Handles invalid input with custom exceptions
     - Uses try-catch-finally blocks appropriately
     - Implements different exception types for various error scenarios

### Code Samples

You'll find the following examples in the subdirectories:
- `ClassBasics/`: Simple class creation and usage
- `InheritanceDemo/`: Example of inheritance and polymorphism
- `InterfacesAndAbstracts/`: Examples of interfaces and abstract classes
- `ExceptionHandlingDemo/`: Exception handling techniques

### Reflection Questions

1. When would you choose an interface over an abstract class, and vice versa?
2. How does encapsulation improve code maintainability?
3. What challenges did you face when implementing polymorphism?
