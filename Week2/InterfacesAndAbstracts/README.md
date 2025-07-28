# Interfaces and Abstract Classes Demo

This project demonstrates the differences and uses of interfaces and abstract classes in C#.

## Concepts Covered

### Interfaces
- **IShape**: Defines contract for geometric shapes
- **IDrawable**: Demonstrates multiple interface implementation
- All methods in interfaces must be implemented by concrete classes

### Abstract Classes
- **Animal**: Provides both concrete methods (Sleep) and abstract methods (MakeSound, Move)
- Derived classes must implement all abstract methods
- Can contain both implementation and contracts

## Key Differences

| Feature | Interface | Abstract Class |
|---------|-----------|----------------|
| Multiple inheritance | ✅ Yes | ❌ No |
| Implementation | ❌ Contract only | ✅ Partial implementation |
| Access modifiers | ❌ Public only | ✅ All modifiers |
| Constructors | ❌ No | ✅ Yes |
| Fields | ❌ No | ✅ Yes |

## How to Run

1. Navigate to the project directory
2. Run `dotnet run` to execute the program
3. Observe how interfaces and abstract classes serve different purposes
