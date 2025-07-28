# Asynchronous Programming Demo

This project provides a comprehensive demonstration of asynchronous programming concepts in C#.

## Concepts Covered

### Core Async/Await
- **async/await keywords**: Basic asynchronous method definition and usage
- **Task and Task\<T\>**: Understanding the fundamental async types
- **Synchronous vs Asynchronous**: Performance and threading differences

### Advanced Async Patterns
- **Task.WhenAll**: Running multiple async operations in parallel
- **Task.WhenAny**: Completing when the first operation finishes
- **Task.Run**: Using thread pool for CPU-bound operations
- **ConfigureAwait**: Controlling synchronization context capture

### Error Handling and Cancellation
- **Exception Handling**: Try-catch blocks with async operations
- **CancellationToken**: Cooperative cancellation of async operations
- **Timeout Handling**: Setting time limits on async operations

### Real-World Scenarios
- **File I/O**: Asynchronous file reading and writing
- **HTTP Requests**: Web API calls with HttpClient
- **Producer-Consumer**: Async coordination patterns
- **Performance Comparisons**: Measuring async vs sync performance

## Key Learning Points

1. **When to Use Async**:
   - I/O-bound operations (file access, network calls, database queries)
   - Operations that naturally involve waiting

2. **When NOT to Use Async**:
   - CPU-bound operations (use Task.Run instead)
   - Simple, fast operations with no I/O

3. **Best Practices**:
   - Use ConfigureAwait(false) in library code
   - Always handle cancellation tokens
   - Avoid async void (except for event handlers)
   - Don't block on async code (avoid .Result or .Wait())

## Threading Concepts

- **Thread Pool**: How async operations use the thread pool
- **Synchronization Context**: How async methods return to the calling context
- **Thread Safety**: Considerations when working with shared state

## How to Run

1. Navigate to the project directory
2. Run `dotnet run` to execute the program
3. Choose from the interactive menu to explore different async concepts
4. Each demo shows practical implementations with timing measurements

## Performance Benefits

The demos show how asynchronous programming can:
- Improve application responsiveness
- Enable better resource utilization
- Allow concurrent operations
- Prevent thread blocking on I/O operations
