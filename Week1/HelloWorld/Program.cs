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
