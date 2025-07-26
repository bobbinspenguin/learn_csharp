using System;

namespace BasicSyntaxPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Welcome message
            Console.WriteLine("Welcome to C# Basic Syntax Practice!\n");
            
            // Variables and data types demonstration
            VariablesAndDataTypes();
            
            // Operators demonstration
            Operators();
            
            // Simple calculator using control flow statements
            SimpleCalculator();
            
            // Wait for user to press a key before closing
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        
        static void VariablesAndDataTypes()
        {
            Console.WriteLine("---VARIABLES AND DATA TYPES---");
            
            // Integer types
            int intValue = 42;
            long longValue = 9223372036854775807;
            
            // Floating-point types
            float floatValue = 3.14f;
            double doubleValue = 3.14159265359;
            decimal decimalValue = 3.14159265359m; // More precise, used for financial calculations
            
            // Character and string types
            char charValue = 'A';
            string stringValue = "Hello, C#!";
            
            // Boolean type
            bool boolValue = true;
            
            // Display all values
            Console.WriteLine($"int: {intValue}");
            Console.WriteLine($"long: {longValue}");
            Console.WriteLine($"float: {floatValue}");
            Console.WriteLine($"double: {doubleValue}");
            Console.WriteLine($"decimal: {decimalValue}");
            Console.WriteLine($"char: {charValue}");
            Console.WriteLine($"string: {stringValue}");
            Console.WriteLine($"bool: {boolValue}");
            
            // Type conversion
            Console.WriteLine("\nType Conversion:");
            int converted = (int)doubleValue; // Explicit conversion (casting)
            Console.WriteLine($"Double {doubleValue} converted to int: {converted}");
            
            string numberString = "123";
            int parsedNumber = int.Parse(numberString);
            Console.WriteLine($"String \"{numberString}\" parsed to int: {parsedNumber}");
            
            Console.WriteLine();
        }
        
        static void Operators()
        {
            Console.WriteLine("---OPERATORS---");
            
            // Arithmetic operators
            int a = 10;
            int b = 3;
            Console.WriteLine("Arithmetic Operators:");
            Console.WriteLine($"{a} + {b} = {a + b}"); // Addition
            Console.WriteLine($"{a} - {b} = {a - b}"); // Subtraction
            Console.WriteLine($"{a} * {b} = {a * b}"); // Multiplication
            Console.WriteLine($"{a} / {b} = {a / b}"); // Division (integer division)
            Console.WriteLine($"{a} % {b} = {a % b}"); // Modulus (remainder)
            
            // Comparison operators
            Console.WriteLine("\nComparison Operators:");
            Console.WriteLine($"{a} == {b}: {a == b}"); // Equal to
            Console.WriteLine($"{a} != {b}: {a != b}"); // Not equal to
            Console.WriteLine($"{a} > {b}: {a > b}");  // Greater than
            Console.WriteLine($"{a} < {b}: {a < b}");  // Less than
            Console.WriteLine($"{a} >= {b}: {a >= b}"); // Greater than or equal to
            Console.WriteLine($"{a} <= {b}: {a <= b}"); // Less than or equal to
            
            // Logical operators
            bool x = true;
            bool y = false;
            Console.WriteLine("\nLogical Operators:");
            Console.WriteLine($"{x} AND {y}: {x && y}"); // Logical AND
            Console.WriteLine($"{x} OR {y}: {x || y}");  // Logical OR
            Console.WriteLine($"NOT {x}: {!x}");        // Logical NOT
            
            Console.WriteLine();
        }
        
        static void SimpleCalculator()
        {
            Console.WriteLine("---SIMPLE CALCULATOR---");
            
            try
            {
                // Get first number
                Console.Write("Enter first number: ");
                double num1 = Convert.ToDouble(Console.ReadLine());
                
                // Get second number
                Console.Write("Enter second number: ");
                double num2 = Convert.ToDouble(Console.ReadLine());
                
                // Get operation
                Console.Write("Enter operation (+, -, *, /): ");
                char operation = Convert.ToChar(Console.ReadLine());
                
                // Calculate and display result using switch statement
                double result = 0;
                bool validOperation = true;
                
                switch (operation)
                {
                    case '+':
                        result = num1 + num2;
                        break;
                    case '-':
                        result = num1 - num2;
                        break;
                    case '*':
                        result = num1 * num2;
                        break;
                    case '/':
                        // Handle division by zero
                        if (num2 == 0)
                        {
                            Console.WriteLine("Error: Cannot divide by zero!");
                            validOperation = false;
                        }
                        else
                        {
                            result = num1 / num2;
                        }
                        break;
                    default:
                        Console.WriteLine("Error: Invalid operation!");
                        validOperation = false;
                        break;
                }
                
                if (validOperation)
                {
                    Console.WriteLine($"Result: {num1} {operation} {num2} = {result}");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter valid numbers!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
