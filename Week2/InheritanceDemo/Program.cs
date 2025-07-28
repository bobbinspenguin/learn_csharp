using System;

namespace InheritanceDemo
{
    // Base class demonstrating encapsulation
    public class Person
    {
        private string _name;
        private int _age;

        public string Name 
        { 
            get => _name; 
            set => _name = !string.IsNullOrEmpty(value) ? value : throw new ArgumentException("Name cannot be empty");
        }

        public int Age 
        { 
            get => _age; 
            set => _age = value >= 0 ? value : throw new ArgumentException("Age cannot be negative");
        }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public virtual void Introduce()
        {
            Console.WriteLine($"Hello, I'm {Name} and I'm {Age} years old.");
        }

        public virtual void DisplayRole()
        {
            Console.WriteLine("I am a person.");
        }
    }

    // Derived class - Student
    public class Student : Person
    {
        public string StudentId { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }

        public Student(string name, int age, string studentId, string major, double gpa) 
            : base(name, age)
        {
            StudentId = studentId;
            Major = major;
            GPA = gpa;
        }

        public override void Introduce()
        {
            base.Introduce();
            Console.WriteLine($"I'm studying {Major} with a GPA of {GPA:F2}.");
        }

        public override void DisplayRole()
        {
            Console.WriteLine("I am a student.");
        }

        public void Study()
        {
            Console.WriteLine($"{Name} is studying {Major}.");
        }
    }

    // Derived class - Teacher
    public class Teacher : Person
    {
        public string EmployeeId { get; set; }
        public string Subject { get; set; }
        public int YearsOfExperience { get; set; }

        public Teacher(string name, int age, string employeeId, string subject, int experience) 
            : base(name, age)
        {
            EmployeeId = employeeId;
            Subject = subject;
            YearsOfExperience = experience;
        }

        public override void Introduce()
        {
            base.Introduce();
            Console.WriteLine($"I teach {Subject} and have {YearsOfExperience} years of experience.");
        }

        public override void DisplayRole()
        {
            Console.WriteLine("I am a teacher.");
        }

        public void Teach()
        {
            Console.WriteLine($"{Name} is teaching {Subject}.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Inheritance and Polymorphism Demo ===\n");

            // Create instances of different classes
            Person person = new Person("John Doe", 30);
            Student student = new Student("Alice Smith", 20, "S001", "Computer Science", 3.75);
            Teacher teacher = new Teacher("Dr. Johnson", 45, "T001", "Mathematics", 15);

            // Array of Person references (polymorphism)
            Person[] people = { person, student, teacher };

            // Demonstrate polymorphism
            Console.WriteLine("--- Polymorphism Demo ---");
            foreach (Person p in people)
            {
                p.Introduce();
                p.DisplayRole();
                Console.WriteLine();
            }

            // Demonstrate specific methods
            Console.WriteLine("--- Specific Class Methods ---");
            student.Study();
            teacher.Teach();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
