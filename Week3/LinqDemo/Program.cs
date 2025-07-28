using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqDemo
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }
        public DateTime HireDate { get; set; }

        public Employee(int id, string name, string department, decimal salary, int age, DateTime hireDate)
        {
            Id = id;
            Name = name;
            Department = department;
            Salary = salary;
            Age = age;
            HireDate = hireDate;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Department} (${Salary:N0}, Age: {Age}, Hired: {HireDate:yyyy-MM-dd})";
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(int id, string name, string category, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Stock = stock;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Category} (${Price:F2}, Stock: {Stock})";
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }

        public Order(int orderId, int employeeId, int productId, int quantity, DateTime orderDate)
        {
            OrderId = orderId;
            EmployeeId = employeeId;
            ProductId = productId;
            Quantity = quantity;
            OrderDate = orderDate;
        }
    }

    class Program
    {
        static List<Employee> employees;
        static List<Product> products;
        static List<Order> orders;

        static void Main(string[] args)
        {
            Console.WriteLine("=== LINQ Demo ===\n");
            
            InitializeData();

            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("Choose a LINQ demonstration:");
                Console.WriteLine("1. Basic Filtering (Where)");
                Console.WriteLine("2. Projection (Select)");
                Console.WriteLine("3. Sorting (OrderBy, ThenBy)");
                Console.WriteLine("4. Grouping (GroupBy)");
                Console.WriteLine("5. Aggregation (Count, Sum, Average, etc.)");
                Console.WriteLine("6. Set Operations (Distinct, Union, Intersect)");
                Console.WriteLine("7. Quantifiers (Any, All, Contains)");
                Console.WriteLine("8. Joins (Inner Join, Group Join)");
                Console.WriteLine("9. Advanced Queries");
                Console.WriteLine("10. Method Syntax vs Query Syntax");
                Console.WriteLine("11. Exit");
                Console.Write("\nEnter your choice (1-11): ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        BasicFilteringDemo();
                        break;
                    case "2":
                        ProjectionDemo();
                        break;
                    case "3":
                        SortingDemo();
                        break;
                    case "4":
                        GroupingDemo();
                        break;
                    case "5":
                        AggregationDemo();
                        break;
                    case "6":
                        SetOperationsDemo();
                        break;
                    case "7":
                        QuantifiersDemo();
                        break;
                    case "8":
                        JoinsDemo();
                        break;
                    case "9":
                        AdvancedQueriesDemo();
                        break;
                    case "10":
                        SyntaxComparisonDemo();
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
            employees = new List<Employee>
            {
                new Employee(1, "Alice Johnson", "IT", 75000, 28, new DateTime(2020, 3, 15)),
                new Employee(2, "Bob Smith", "HR", 55000, 35, new DateTime(2018, 7, 22)),
                new Employee(3, "Charlie Brown", "IT", 82000, 32, new DateTime(2019, 1, 10)),
                new Employee(4, "Diana Prince", "Finance", 68000, 29, new DateTime(2021, 5, 8)),
                new Employee(5, "Eve Adams", "IT", 72000, 26, new DateTime(2022, 2, 14)),
                new Employee(6, "Frank Wilson", "HR", 58000, 40, new DateTime(2017, 11, 30)),
                new Employee(7, "Grace Lee", "Finance", 71000, 31, new DateTime(2020, 9, 5)),
                new Employee(8, "Henry Davis", "IT", 79000, 33, new DateTime(2019, 6, 18))
            };

            products = new List<Product>
            {
                new Product(1, "Laptop", "Electronics", 999.99m, 50),
                new Product(2, "Mouse", "Electronics", 29.99m, 200),
                new Product(3, "Keyboard", "Electronics", 79.99m, 150),
                new Product(4, "Monitor", "Electronics", 299.99m, 75),
                new Product(5, "Chair", "Furniture", 199.99m, 30),
                new Product(6, "Desk", "Furniture", 399.99m, 20),
                new Product(7, "Book", "Education", 24.99m, 100),
                new Product(8, "Pen", "Office", 2.99m, 500),
                new Product(9, "Notebook", "Office", 5.99m, 300),
                new Product(10, "Coffee Mug", "Kitchen", 12.99m, 80)
            };

            orders = new List<Order>
            {
                new Order(1, 1, 1, 2, new DateTime(2023, 1, 15)),
                new Order(2, 2, 3, 1, new DateTime(2023, 1, 20)),
                new Order(3, 1, 2, 3, new DateTime(2023, 2, 5)),
                new Order(4, 3, 4, 1, new DateTime(2023, 2, 10)),
                new Order(5, 4, 5, 2, new DateTime(2023, 2, 15)),
                new Order(6, 2, 7, 5, new DateTime(2023, 3, 1)),
                new Order(7, 5, 8, 10, new DateTime(2023, 3, 5)),
                new Order(8, 3, 1, 1, new DateTime(2023, 3, 10))
            };
        }

        static void BasicFilteringDemo()
        {
            Console.WriteLine("--- Basic Filtering (Where) Demo ---");

            // Filter employees in IT department
            var itEmployees = employees.Where(e => e.Department == "IT").ToList();
            Console.WriteLine("IT Department employees:");
            itEmployees.ForEach(e => Console.WriteLine($"  {e}"));

            // Filter employees with salary > 70000
            var highSalaryEmployees = employees.Where(e => e.Salary > 70000).ToList();
            Console.WriteLine("\nEmployees with salary > $70,000:");
            highSalaryEmployees.ForEach(e => Console.WriteLine($"  {e}"));

            // Multiple conditions
            var youngItEmployees = employees
                .Where(e => e.Department == "IT" && e.Age < 30)
                .ToList();
            Console.WriteLine("\nYoung IT employees (Age < 30):");
            youngItEmployees.ForEach(e => Console.WriteLine($"  {e}"));

            // Filter products in stock
            var inStockProducts = products.Where(p => p.Stock > 0).ToList();
            Console.WriteLine($"\nProducts in stock: {inStockProducts.Count}");
        }

        static void ProjectionDemo()
        {
            Console.WriteLine("--- Projection (Select) Demo ---");

            // Select employee names only
            var employeeNames = employees.Select(e => e.Name).ToList();
            Console.WriteLine("Employee names:");
            employeeNames.ForEach(name => Console.WriteLine($"  {name}"));

            // Select anonymous objects
            var employeeSummary = employees
                .Select(e => new { e.Name, e.Department, e.Salary })
                .ToList();
            Console.WriteLine("\nEmployee summary:");
            employeeSummary.ForEach(e => Console.WriteLine($"  {e.Name} - {e.Department} (${e.Salary:N0})"));

            // Transform data
            var employeeCategories = employees
                .Select(e => new 
                { 
                    e.Name, 
                    SalaryCategory = e.Salary > 70000 ? "High" : "Standard",
                    YearsOfService = DateTime.Now.Year - e.HireDate.Year
                })
                .ToList();
            Console.WriteLine("\nEmployee categories:");
            employeeCategories.ForEach(e => Console.WriteLine($"  {e.Name} - {e.SalaryCategory} Salary, {e.YearsOfService} years"));

            // Select from products
            var productPriceList = products
                .Select(p => $"{p.Name}: ${p.Price:F2}")
                .ToList();
            Console.WriteLine("\nProduct price list:");
            productPriceList.ForEach(p => Console.WriteLine($"  {p}"));
        }

        static void SortingDemo()
        {
            Console.WriteLine("--- Sorting (OrderBy, ThenBy) Demo ---");

            // Sort employees by salary (ascending)
            var employeesBySalary = employees
                .OrderBy(e => e.Salary)
                .ToList();
            Console.WriteLine("Employees sorted by salary (ascending):");
            employeesBySalary.ForEach(e => Console.WriteLine($"  {e}"));

            // Sort employees by salary (descending)
            var employeesBySalaryDesc = employees
                .OrderByDescending(e => e.Salary)
                .ToList();
            Console.WriteLine("\nEmployees sorted by salary (descending):");
            employeesBySalaryDesc.ForEach(e => Console.WriteLine($"  {e}"));

            // Multiple sorting criteria
            var employeesSorted = employees
                .OrderBy(e => e.Department)
                .ThenByDescending(e => e.Salary)
                .ThenBy(e => e.Name)
                .ToList();
            Console.WriteLine("\nEmployees sorted by department, then salary (desc), then name:");
            employeesSorted.ForEach(e => Console.WriteLine($"  {e}"));

            // Sort products by price
            var productsByPrice = products
                .OrderBy(p => p.Price)
                .ToList();
            Console.WriteLine("\nProducts sorted by price:");
            productsByPrice.ForEach(p => Console.WriteLine($"  {p}"));
        }

        static void GroupingDemo()
        {
            Console.WriteLine("--- Grouping (GroupBy) Demo ---");

            // Group employees by department
            var employeesByDept = employees
                .GroupBy(e => e.Department)
                .ToList();
            Console.WriteLine("Employees grouped by department:");
            foreach (var group in employeesByDept)
            {
                Console.WriteLine($"  {group.Key} ({group.Count()} employees):");
                foreach (var emp in group)
                {
                    Console.WriteLine($"    {emp.Name} - ${emp.Salary:N0}");
                }
            }

            // Group products by category with additional info
            var productsByCategory = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(p => p.Price * p.Stock),
                    AveragePrice = g.Average(p => p.Price)
                })
                .ToList();
            Console.WriteLine("\nProduct categories summary:");
            productsByCategory.ForEach(g => 
                Console.WriteLine($"  {g.Category}: {g.Count} products, Total Value: ${g.TotalValue:N2}, Avg Price: ${g.AveragePrice:F2}"));

            // Group employees by age range
            var employeesByAgeGroup = employees
                .GroupBy(e => e.Age / 10 * 10) // Group by decade
                .OrderBy(g => g.Key)
                .ToList();
            Console.WriteLine("\nEmployees by age group:");
            foreach (var group in employeesByAgeGroup)
            {
                Console.WriteLine($"  {group.Key}s: {string.Join(", ", group.Select(e => e.Name))}");
            }
        }

        static void AggregationDemo()
        {
            Console.WriteLine("--- Aggregation Demo ---");

            // Count
            Console.WriteLine($"Total employees: {employees.Count()}");
            Console.WriteLine($"IT employees: {employees.Count(e => e.Department == "IT")}");
            Console.WriteLine($"Total products: {products.Count()}");

            // Sum
            Console.WriteLine($"Total payroll: ${employees.Sum(e => e.Salary):N2}");
            Console.WriteLine($"Total inventory value: ${products.Sum(p => p.Price * p.Stock):N2}");

            // Average
            Console.WriteLine($"Average salary: ${employees.Average(e => e.Salary):N2}");
            Console.WriteLine($"Average product price: ${products.Average(p => p.Price):F2}");
            Console.WriteLine($"Average age: {employees.Average(e => e.Age):F1} years");

            // Min/Max
            Console.WriteLine($"Minimum salary: ${employees.Min(e => e.Salary):N2}");
            Console.WriteLine($"Maximum salary: ${employees.Max(e => e.Salary):N2}");
            Console.WriteLine($"Cheapest product: ${products.Min(p => p.Price):F2}");
            Console.WriteLine($"Most expensive product: ${products.Max(p => p.Price):F2}");

            // Department-wise aggregations
            var deptStats = employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    Count = g.Count(),
                    TotalSalary = g.Sum(e => e.Salary),
                    AverageSalary = g.Average(e => e.Salary),
                    MinSalary = g.Min(e => e.Salary),
                    MaxSalary = g.Max(e => e.Salary)
                })
                .ToList();

            Console.WriteLine("\nDepartment statistics:");
            foreach (var dept in deptStats)
            {
                Console.WriteLine($"  {dept.Department}:");
                Console.WriteLine($"    Count: {dept.Count}, Total: ${dept.TotalSalary:N0}");
                Console.WriteLine($"    Average: ${dept.AverageSalary:N0}, Range: ${dept.MinSalary:N0} - ${dept.MaxSalary:N0}");
            }
        }

        static void SetOperationsDemo()
        {
            Console.WriteLine("--- Set Operations Demo ---");

            // Distinct
            var allDepartments = employees.Select(e => e.Department).Distinct().ToList();
            Console.WriteLine($"Distinct departments: {string.Join(", ", allDepartments)}");

            var allCategories = products.Select(p => p.Category).Distinct().ToList();
            Console.WriteLine($"Distinct categories: {string.Join(", ", allCategories)}");

            // Creating sample sets for demonstration
            var set1 = new List<int> { 1, 2, 3, 4, 5 };
            var set2 = new List<int> { 4, 5, 6, 7, 8 };

            Console.WriteLine($"\nSet 1: [{string.Join(", ", set1)}]");
            Console.WriteLine($"Set 2: [{string.Join(", ", set2)}]");

            // Union
            var union = set1.Union(set2).ToList();
            Console.WriteLine($"Union: [{string.Join(", ", union)}]");

            // Intersect
            var intersection = set1.Intersect(set2).ToList();
            Console.WriteLine($"Intersection: [{string.Join(", ", intersection)}]");

            // Except
            var difference = set1.Except(set2).ToList();
            Console.WriteLine($"Set1 except Set2: [{string.Join(", ", difference)}]");

            // Practical example with employee names
            var itEmployeeNames = employees.Where(e => e.Department == "IT").Select(e => e.Name);
            var seniorEmployeeNames = employees.Where(e => e.Age > 30).Select(e => e.Name);

            Console.WriteLine($"IT employees: {string.Join(", ", itEmployeeNames)}");
            Console.WriteLine($"Senior employees (>30): {string.Join(", ", seniorEmployeeNames)}");
            Console.WriteLine($"Senior IT employees: {string.Join(", ", itEmployeeNames.Intersect(seniorEmployeeNames))}");
        }

        static void QuantifiersDemo()
        {
            Console.WriteLine("--- Quantifiers (Any, All, Contains) Demo ---");

            // Any
            Console.WriteLine($"Any employees in IT? {employees.Any(e => e.Department == "IT")}");
            Console.WriteLine($"Any employees earning > $80k? {employees.Any(e => e.Salary > 80000)}");
            Console.WriteLine($"Any products out of stock? {products.Any(p => p.Stock == 0)}");

            // All
            Console.WriteLine($"All employees earn > $50k? {employees.All(e => e.Salary > 50000)}");
            Console.WriteLine($"All employees are adults? {employees.All(e => e.Age >= 18)}");
            Console.WriteLine($"All products have positive stock? {products.All(p => p.Stock > 0)}");

            // Contains
            var productNames = products.Select(p => p.Name).ToList();
            Console.WriteLine($"Products contain 'Laptop'? {productNames.Contains("Laptop")}");
            Console.WriteLine($"Products contain 'Phone'? {productNames.Contains("Phone")}");

            // Practical usage with complex conditions
            var hasHighPaidItEmployee = employees.Any(e => e.Department == "IT" && e.Salary > 75000);
            Console.WriteLine($"Has high-paid IT employee (>$75k)? {hasHighPaidItEmployee}");

            var allItEmployeesWellPaid = employees
                .Where(e => e.Department == "IT")
                .All(e => e.Salary > 70000);
            Console.WriteLine($"All IT employees earn > $70k? {allItEmployeesWellPaid}");

            // Check if any department has average salary > $70k
            var hasHighPayingDept = employees
                .GroupBy(e => e.Department)
                .Any(g => g.Average(e => e.Salary) > 70000);
            Console.WriteLine($"Any department with average salary > $70k? {hasHighPayingDept}");
        }

        static void JoinsDemo()
        {
            Console.WriteLine("--- Joins Demo ---");

            // Inner join - orders with employee and product details
            var orderDetails = from order in orders
                               join emp in employees on order.EmployeeId equals emp.Id
                               join prod in products on order.ProductId equals prod.Id
                               select new
                               {
                                   OrderId = order.OrderId,
                                   EmployeeName = emp.Name,
                                   ProductName = prod.Name,
                                   Quantity = order.Quantity,
                                   UnitPrice = prod.Price,
                                   Total = order.Quantity * prod.Price,
                                   OrderDate = order.OrderDate
                               };

            Console.WriteLine("Order details (Inner Join):");
            foreach (var detail in orderDetails)
            {
                Console.WriteLine($"  Order {detail.OrderId}: {detail.EmployeeName} ordered {detail.Quantity}x {detail.ProductName} (${detail.Total:F2}) on {detail.OrderDate:yyyy-MM-dd}");
            }

            // Group join - employees with their orders
            var employeeOrders = from emp in employees
                               join order in orders on emp.Id equals order.EmployeeId into empOrders
                               select new
                               {
                                   Employee = emp,
                                   OrderCount = empOrders.Count(),
                                   Orders = empOrders.ToList()
                               };

            Console.WriteLine("\nEmployees with their order count:");
            foreach (var empOrder in employeeOrders)
            {
                Console.WriteLine($"  {empOrder.Employee.Name}: {empOrder.OrderCount} orders");
            }

            // Method syntax join
            var orderSummary = orders
                .Join(employees, o => o.EmployeeId, e => e.Id, (o, e) => new { Order = o, Employee = e })
                .Join(products, oe => oe.Order.ProductId, p => p.Id, (oe, p) => new
                {
                    EmployeeName = oe.Employee.Name,
                    ProductName = p.Name,
                    OrderValue = oe.Order.Quantity * p.Price
                })
                .GroupBy(x => x.EmployeeName)
                .Select(g => new
                {
                    EmployeeName = g.Key,
                    TotalOrderValue = g.Sum(x => x.OrderValue),
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.TotalOrderValue)
                .ToList();

            Console.WriteLine("\nEmployee order summary (total value):");
            orderSummary.ForEach(s => Console.WriteLine($"  {s.EmployeeName}: ${s.TotalOrderValue:F2} ({s.OrderCount} orders)"));
        }

        static void AdvancedQueriesDemo()
        {
            Console.WriteLine("--- Advanced Queries Demo ---");

            // Complex query 1: Top performing employees by department
            var topPerformers = employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    TopPerformer = g.OrderByDescending(e => e.Salary).First(),
                    AverageSalary = g.Average(e => e.Salary)
                })
                .ToList();

            Console.WriteLine("Top performer by department:");
            topPerformers.ForEach(tp => 
                Console.WriteLine($"  {tp.Department}: {tp.TopPerformer.Name} (${tp.TopPerformer.Salary:N0}) - Dept Avg: ${tp.AverageSalary:N0}"));

            // Complex query 2: Products that were never ordered
            var unorderedProducts = products
                .Where(p => !orders.Any(o => o.ProductId == p.Id))
                .ToList();

            Console.WriteLine($"\nProducts never ordered ({unorderedProducts.Count}):");
            unorderedProducts.ForEach(p => Console.WriteLine($"  {p.Name} - ${p.Price:F2}"));

            // Complex query 3: Employee productivity analysis
            var employeeProductivity = employees
                .Select(e => new
                {
                    Employee = e,
                    OrderCount = orders.Count(o => o.EmployeeId == e.Id),
                    TotalQuantityOrdered = orders.Where(o => o.EmployeeId == e.Id).Sum(o => o.Quantity),
                    TotalOrderValue = orders
                        .Where(o => o.EmployeeId == e.Id)
                        .Join(products, o => o.ProductId, p => p.Id, (o, p) => o.Quantity * p.Price)
                        .Sum()
                })
                .Where(ep => ep.OrderCount > 0)
                .OrderByDescending(ep => ep.TotalOrderValue)
                .ToList();

            Console.WriteLine("\nEmployee productivity (with orders):");
            employeeProductivity.ForEach(ep => 
                Console.WriteLine($"  {ep.Employee.Name}: {ep.OrderCount} orders, {ep.TotalQuantityOrdered} items, ${ep.TotalOrderValue:F2} value"));

            // Complex query 4: Monthly order analysis
            var monthlyAnalysis = orders
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    OrderCount = g.Count(),
                    TotalQuantity = g.Sum(o => o.Quantity),
                    UniqueEmployees = g.Select(o => o.EmployeeId).Distinct().Count(),
                    UniqueProducts = g.Select(o => o.ProductId).Distinct().Count()
                })
                .OrderBy(ma => ma.Month)
                .ToList();

            Console.WriteLine("\nMonthly order analysis:");
            monthlyAnalysis.ForEach(ma => 
                Console.WriteLine($"  {ma.Month}: {ma.OrderCount} orders, {ma.TotalQuantity} items, {ma.UniqueEmployees} employees, {ma.UniqueProducts} products"));
        }

        static void SyntaxComparisonDemo()
        {
            Console.WriteLine("--- Method Syntax vs Query Syntax ---");

            // Query Syntax
            Console.WriteLine("1. Query Syntax:");
            var queryResult = from e in employees
                            where e.Department == "IT" && e.Salary > 70000
                            orderby e.Salary descending
                            select new { e.Name, e.Salary };

            Console.WriteLine("High-paid IT employees (Query Syntax):");
            foreach (var emp in queryResult)
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary:N0}");
            }

            // Method Syntax (equivalent)
            Console.WriteLine("\n2. Method Syntax (equivalent):");
            var methodResult = employees
                .Where(e => e.Department == "IT" && e.Salary > 70000)
                .OrderByDescending(e => e.Salary)
                .Select(e => new { e.Name, e.Salary });

            Console.WriteLine("High-paid IT employees (Method Syntax):");
            foreach (var emp in methodResult)
            {
                Console.WriteLine($"  {emp.Name}: ${emp.Salary:N0}");
            }

            // Complex example - Query Syntax
            Console.WriteLine("\n3. Complex Query - Query Syntax:");
            var complexQuery = from e in employees
                             join o in orders on e.Id equals o.EmployeeId into empOrders
                             where empOrders.Any()
                             select new
                             {
                                 e.Name,
                                 e.Department,
                                 OrderCount = empOrders.Count()
                             } into result
                             where result.OrderCount > 1
                             orderby result.OrderCount descending
                             select result;

            Console.WriteLine("Employees with multiple orders (Query Syntax):");
            foreach (var emp in complexQuery)
            {
                Console.WriteLine($"  {emp.Name} ({emp.Department}): {emp.OrderCount} orders");
            }

            // Complex example - Method Syntax
            Console.WriteLine("\n4. Complex Query - Method Syntax (equivalent):");
            var complexMethod = employees
                .GroupJoin(orders, e => e.Id, o => o.EmployeeId, (e, empOrders) => new { e, empOrders })
                .Where(x => x.empOrders.Any())
                .Select(x => new
                {
                    x.e.Name,
                    x.e.Department,
                    OrderCount = x.empOrders.Count()
                })
                .Where(result => result.OrderCount > 1)
                .OrderByDescending(result => result.OrderCount);

            Console.WriteLine("Employees with multiple orders (Method Syntax):");
            foreach (var emp in complexMethod)
            {
                Console.WriteLine($"  {emp.Name} ({emp.Department}): {emp.OrderCount} orders");
            }

            Console.WriteLine("\nKey Differences:");
            Console.WriteLine("• Query Syntax: More SQL-like, readable for complex queries");
            Console.WriteLine("• Method Syntax: More flexible, better for chaining operations");
            Console.WriteLine("• Both compile to the same IL code");
            Console.WriteLine("• Choose based on readability and team preference");
        }
    }
}
