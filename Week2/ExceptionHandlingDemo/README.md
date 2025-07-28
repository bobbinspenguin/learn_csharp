# Exception Handling Demo

This project demonstrates comprehensive exception handling techniques in C# through a simple banking application.

## Concepts Covered

- **Custom Exceptions**: Creating domain-specific exception classes
- **Try-Catch-Finally**: Proper exception handling structure
- **Multiple Catch Blocks**: Handling different exception types
- **Exception Properties**: Accessing exception details
- **Resource Cleanup**: Using finally blocks for cleanup

## Exception Types Demonstrated

1. **Custom Exceptions**:
   - `InsufficientFundsException`: Banking-specific error with additional properties
   - `InvalidAccountException`: Account validation errors
   - `NegativeAmountException`: Business rule violations

2. **Built-in Exceptions**:
   - `ArgumentException`: Invalid arguments
   - `FormatException`: Parsing errors
   - `FileNotFoundException`: File operations
   - `DivideByZeroException`: Math errors

## Best Practices Shown

- Creating specific custom exceptions for different error scenarios
- Using meaningful exception messages
- Proper exception hierarchy (inheriting from appropriate base classes)
- Resource cleanup in finally blocks
- Catching specific exceptions before general ones

## How to Run

1. Navigate to the project directory
2. Run `dotnet run` to execute the program
3. Try different menu options to see various exception scenarios
4. Use option 6 to see specific exception handling demonstrations
