# Lambda Expressions Demo

This project provides a comprehensive demonstration of lambda expressions and functional programming concepts in C#.

## Concepts Covered

### Basic Lambda Syntax
- **Simple Expressions**: Single line lambda expressions
- **Statement Bodies**: Multi-line lambda expressions with curly braces
- **Type Inference**: How the compiler infers lambda parameter types
- **Parameter Variations**: No parameters, single parameter, multiple parameters

### Delegates and Functional Types
- **Func\<T, TResult\>**: Delegates that return values
- **Action\<T\>**: Delegates that perform actions (void return)
- **Predicate\<T\>**: Boolean-returning delegates
- **Custom Delegates**: Creating and using custom delegate types

### Advanced Features
- **Expression Trees**: Converting lambdas to analyzable expression structures
- **Closures**: How lambdas capture variables from their enclosing scope
- **Method vs Query Syntax**: Different ways to write the same lambda logic
- **Performance Considerations**: When lambdas are efficient vs when they're not

### Real-World Applications
- **LINQ Integration**: Using lambdas with LINQ queries
- **Event Handling**: Lambda expressions in event-driven programming
- **Data Processing Pipelines**: Chaining operations with lambdas
- **Validation and Configuration**: Functional approaches to common tasks

## Key Learning Points

1. **When to Use Lambdas**:
   - LINQ queries and data transformations
   - Event handling and callbacks
   - Short, inline functions
   - Functional programming patterns

2. **Lambda vs Anonymous Methods**:
   - Lambdas are more concise and readable
   - Lambdas can be converted to expression trees
   - Both compile to similar IL code

3. **Performance Considerations**:
   - Lambdas without captures are cached and efficient
   - Lambdas with captures create closures (heap allocation)
   - Expression trees are slower than compiled delegates

4. **Best Practices**:
   - Keep lambdas short and focused
   - Avoid complex logic in lambda expressions
   - Be mindful of variable captures
   - Use meaningful parameter names

## Expression Trees

Expression trees allow lambdas to be analyzed and manipulated at runtime:
- Convert lambdas to data structures
- Analyze the structure of expressions
- Build expressions programmatically
- Used extensively by LINQ providers (Entity Framework, etc.)

## Closures and Variable Capture

Lambdas can "capture" variables from their enclosing scope:
- **Value Capture**: Local variables are captured by value
- **Reference Capture**: Reference types and ref parameters
- **Loop Variable Pitfalls**: Common mistakes with loop variables
- **Performance Impact**: Memory allocation for closures

## How to Run

1. Navigate to the project directory
2. Run `dotnet run` to execute the program
3. Choose from the interactive menu to explore different lambda concepts
4. Each demo shows practical examples with detailed explanations

## Functional Programming Concepts

- **Higher-order functions**: Functions that take or return other functions
- **Immutability**: Working with immutable data transformations
- **Composition**: Combining simple functions to create complex behavior
- **Pipeline Operations**: Chaining operations for data processing
