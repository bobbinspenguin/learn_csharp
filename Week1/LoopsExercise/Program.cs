using System;

namespace LoopsExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to C# Loops Exercise!\n");
            
            bool continueProgram = true;
            
            while (continueProgram)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Multiplication Table (for loop)");
                Console.WriteLine("2. Sum Calculator (while loop)");
                Console.WriteLine("3. Number Validator (do-while loop)");
                Console.WriteLine("4. Exit");
                Console.Write("\nEnter your choice (1-4): ");
                
                string choice = Console.ReadLine();
                Console.WriteLine();
                
                switch (choice)
                {
                    case "1":
                        MultiplicationTable();
                        break;
                    case "2":
                        SumCalculator();
                        break;
                    case "3":
                        NumberValidator();
                        break;
                    case "4":
                        continueProgram = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
            }
            
            // Wait for user to press a key before closing
            if (!continueProgram)
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
        
        static void MultiplicationTable()
        {
            Console.WriteLine("--- MULTIPLICATION TABLE (FOR LOOP) ---");
            
            // Get number from user
            Console.Write("Enter a number to generate its multiplication table: ");
            if (int.TryParse(Console.ReadLine(), out int number))
            {
                // Define table range
                int start = 1;
                int end = 10;
                
                // Generate multiplication table using for loop
                Console.WriteLine($"\nMultiplication table for {number}:");
                for (int i = start; i <= end; i++)
                {
                    Console.WriteLine($"{number} x {i} = {number * i}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
            
            Console.WriteLine();
        }
        
        static void SumCalculator()
        {
            Console.WriteLine("--- SUM CALCULATOR (WHILE LOOP) ---");
            
            // Get number from user
            Console.Write("Enter a positive integer to find sum from 1 to n: ");
            if (int.TryParse(Console.ReadLine(), out int n) && n > 0)
            {
                // Calculate sum using while loop
                int sum = 0;
                int counter = 1;
                
                while (counter <= n)
                {
                    sum += counter;
                    counter++;
                }
                
                Console.WriteLine($"\nSum of numbers from 1 to {n} is: {sum}");
                
                // Formula verification: sum = n*(n+1)/2
                int formulaSum = n * (n + 1) / 2;
                Console.WriteLine($"Verification using formula: {formulaSum}");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive integer.");
            }
            
            Console.WriteLine();
        }
        
        static void NumberValidator()
        {
            Console.WriteLine("--- NUMBER VALIDATOR (DO-WHILE LOOP) ---");
            Console.WriteLine("This program validates if the input is within range (1-100)");
            
            int number;
            string input;
            bool isValid;
            
            do
            {
                Console.Write("Enter a number between 1 and 100: ");
                input = Console.ReadLine();
                
                // Check if input is a valid number
                isValid = int.TryParse(input, out number);
                
                if (!isValid)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.\n");
                }
                else if (number < 1 || number > 100)
                {
                    Console.WriteLine("Number out of range. Please enter a number between 1 and 100.\n");
                    isValid = false;
                }
                
            } while (!isValid);
            
            Console.WriteLine($"\nValid input received: {number} is within range!");
            
            // Additional feedback based on the number
            if (number <= 50)
            {
                Console.WriteLine($"{number} is in the lower half (1-50)");
            }
            else
            {
                Console.WriteLine($"{number} is in the upper half (51-100)");
            }
            
            Console.WriteLine();
        }
    }
}
