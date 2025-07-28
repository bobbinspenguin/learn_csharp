using System;

namespace InterfacesAndAbstracts
{
    // Interface example - defines a contract
    public interface IShape
    {
        double CalculateArea();
        double CalculatePerimeter();
        void DisplayInfo();
    }

    // Interface for drawable objects
    public interface IDrawable
    {
        void Draw();
    }

    // Abstract class example - provides some implementation and requires some to be implemented
    public abstract class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Animal(string name, int age)
        {
            Name = name;
            Age = age;
        }

        // Concrete method that all animals share
        public void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping.");
        }

        // Abstract method that must be implemented by derived classes
        public abstract void MakeSound();
        public abstract void Move();
    }

    // Concrete implementation of IShape
    public class Circle : IShape, IDrawable
    {
        public double Radius { get; set; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }

        public double CalculatePerimeter()
        {
            return 2 * Math.PI * Radius;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Circle - Radius: {Radius:F2}, Area: {CalculateArea():F2}, Perimeter: {CalculatePerimeter():F2}");
        }

        public void Draw()
        {
            Console.WriteLine($"Drawing a circle with radius {Radius}");
        }
    }

    public class Rectangle : IShape, IDrawable
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public double CalculateArea()
        {
            return Width * Height;
        }

        public double CalculatePerimeter()
        {
            return 2 * (Width + Height);
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Rectangle - Width: {Width:F2}, Height: {Height:F2}, Area: {CalculateArea():F2}, Perimeter: {CalculatePerimeter():F2}");
        }

        public void Draw()
        {
            Console.WriteLine($"Drawing a rectangle {Width} x {Height}");
        }
    }

    public class Triangle : IShape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(double sideA, double sideB, double sideC)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }

        public double CalculateArea()
        {
            // Using Heron's formula
            double s = (SideA + SideB + SideC) / 2;
            return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
        }

        public double CalculatePerimeter()
        {
            return SideA + SideB + SideC;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Triangle - Sides: {SideA:F2}, {SideB:F2}, {SideC:F2}, Area: {CalculateArea():F2}, Perimeter: {CalculatePerimeter():F2}");
        }
    }

    // Concrete implementations of abstract Animal class
    public class Dog : Animal
    {
        public string Breed { get; set; }

        public Dog(string name, int age, string breed) : base(name, age)
        {
            Breed = breed;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} the {Breed} says: Woof! Woof!");
        }

        public override void Move()
        {
            Console.WriteLine($"{Name} is running around happily.");
        }
    }

    public class Cat : Animal
    {
        public bool IsIndoor { get; set; }

        public Cat(string name, int age, bool isIndoor) : base(name, age)
        {
            IsIndoor = isIndoor;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Meow!");
        }

        public override void Move()
        {
            string location = IsIndoor ? "around the house" : "outside";
            Console.WriteLine($"{Name} is gracefully walking {location}.");
        }
    }

    public class Bird : Animal
    {
        public bool CanFly { get; set; }

        public Bird(string name, int age, bool canFly) : base(name, age)
        {
            CanFly = canFly;
        }

        public override void MakeSound()
        {
            Console.WriteLine($"{Name} says: Tweet! Tweet!");
        }

        public override void Move()
        {
            if (CanFly)
                Console.WriteLine($"{Name} is flying high in the sky.");
            else
                Console.WriteLine($"{Name} is hopping on the ground.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Interfaces and Abstract Classes Demo ===\n");

            // Interface demonstration
            Console.WriteLine("--- Interface Implementation (IShape) ---");
            IShape[] shapes = 
            {
                new Circle(5.0),
                new Rectangle(4.0, 6.0),
                new Triangle(3.0, 4.0, 5.0)
            };

            foreach (IShape shape in shapes)
            {
                shape.DisplayInfo();
            }

            Console.WriteLine("\n--- Multiple Interface Implementation ---");
            IDrawable[] drawables = 
            {
                new Circle(3.0),
                new Rectangle(2.0, 8.0)
            };

            foreach (IDrawable drawable in drawables)
            {
                drawable.Draw();
            }

            // Abstract class demonstration
            Console.WriteLine("\n--- Abstract Class Implementation (Animal) ---");
            Animal[] animals = 
            {
                new Dog("Buddy", 5, "Golden Retriever"),
                new Cat("Whiskers", 3, true),
                new Bird("Tweety", 2, true)
            };

            foreach (Animal animal in animals)
            {
                Console.WriteLine($"\n{animal.Name} (Age: {animal.Age}):");
                animal.MakeSound();
                animal.Move();
                animal.Sleep();
            }

            Console.WriteLine("\n--- Key Differences ---");
            Console.WriteLine("• Interfaces define contracts (what must be implemented)");
            Console.WriteLine("• Abstract classes provide partial implementation (shared behavior + requirements)");
            Console.WriteLine("• Classes can implement multiple interfaces but inherit from only one abstract class");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
