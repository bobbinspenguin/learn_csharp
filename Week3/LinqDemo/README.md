# LINQ Demo

This project provides a comprehensive demonstration of Language Integrated Query (LINQ) in C#.

## Concepts Covered

### Basic LINQ Operations
- **Filtering**: `Where` clause for data filtering
- **Projection**: `Select` for data transformation
- **Sorting**: `OrderBy`, `OrderByDescending`, `ThenBy`
- **Grouping**: `GroupBy` for data aggregation

### Advanced LINQ Features
- **Aggregation**: `Count`, `Sum`, `Average`, `Min`, `Max`
- **Set Operations**: `Distinct`, `Union`, `Intersect`, `Except`
- **Quantifiers**: `Any`, `All`, `Contains`
- **Joins**: Inner joins and group joins

### Query Syntax vs Method Syntax
- **Query Syntax**: SQL-like syntax using `from`, `where`, `select`
- **Method Syntax**: Fluent API using method chaining
- Both approaches compile to identical IL code

## Data Model

The demo uses three related entities:
- **Employee**: Id, Name, Department, Salary, Age, HireDate
- **Product**: Id, Name, Category, Price, Stock
- **Order**: OrderId, EmployeeId, ProductId, Quantity, OrderDate

## Key Learning Points

1. **Deferred Execution**: LINQ queries are not executed until enumerated
2. **Strongly Typed**: Full IntelliSense support and compile-time checking
3. **Composable**: Queries can be built incrementally
4. **Readable**: Code closely matches the intent
5. **Efficient**: Optimized execution plans

## How to Run

1. Navigate to the project directory
2. Run `dotnet run` to execute the program
3. Choose from the interactive menu to explore different LINQ concepts
4. Each demo shows both simple and complex real-world scenarios

## Best Practices Demonstrated

- Using meaningful variable names in queries
- Combining multiple LINQ operations effectively
- Choosing between query and method syntax appropriately
- Leveraging anonymous types for projections
- Using joins to combine related data
