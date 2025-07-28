using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LambdaExpressionsDemo
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime DateAdded { get; set; }

        public Product(int id, string name, string category, decimal price, int stock, DateTime dateAdded)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
            DateAdded = dateAdded;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Category} (${Price:F2}, Stock: {Stock})";
        }
    }

    public class Calculator
    {
        // Delegates for lambda expressions
        public delegate int MathOperation(int a, int b);
        public delegate bool Predicate<T>(T input);
        public delegate TResult Converter<T, TResult>(T input);
    }

    class Program
    {
        static List<Product> products;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Lambda Expressions Demo ===\n");
            
            InitializeData();

            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("Choose a demonstration:");
                Console.WriteLine("1. Basic Lambda Syntax");
                Console.WriteLine("2. Lambda with LINQ");
                Console.WriteLine("3. Func and Action Delegates");
                Console.WriteLine("4. Expression Trees");
                Console.WriteLine("5. Lambda Captures (Closures)");
                Console.WriteLine("6. Complex Lambda Examples");
                Console.WriteLine("7. Performance Considerations");
                Console.WriteLine("8. Lambda vs Anonymous Methods");
                Console.WriteLine("9. Custom Delegates with Lambdas");
                Console.WriteLine("10. Real-World Lambda Usage");
                Console.WriteLine("11. Exit");
                Console.Write("\nEnter your choice (1-11): ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        BasicLambdaSyntaxDemo();
                        break;
                    case "2":
                        LambdaWithLinqDemo();
                        break;
                    case "3":
                        FuncAndActionDemo();
                        break;
                    case "4":
                        ExpressionTreesDemo();
                        break;
                    case "5":
                        LambdaCapturesDemo();
                        break;
                    case "6":
                        ComplexLambdaExamples();
                        break;
                    case "7":
                        PerformanceConsiderations();
                        break;
                    case "8":
                        LambdaVsAnonymousMethodsDemo();
                        break;
                    case "9":
                        CustomDelegatesDemo();
                        break;
                    case "10":
                        RealWorldUsageDemo();
                        break;
                    case "11":
                        continueProgram = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (continueProgram)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void InitializeData()
        {
            products = new List<Product>
            {
                new Product(1, "Laptop", "Electronics", 999.99m, 50, DateTime.Now.AddDays(-30)),
                new Product(2, "Mouse", "Electronics", 29.99m, 200, DateTime.Now.AddDays(-25)),
                new Product(3, "Keyboard", "Electronics", 79.99m, 150, DateTime.Now.AddDays(-20)),
                new Product(4, "Monitor", "Electronics", 299.99m, 75, DateTime.Now.AddDays(-15)),
                new Product(5, "Chair", "Furniture", 199.99m, 30, DateTime.Now.AddDays(-10)),
                new Product(6, "Desk", "Furniture", 399.99m, 20, DateTime.Now.AddDays(-5)),
                new Product(7, "Book", "Education", 24.99m, 100, DateTime.Now.AddDays(-12)),
                new Product(8, "Pen", "Office", 2.99m, 500, DateTime.Now.AddDays(-8)),
                new Product(9, "Notebook", "Office", 5.99m, 300, DateTime.Now.AddDays(-3)),
                new Product(10, "Coffee Mug", "Kitchen", 12.99m, 80, DateTime.Now.AddDays(-1))
            };
        }

        static void BasicLambdaSyntaxDemo()
        {
            Console.WriteLine("--- Basic Lambda Syntax Demo ---");

            // Simple lambda expressions
            Console.WriteLine("1. Simple Lambda Expressions:");
            
            // Single parameter, single expression
            Func<int, int> square = x => x * x;
            Console.WriteLine($"Square of 5: {square(5)}");

            // Multiple parameters
            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine($"Add 3 + 7: {add(3, 7)}");

            // No parameters
            Func<string> getGreeting = () => "Hello, World!";
            Console.WriteLine($"Greeting: {getGreeting()}");

            // Lambda with statement body
            Func<int, string> describe = x =>
            {
                if (x < 0) return "negative";
                if (x == 0) return "zero";
                return "positive";
            };
            Console.WriteLine($"Describe -5: {describe(-5)}");
            Console.WriteLine($"Describe 0: {describe(0)}");
            Console.WriteLine($"Describe 10: {describe(10)}");

            // Lambda with void return (Action)
            Action<string> printMessage = message => Console.WriteLine($"Message: {message}");
            printMessage("Hello from lambda!");

            Console.WriteLine("\n2. Type Inference:");
            // Compiler infers types
            var multiply = (int a, int b) => a * b;
            Console.WriteLine($"Multiply 4 * 6: {multiply(4, 6)}");

            // Explicit types when needed
            Func<object, string> toString = (object obj) => obj?.ToString() ?? "null";
            Console.WriteLine($"ToString of 42: {toString(42)}");
        }

        static void LambdaWithLinqDemo()
        {
            Console.WriteLine("--- Lambda with LINQ Demo ---");

            Console.WriteLine("Available products:");
            products.Take(5).ToList().ForEach(p => Console.WriteLine($"  {p}"));

            Console.WriteLine("\n1. Filtering with Where:");
            var expensiveProducts = products.Where(p => p.Price > 100).ToList();
            Console.WriteLine($"Products over $100 ({expensiveProducts.Count}):");
            expensiveProducts.ForEach(p => Console.WriteLine($"  {p}"));

            Console.WriteLine("\n2. Projection with Select:");
            var productNames = products.Select(p => p.Name.ToUpper()).ToList();
            Console.WriteLine("Product names (uppercase):");
            productNames.ForEach(name => Console.WriteLine($"  {name}"));

            // Complex selection
            var productSummary = products
                .Select(p => new { 
                    p.Name, 
                    p.Price, 
                    TotalValue = p.Price * p.Stock,
                    IsExpensive = p.Price > 100
                })
                .ToList();
            
            Console.WriteLine("\n3. Complex Selection:");
            productSummary.Take(3).ToList().ForEach(ps => 
                Console.WriteLine($"  {ps.Name}: ${ps.Price:F2} (Total: ${ps.TotalValue:F2}, Expensive: {ps.IsExpensive})"));

            Console.WriteLine("\n4. Sorting with OrderBy:");
            var sortedProducts = products
                .OrderByDescending(p => p.Price)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToList();
            
            Console.WriteLine("Top 5 most expensive products:");
            sortedProducts.ForEach(p => Console.WriteLine($"  {p}"));

            Console.WriteLine("\n5. Grouping with GroupBy:");
            var productsByCategory = products
                .GroupBy(p => p.Category)
                .Select(g => new { 
                    Category = g.Key, 
                    Count = g.Count(), 
                    AvgPrice = g.Average(p => p.Price) 
                })
                .ToList();
            
            Console.WriteLine("Products grouped by category:");
            productsByCategory.ForEach(g => 
                Console.WriteLine($"  {g.Category}: {g.Count} items, Avg: ${g.AvgPrice:F2}"));

            Console.WriteLine("\n6. Aggregations:");
            var totalValue = products.Sum(p => p.Price * p.Stock);
            var averagePrice = products.Average(p => p.Price);
            var maxPrice = products.Max(p => p.Price);
            var cheapestProduct = products.OrderBy(p => p.Price).First();

            Console.WriteLine($"Total inventory value: ${totalValue:F2}");
            Console.WriteLine($"Average price: ${averagePrice:F2}");
            Console.WriteLine($"Maximum price: ${maxPrice:F2}");
            Console.WriteLine($"Cheapest product: {cheapestProduct.Name} (${cheapestProduct.Price:F2})");
        }

        static void FuncAndActionDemo()
        {
            Console.WriteLine("--- Func and Action Delegates Demo ---");

            Console.WriteLine("1. Func<T, TResult> Examples:");
            
            // Func with one parameter
            Func<int, bool> isEven = n => n % 2 == 0;
            Console.WriteLine($"Is 4 even? {isEven(4)}");
            Console.WriteLine($"Is 7 even? {isEven(7)}");

            // Func with multiple parameters
            Func<string, string, string> concatenate = (s1, s2) => $"{s1} {s2}";
            Console.WriteLine($"Concatenate: {concatenate("Hello", "World")}");

            // Func with complex logic
            Func<Product, string> getProductDescription = p => 
                $"{p.Name} ({p.Category}) - ${p.Price:F2} " + 
                (p.Stock > 0 ? $"({p.Stock} in stock)" : "(Out of stock)");

            Console.WriteLine("\nProduct descriptions:");
            products.Take(3).ToList().ForEach(p => 
                Console.WriteLine($"  {getProductDescription(p)}"));

            Console.WriteLine("\n2. Action<T> Examples:");
            
            // Action with one parameter
            Action<string> logMessage = message => 
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            logMessage("Application started");

            // Action with multiple parameters
            Action<string, ConsoleColor> colorPrint = (text, color) =>
            {
                var originalColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ForegroundColor = originalColor;
            };

            colorPrint("This is red text", ConsoleColor.Red);
            colorPrint("This is green text", ConsoleColor.Green);

            // Action with no parameters
            Action printSeparator = () => Console.WriteLine(new string('-', 40));
            printSeparator();

            Console.WriteLine("\n3. Predicate<T> Examples:");
            
            // Predicate is essentially Func<T, bool>
            Predicate<Product> isElectronics = p => p.Category == "Electronics";
            Predicate<Product> isInStock = p => p.Stock > 0;
            Predicate<Product> isAffordable = p => p.Price <= 50;

            var electronicsProducts = products.Where(p => isElectronics(p)).ToList();
            var affordableProducts = products.Where(p => isAffordable(p)).ToList();

            Console.WriteLine($"Electronics products: {electronicsProducts.Count}");
            Console.WriteLine($"Affordable products (≤$50): {affordableProducts.Count}");

            // Combining predicates
            var affordableElectronics = products
                .Where(p => isElectronics(p) && isAffordable(p))
                .ToList();
            Console.WriteLine($"Affordable electronics: {affordableElectronics.Count}");
        }

        static void ExpressionTreesDemo()
        {
            Console.WriteLine("--- Expression Trees Demo ---");

            Console.WriteLine("1. Basic Expression Trees:");
            
            // Lambda expression as delegate
            Func<int, int> squareFunc = x => x * x;
            Console.WriteLine($"Square function result: {squareFunc(5)}");

            // Lambda expression as expression tree
            Expression<Func<int, int>> squareExpr = x => x * x;
            Console.WriteLine($"Expression tree: {squareExpr}");
            Console.WriteLine($"Expression body: {squareExpr.Body}");
            Console.WriteLine($"Expression parameters: {string.Join(", ", squareExpr.Parameters)}");

            // Compile and execute expression
            var compiledFunc = squareExpr.Compile();
            Console.WriteLine($"Compiled expression result: {compiledFunc(5)}");

            Console.WriteLine("\n2. Analyzing Expression Structure:");
            Expression<Func<Product, bool>> productFilter = p => p.Price > 100 && p.Category == "Electronics";
            
            Console.WriteLine($"Filter expression: {productFilter}");
            Console.WriteLine($"Expression type: {productFilter.Body.GetType().Name}");
            
            if (productFilter.Body is BinaryExpression binaryExpr)
            {
                Console.WriteLine($"Binary operator: {binaryExpr.NodeType}");
                Console.WriteLine($"Left side: {binaryExpr.Left}");
                Console.WriteLine($"Right side: {binaryExpr.Right}");
            }

            Console.WriteLine("\n3. Building Expression Trees Programmatically:");
            
            // Manually building: x => x * 2
            var parameter = Expression.Parameter(typeof(int), "x");
            var constant = Expression.Constant(2);
            var multiply = Expression.Multiply(parameter, constant);
            var lambda = Expression.Lambda<Func<int, int>>(multiply, parameter);
            
            Console.WriteLine($"Programmatically built expression: {lambda}");
            var compiledLambda = lambda.Compile();
            Console.WriteLine($"Result for x=6: {compiledLambda(6)}");

            Console.WriteLine("\n4. Expression Trees with LINQ:");
            
            // These create expression trees, not delegates
            var expensiveProducts = products.AsQueryable()
                .Where(p => p.Price > 50)
                .Select(p => new { p.Name, p.Price })
                .ToList();

            Console.WriteLine($"Expensive products (LINQ with expressions): {expensiveProducts.Count}");
            expensiveProducts.Take(3).ToList().ForEach(p => 
                Console.WriteLine($"  {p.Name}: ${p.Price:F2}"));
        }

        static void LambdaCapturesDemo()
        {
            Console.WriteLine("--- Lambda Captures (Closures) Demo ---");

            Console.WriteLine("1. Basic Variable Capture:");
            
            int multiplier = 3;
            Func<int, int> multiplyByCapture = x => x * multiplier;
            
            Console.WriteLine($"5 * {multiplier} = {multiplyByCapture(5)}");
            
            // Modifying captured variable
            multiplier = 5;
            Console.WriteLine($"5 * {multiplier} = {multiplyByCapture(5)} (captured variable changed)");

            Console.WriteLine("\n2. Capturing Loop Variables:");
            
            // Correct way to capture loop variables
            var functions = new List<Func<int>>();
            
            for (int i = 0; i < 5; i++)
            {
                int localCopy = i; // Capture local copy
                functions.Add(() => localCopy * 2);
            }

            Console.WriteLine("Loop variable captures (correct):");
            for (int j = 0; j < functions.Count; j++)
            {
                Console.WriteLine($"  Function {j}: {functions[j]()}");
            }

            Console.WriteLine("\n3. Capturing Reference Types:");
            
            var list = new List<string> { "Apple", "Banana", "Cherry" };
            Action addItem = () => list.Add("Date");
            Func<int> getCount = () => list.Count;
            
            Console.WriteLine($"Initial count: {getCount()}");
            addItem();
            Console.WriteLine($"After adding item: {getCount()}");
            Console.WriteLine($"Items: {string.Join(", ", list)}");

            Console.WriteLine("\n4. Capturing 'this' in Instance Methods:");
            var calculator = new LambdaCalculator();
            calculator.DemonstrateClosure();

            Console.WriteLine("\n5. Performance Impact of Captures:");
            
            // No captures - can be cached
            Func<int, int> noCaptureFunc = x => x * 2;
            
            // With captures - creates closure
            int factor = 3;
            Func<int, int> captureFunc = x => x * factor;
            
            Console.WriteLine("Lambdas without captures are more efficient (can be cached)");
            Console.WriteLine("Lambdas with captures create closures (heap allocation)");
        }

        static void ComplexLambdaExamples()
        {
            Console.WriteLine("--- Complex Lambda Examples ---");

            Console.WriteLine("1. Chained Lambda Operations:");
            
            var result = products
                .Where(p => p.Stock > 0)                    // Filter: in stock
                .GroupBy(p => p.Category)                   // Group by category
                .Select(g => new                            // Project to new type
                {
                    Category = g.Key,
                    Products = g.OrderByDescending(p => p.Price).Take(2).ToList(),
                    TotalValue = g.Sum(p => p.Price * p.Stock),
                    AveragePrice = g.Average(p => p.Price)
                })
                .OrderByDescending(x => x.TotalValue)       // Sort by total value
                .ToList();

            Console.WriteLine("Top 2 products per category by value:");
            foreach (var category in result)
            {
                Console.WriteLine($"\n{category.Category} (Total: ${category.TotalValue:F2}, Avg: ${category.AveragePrice:F2}):");
                category.Products.ForEach(p => Console.WriteLine($"  {p}"));
            }

            Console.WriteLine("\n2. Conditional Lambda Logic:");
            
            var conditionalProcessing = products
                .Select(p => new
                {
                    Product = p,
                    PriceCategory = p.Price switch
                    {
                        <= 10 => "Budget",
                        <= 50 => "Standard", 
                        <= 200 => "Premium",
                        _ => "Luxury"
                    },
                    StockStatus = p.Stock switch
                    {
                        0 => "Out of Stock",
                        < 50 => "Low Stock",
                        < 100 => "Medium Stock",
                        _ => "High Stock"
                    }
                })
                .Where(x => x.Product.Stock > 0)
                .GroupBy(x => x.PriceCategory)
                .ToList();

            Console.WriteLine("\nProducts by price category:");
            foreach (var group in conditionalProcessing)
            {
                Console.WriteLine($"\n{group.Key} ({group.Count()} items):");
                group.Take(3).ToList().ForEach(item => 
                    Console.WriteLine($"  {item.Product.Name} - {item.StockStatus}"));
            }

            Console.WriteLine("\n3. Advanced Aggregations:");
            
            var analytics = products.Aggregate(
                new { TotalProducts = 0, TotalValue = 0m, Categories = new HashSet<string>() },
                (acc, product) => new
                {
                    TotalProducts = acc.TotalProducts + 1,
                    TotalValue = acc.TotalValue + (product.Price * product.Stock),
                    Categories = new HashSet<string>(acc.Categories) { product.Category }
                },
                acc => new
                {
                    acc.TotalProducts,
                    acc.TotalValue,
                    CategoryCount = acc.Categories.Count,
                    AverageValuePerProduct = acc.TotalValue / acc.TotalProducts
                });

            Console.WriteLine($"\nAnalytics Summary:");
            Console.WriteLine($"Total Products: {analytics.TotalProducts}");
            Console.WriteLine($"Total Value: ${analytics.TotalValue:F2}");
            Console.WriteLine($"Categories: {analytics.CategoryCount}");
            Console.WriteLine($"Average Value per Product: ${analytics.AverageValuePerProduct:F2}");
        }

        static void PerformanceConsiderations()
        {
            Console.WriteLine("--- Performance Considerations ---");

            var random = new Random();
            var numbers = Enumerable.Range(1, 1000000).Select(_ => random.Next(1, 1000)).ToList();

            Console.WriteLine("1. Compiled vs Interpreted:");
            
            // Compiled delegate
            Func<int, bool> compiledPredicate = x => x % 2 == 0;
            
            // Expression tree (interpreted)
            Expression<Func<int, bool>> expressionPredicate = x => x % 2 == 0;
            var interpretedPredicate = expressionPredicate.Compile();

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var evenCountCompiled = numbers.Count(compiledPredicate);
            sw.Stop();
            Console.WriteLine($"Compiled delegate: {evenCountCompiled} evens in {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            var evenCountInterpreted = numbers.Count(interpretedPredicate);
            sw.Stop();
            Console.WriteLine($"Compiled expression: {evenCountInterpreted} evens in {sw.ElapsedMilliseconds}ms");

            Console.WriteLine("\n2. Lambda vs Traditional Methods:");
            
            // Lambda approach
            sw.Restart();
            var lambdaResult = numbers.Where(x => x > 500).Sum(x => x);
            sw.Stop();
            var lambdaTime = sw.ElapsedMilliseconds;

            // Traditional loop
            sw.Restart();
            long traditionalResult = 0;
            foreach (var number in numbers)
            {
                if (number > 500)
                    traditionalResult += number;
            }
            sw.Stop();
            var traditionalTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"Lambda approach: {lambdaResult} in {lambdaTime}ms");
            Console.WriteLine($"Traditional loop: {traditionalResult} in {traditionalTime}ms");

            Console.WriteLine("\n3. Memory Allocation with Captures:");
            
            // Without capture (can be cached)
            Func<int, int> noCaptureFunc = x => x * 2;
            
            // With capture (creates closure)
            int multiplier = 3;
            Func<int, int> captureFunc = x => x * multiplier;

            Console.WriteLine("Lambdas without captures are more memory efficient");
            Console.WriteLine("Lambdas with captures create closures (heap allocation)");
            
            Console.WriteLine("\nPerformance Tips:");
            Console.WriteLine("• Use compiled delegates over expression trees for repeated execution");
            Console.WriteLine("• Avoid unnecessary captures in lambdas");
            Console.WriteLine("• Consider traditional loops for simple operations on large datasets");
            Console.WriteLine("• Cache compiled expressions when reusing");
        }

        static void LambdaVsAnonymousMethodsDemo()
        {
            Console.WriteLine("--- Lambda vs Anonymous Methods Demo ---");

            Console.WriteLine("1. Anonymous Methods (C# 2.0):");
            
            // Anonymous method syntax
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            var evenNumbersAnonymous = numbers.FindAll(delegate(int x) { return x % 2 == 0; });
            Console.WriteLine($"Even numbers (anonymous method): [{string.Join(", ", evenNumbersAnonymous)}]");

            Console.WriteLine("\n2. Lambda Expressions (C# 3.0+):");
            
            // Lambda expression syntax
            var evenNumbersLambda = numbers.Where(x => x % 2 == 0).ToList();
            Console.WriteLine($"Even numbers (lambda): [{string.Join(", ", evenNumbersLambda)}]");

            Console.WriteLine("\n3. Syntax Comparison:");
            
            // Anonymous method
            Action<string> anonymousAction = delegate(string message)
            {
                Console.WriteLine($"Anonymous: {message}");
            };

            // Lambda expression
            Action<string> lambdaAction = message => Console.WriteLine($"Lambda: {message}");

            anonymousAction("Hello from anonymous method");
            lambdaAction("Hello from lambda expression");

            Console.WriteLine("\n4. Expression Trees (Lambda Only):");
            
            // This works with lambdas
            Expression<Func<int, bool>> lambdaExpression = x => x > 5;
            Console.WriteLine($"Lambda as expression tree: {lambdaExpression}");

            // This would NOT work with anonymous methods
            // Expression<Func<int, bool>> anonymousExpression = delegate(int x) { return x > 5; }; // Error!

            Console.WriteLine("\nKey Differences:");
            Console.WriteLine("• Lambdas are more concise and readable");
            Console.WriteLine("• Lambdas can be converted to expression trees");
            Console.WriteLine("• Anonymous methods provide more explicit control");
            Console.WriteLine("• Both compile to similar IL code");
        }

        static void CustomDelegatesDemo()
        {
            Console.WriteLine("--- Custom Delegates with Lambdas Demo ---");

            Console.WriteLine("1. Basic Custom Delegates:");
            
            // Using the Calculator delegates
            Calculator.MathOperation add = (a, b) => a + b;
            Calculator.MathOperation multiply = (a, b) => a * b;
            Calculator.MathOperation power = (a, b) => (int)Math.Pow(a, b);

            Console.WriteLine($"Add 5 + 3 = {add(5, 3)}");
            Console.WriteLine($"Multiply 5 * 3 = {multiply(5, 3)}");
            Console.WriteLine($"Power 5^3 = {power(5, 3)}");

            Console.WriteLine("\n2. Generic Custom Delegates:");
            
            Calculator.Predicate<int> isPositive = x => x > 0;
            Calculator.Predicate<string> isNotEmpty = s => !string.IsNullOrEmpty(s);
            Calculator.Converter<int, string> intToString = x => $"Number: {x}";

            Console.WriteLine($"Is 5 positive? {isPositive(5)}");
            Console.WriteLine($"Is -2 positive? {isPositive(-2)}");
            Console.WriteLine($"Is 'hello' not empty? {isNotEmpty("hello")}");
            Console.WriteLine($"Convert 42: {intToString(42)}");

            Console.WriteLine("\n3. Multicast Delegates:");
            
            Action<string> logger = message => Console.WriteLine($"[LOG] {message}");
            logger += message => Console.WriteLine($"[DEBUG] {message}");
            logger += message => Console.WriteLine($"[TRACE] {message}");

            Console.WriteLine("Multicast delegate execution:");
            logger("Application started");

            Console.WriteLine("\n4. Delegate as Parameters:");
            
            ProcessList(numbers => numbers.Where(n => n % 2 == 0), "even numbers");
            ProcessList(numbers => numbers.Where(n => n > 50), "numbers > 50");
            ProcessList(numbers => numbers.OrderByDescending(n => n).Take(3), "top 3 numbers");
        }

        static void ProcessList(Func<IEnumerable<int>, IEnumerable<int>> processor, string description)
        {
            var numbers = new List<int> { 15, 22, 8, 91, 34, 5, 67, 12, 78, 3 };
            var result = processor(numbers).ToList();
            Console.WriteLine($"Processing {description}: [{string.Join(", ", result)}]");
        }

        static void RealWorldUsageDemo()
        {
            Console.WriteLine("--- Real-World Lambda Usage Demo ---");

            Console.WriteLine("1. Event Handling:");
            
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (sender, e) => Console.WriteLine($"Timer tick at {DateTime.Now:HH:mm:ss}");
            
            Console.WriteLine("Timer started (will tick 3 times)...");
            timer.Start();
            System.Threading.Thread.Sleep(3500);
            timer.Stop();
            timer.Dispose();

            Console.WriteLine("\n2. Configuration and Options:");
            
            var processors = new List<Func<Product, Product>>
            {
                p => new Product(p.Id, p.Name.ToUpper(), p.Category, p.Price, p.Stock, p.DateAdded),
                p => new Product(p.Id, p.Name, p.Category, p.Price * 1.1m, p.Stock, p.DateAdded), // 10% price increase
                p => new Product(p.Id, p.Name, p.Category, p.Price, Math.Max(0, p.Stock - 1), p.DateAdded) // Reduce stock
            };

            var sampleProduct = products.First();
            Console.WriteLine($"Original: {sampleProduct}");
            
            var processedProduct = processors.Aggregate(sampleProduct, (current, processor) => processor(current));
            Console.WriteLine($"Processed: {processedProduct}");

            Console.WriteLine("\n3. Validation Chains:");
            
            var validators = new List<Func<Product, (bool IsValid, string Message)>>
            {
                p => (p.Price > 0, "Price must be positive"),
                p => (p.Stock >= 0, "Stock cannot be negative"), 
                p => (!string.IsNullOrEmpty(p.Name), "Name is required"),
                p => (p.Name.Length <= 100, "Name too long")
            };

            var testProduct = new Product(99, "Test Product", "Test", 25.99m, 10, DateTime.Now);
            var validationResults = validators.Select(validator => validator(testProduct)).ToList();
            
            Console.WriteLine($"Validating: {testProduct.Name}");
            var allValid = validationResults.All(r => r.IsValid);
            Console.WriteLine($"All validations passed: {allValid}");
            
            if (!allValid)
            {
                var failures = validationResults.Where(r => !r.IsValid).Select(r => r.Message);
                Console.WriteLine($"Validation failures: {string.Join(", ", failures)}");
            }

            Console.WriteLine("\n4. Data Transformation Pipeline:");
            
            var pipeline = products
                .Where(p => p.Stock > 0)                           // Filter: in stock
                .Select(p => new { p.Name, p.Price, p.Category })  // Project: minimal data
                .GroupBy(p => p.Category)                          // Group: by category
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count(),
                    MinPrice = g.Min(p => p.Price),
                    MaxPrice = g.Max(p => p.Price),
                    Products = g.Select(p => p.Name).ToList()
                })
                .OrderBy(g => g.Category)                          // Sort: by category
                .ToList();

            Console.WriteLine("Data transformation pipeline results:");
            pipeline.ForEach(category =>
            {
                Console.WriteLine($"{category.Category}: {category.Count} items, " +
                    $"${category.MinPrice:F2}-${category.MaxPrice:F2}");
                Console.WriteLine($"  Products: {string.Join(", ", category.Products)}");
            });
        }
    }

    // Helper class for lambda captures demo
    public class LambdaCalculator
    {
        private int baseValue = 10;

        public void DemonstrateClosure()
        {
            // Lambda captures 'this' and can access instance members
            Func<int, int> addToBase = x => x + baseValue;
            Action incrementBase = () => baseValue++;

            Console.WriteLine($"Initial base value: {baseValue}");
            Console.WriteLine($"Add 5 to base: {addToBase(5)}");
            
            incrementBase();
            Console.WriteLine($"After increment, base value: {baseValue}");
            Console.WriteLine($"Add 5 to new base: {addToBase(5)}");
        }
    }
}
