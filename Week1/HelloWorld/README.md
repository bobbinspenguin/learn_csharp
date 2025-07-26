# First C# Console Application

This is a simple "Hello World" console application to get you started with C# programming.

## Steps to create this project:

1. In Visual Studio, create a new project
2. Select "Console App (.NET Core)" or "Console App (.NET)"
3. Name it "HelloWorld"
4. Replace the default Program.cs code with the provided sample

## Program.cs

```csharp
using System;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, C# World!");
            
            // Get user input
            Console.Write("What's your name? ");
            string name = Console.ReadLine();
            
            // Display personalized message
            Console.WriteLine($"Hello, {name}! Welcome to the world of C#!");
            
            // Wait for user to press a key before closing
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
```

## Concepts Covered:
- Console input and output
- Variables and string manipulation 
- String interpolation with `$"...{variable}..."`
- Basic program structure and entry point (Main method)

## Next Steps:
- Try modifying the greeting message
- Add more questions and responses
- Experiment with different data types
