using System;

namespace ClassBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# Classes and Objects Demonstration\n");

            // Create a Person object
            Console.WriteLine("=== Person Class ===");
            Person person = new Person("John Doe", 30);
            person.Introduce();
            person.Celebrate();
            Console.WriteLine();

            // Create a Student object (derived from Person)
            Console.WriteLine("=== Student Class (inherits from Person) ===");
            Student student = new Student("Jane Smith", 20, "Computer Science");
            student.Introduce();  // Calls the overridden method
            student.Study();
            student.Celebrate();  // Calls the inherited method
            Console.WriteLine();

            // Create a Teacher object (derived from Person)
            Console.WriteLine("=== Teacher Class (inherits from Person) ===");
            Teacher teacher = new Teacher("Prof. Johnson", 45, "Mathematics");
            teacher.Introduce();  // Calls the overridden method
            teacher.Teach();
            teacher.Celebrate();  // Calls the inherited method
            Console.WriteLine();

            // Demonstrate polymorphism
            Console.WriteLine("=== Demonstrating Polymorphism ===");
            Person[] people = new Person[]
            {
                new Person("Alex", 25),
                new Student("Sarah", 22, "Biology"),
                new Teacher("Dr. Brown", 50, "Physics")
            };

            foreach (Person p in people)
            {
                // The appropriate Introduce method will be called
                // based on the actual object type (polymorphism)
                p.Introduce();
            }

            // Wait for user to press a key before closing
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    // Base class
    class Person
    {
        // Properties
        public string Name { get; set; }
        public int Age { get; set; }

        // Constructor
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        // Virtual method (can be overridden by derived classes)
        public virtual void Introduce()
        {
            Console.WriteLine($"Hello, my name is {Name} and I am {Age} years old.");
        }

        // Regular method
        public void Celebrate()
        {
            Console.WriteLine($"{Name} is celebrating their birthday!");
        }
    }

    // Derived class (inherits from Person)
    class Student : Person
    {
        // Additional property
        public string Major { get; set; }

        // Constructor that calls the base class constructor
        public Student(string name, int age, string major)
            : base(name, age)
        {
            Major = major;
        }

        // Override the Introduce method
        public override void Introduce()
        {
            Console.WriteLine($"Hello, my name is {Name}, I am {Age} years old, and I am studying {Major}.");
        }

        // Additional method specific to Student
        public void Study()
        {
            Console.WriteLine($"{Name} is studying {Major}.");
        }
    }

    // Another derived class (inherits from Person)
    class Teacher : Person
    {
        // Additional property
        public string Subject { get; set; }

        // Constructor that calls the base class constructor
        public Teacher(string name, int age, string subject)
            : base(name, age)
        {
            Subject = subject;
        }

        // Override the Introduce method
        public override void Introduce()
        {
            Console.WriteLine($"Hello, my name is {Name}, I am {Age} years old, and I teach {Subject}.");
        }

        // Additional method specific to Teacher
        public void Teach()
        {
            Console.WriteLine($"{Name} is teaching {Subject}.");
        }
    }
}
