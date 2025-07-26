# Week 2: Object-Oriented Programming in C#

## Day 1: Classes and Objects

### Learning Objectives

- Understand the fundamentals of Object-Oriented Programming
- Learn how to create and use classes and objects in C#
- Implement encapsulation, inheritance, and polymorphism

### Resources

- [Classes and Objects in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/)
- [Object-Oriented Programming in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/object-oriented-programming)
- [Inheritance in C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/inheritance)

### Hands-on Tasks

1. Create a simple class hierarchy:
   - Design a `Person` base class with properties like Name, Age, and methods like Introduce()
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
