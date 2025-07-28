using System;
using System.Collections.Generic;
using System.Collections;

namespace CollectionsDemo
{
    // Generic class example
    public class SimpleStack<T>
    {
        private List<T> items = new List<T>();

        public void Push(T item)
        {
            items.Add(item);
        }

        public T Pop()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Stack is empty");
            
            T item = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return item;
        }

        public T Peek()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Stack is empty");
            
            return items[items.Count - 1];
        }

        public int Count => items.Count;
        public bool IsEmpty => items.Count == 0;

        public void Display()
        {
            Console.WriteLine($"Stack contents (top to bottom): [{string.Join(", ", items.AsEnumerable().Reverse())}]");
        }
    }

    // Student class for demonstration
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Major { get; set; }
        public double GPA { get; set; }

        public Student(int id, string name, int age, string major, double gpa)
        {
            Id = id;
            Name = name;
            Age = age;
            Major = major;
            GPA = gpa;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Major} (Age: {Age}, GPA: {GPA:F2})";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Collections and Generics Demo ===\n");

            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("Choose a demo:");
                Console.WriteLine("1. Arrays vs Lists");
                Console.WriteLine("2. Dictionary Operations");
                Console.WriteLine("3. HashSet (Unique Collections)");
                Console.WriteLine("4. Queue and Stack");
                Console.WriteLine("5. Custom Generic Stack");
                Console.WriteLine("6. Generic Methods Demo");
                Console.WriteLine("7. Complex Collections (Students)");
                Console.WriteLine("8. Exit");
                Console.Write("\nEnter your choice (1-8): ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ArraysVsListsDemo();
                        break;
                    case "2":
                        DictionaryDemo();
                        break;
                    case "3":
                        HashSetDemo();
                        break;
                    case "4":
                        QueueAndStackDemo();
                        break;
                    case "5":
                        CustomGenericStackDemo();
                        break;
                    case "6":
                        GenericMethodsDemo();
                        break;
                    case "7":
                        ComplexCollectionsDemo();
                        break;
                    case "8":
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

        static void ArraysVsListsDemo()
        {
            Console.WriteLine("--- Arrays vs Lists Demo ---");

            // Arrays - fixed size
            Console.WriteLine("1. Arrays (Fixed Size):");
            int[] numbersArray = new int[5] { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Array: [{string.Join(", ", numbersArray)}]");
            Console.WriteLine($"Array Length: {numbersArray.Length}");
            
            // Modifying array
            numbersArray[2] = 10;
            Console.WriteLine($"After modification: [{string.Join(", ", numbersArray)}]");

            // Lists - dynamic size
            Console.WriteLine("\n2. Lists (Dynamic Size):");
            List<int> numbersList = new List<int> { 1, 2, 3, 4, 5 };
            Console.WriteLine($"List: [{string.Join(", ", numbersList)}]");
            Console.WriteLine($"List Count: {numbersList.Count}");

            // Adding elements
            numbersList.Add(6);
            numbersList.AddRange(new int[] { 7, 8, 9 });
            Console.WriteLine($"After adding elements: [{string.Join(", ", numbersList)}]");

            // Removing elements
            numbersList.Remove(5); // Remove first occurrence of 5
            numbersList.RemoveAt(0); // Remove element at index 0
            Console.WriteLine($"After removing elements: [{string.Join(", ", numbersList)}]");

            // List methods
            Console.WriteLine($"Contains 3: {numbersList.Contains(3)}");
            Console.WriteLine($"Index of 4: {numbersList.IndexOf(4)}");
        }

        static void DictionaryDemo()
        {
            Console.WriteLine("--- Dictionary Demo ---");

            // Creating and populating dictionary
            Dictionary<string, int> ages = new Dictionary<string, int>
            {
                {"Alice", 25},
                {"Bob", 30},
                {"Charlie", 35}
            };

            Console.WriteLine("Initial dictionary:");
            DisplayDictionary(ages);

            // Adding new entries
            ages["David"] = 28;
            ages.Add("Eve", 32);
            Console.WriteLine("\nAfter adding David and Eve:");
            DisplayDictionary(ages);

            // Accessing values
            Console.WriteLine($"\nAlice's age: {ages["Alice"]}");
            
            // Safe access using TryGetValue
            if (ages.TryGetValue("Frank", out int frankAge))
                Console.WriteLine($"Frank's age: {frankAge}");
            else
                Console.WriteLine("Frank not found in dictionary");

            // Updating values
            ages["Bob"] = 31;
            Console.WriteLine("\nAfter updating Bob's age:");
            DisplayDictionary(ages);

            // Checking existence
            Console.WriteLine($"\nContains key 'Alice': {ages.ContainsKey("Alice")}");
            Console.WriteLine($"Contains value 25: {ages.ContainsValue(25)}");

            // Removing entries
            ages.Remove("Charlie");
            Console.WriteLine("\nAfter removing Charlie:");
            DisplayDictionary(ages);

            // Iterating through dictionary
            Console.WriteLine("\nIterating through keys and values:");
            foreach (var kvp in ages)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        static void DisplayDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        static void HashSetDemo()
        {
            Console.WriteLine("--- HashSet Demo (Unique Collections) ---");

            // Creating HashSets
            HashSet<string> fruits = new HashSet<string> { "apple", "banana", "orange" };
            HashSet<string> citrus = new HashSet<string> { "orange", "lemon", "lime", "grapefruit" };

            Console.WriteLine($"Fruits: [{string.Join(", ", fruits)}]");
            Console.WriteLine($"Citrus: [{string.Join(", ", citrus)}]");

            // Adding duplicates (won't be added)
            fruits.Add("apple"); // This won't add a duplicate
            fruits.Add("grape");
            Console.WriteLine($"Fruits after adding 'apple' and 'grape': [{string.Join(", ", fruits)}]");

            // Set operations
            HashSet<string> union = new HashSet<string>(fruits);
            union.UnionWith(citrus);
            Console.WriteLine($"Union: [{string.Join(", ", union)}]");

            HashSet<string> intersection = new HashSet<string>(fruits);
            intersection.IntersectWith(citrus);
            Console.WriteLine($"Intersection: [{string.Join(", ", intersection)}]");

            HashSet<string> difference = new HashSet<string>(fruits);
            difference.ExceptWith(citrus);
            Console.WriteLine($"Fruits except Citrus: [{string.Join(", ", difference)}]");

            // Checking relationships
            Console.WriteLine($"Is citrus a subset of union? {citrus.IsSubsetOf(union)}");
            Console.WriteLine($"Do fruits and citrus overlap? {fruits.Overlaps(citrus)}");
        }

        static void QueueAndStackDemo()
        {
            Console.WriteLine("--- Queue and Stack Demo ---");

            // Queue (FIFO - First In, First Out)
            Console.WriteLine("1. Queue (FIFO):");
            Queue<string> customerQueue = new Queue<string>();
            
            customerQueue.Enqueue("Alice");
            customerQueue.Enqueue("Bob");
            customerQueue.Enqueue("Charlie");
            Console.WriteLine($"Queue: [{string.Join(", ", customerQueue)}]");

            Console.WriteLine($"Serving: {customerQueue.Dequeue()}");
            Console.WriteLine($"Next in line: {customerQueue.Peek()}");
            Console.WriteLine($"Queue after serving Alice: [{string.Join(", ", customerQueue)}]");

            // Stack (LIFO - Last In, First Out)
            Console.WriteLine("\n2. Stack (LIFO):");
            Stack<string> bookStack = new Stack<string>();
            
            bookStack.Push("Book 1");
            bookStack.Push("Book 2");
            bookStack.Push("Book 3");
            Console.WriteLine($"Stack: [{string.Join(", ", bookStack)}]");

            Console.WriteLine($"Taking: {bookStack.Pop()}");
            Console.WriteLine($"Top book: {bookStack.Peek()}");
            Console.WriteLine($"Stack after taking Book 3: [{string.Join(", ", bookStack)}]");
        }

        static void CustomGenericStackDemo()
        {
            Console.WriteLine("--- Custom Generic Stack Demo ---");

            // String stack
            SimpleStack<string> stringStack = new SimpleStack<string>();
            stringStack.Push("First");
            stringStack.Push("Second");
            stringStack.Push("Third");
            
            Console.WriteLine("String Stack:");
            stringStack.Display();
            Console.WriteLine($"Popped: {stringStack.Pop()}");
            stringStack.Display();

            // Integer stack
            SimpleStack<int> intStack = new SimpleStack<int>();
            intStack.Push(10);
            intStack.Push(20);
            intStack.Push(30);
            
            Console.WriteLine("\nInteger Stack:");
            intStack.Display();
            Console.WriteLine($"Peek: {intStack.Peek()}");
            intStack.Display();
        }

        static void GenericMethodsDemo()
        {
            Console.WriteLine("--- Generic Methods Demo ---");

            // Generic method for swapping
            int a = 5, b = 10;
            Console.WriteLine($"Before swap: a = {a}, b = {b}");
            Swap(ref a, ref b);
            Console.WriteLine($"After swap: a = {a}, b = {b}");

            string x = "Hello", y = "World";
            Console.WriteLine($"Before swap: x = {x}, y = {y}");
            Swap(ref x, ref y);
            Console.WriteLine($"After swap: x = {x}, y = {y}");

            // Generic method for finding maximum
            int[] numbers = { 3, 7, 2, 9, 1 };
            Console.WriteLine($"Numbers: [{string.Join(", ", numbers)}]");
            Console.WriteLine($"Maximum: {FindMaximum(numbers)}");

            string[] words = { "apple", "zebra", "banana", "cherry" };
            Console.WriteLine($"Words: [{string.Join(", ", words)}]");
            Console.WriteLine($"Maximum (lexicographically): {FindMaximum(words)}");
        }

        static void Swap<T>(ref T first, ref T second)
        {
            T temp = first;
            first = second;
            second = temp;
        }

        static T FindMaximum<T>(T[] array) where T : IComparable<T>
        {
            if (array.Length == 0)
                throw new ArgumentException("Array cannot be empty");

            T max = array[0];
            foreach (T item in array)
            {
                if (item.CompareTo(max) > 0)
                    max = item;
            }
            return max;
        }

        static void ComplexCollectionsDemo()
        {
            Console.WriteLine("--- Complex Collections Demo (Students) ---");

            // List of students
            List<Student> students = new List<Student>
            {
                new Student(1, "Alice Johnson", 20, "Computer Science", 3.8),
                new Student(2, "Bob Smith", 22, "Mathematics", 3.6),
                new Student(3, "Charlie Brown", 19, "Physics", 3.9),
                new Student(4, "Diana Prince", 21, "Computer Science", 3.7),
                new Student(5, "Eve Adams", 23, "Mathematics", 3.5)
            };

            Console.WriteLine("All Students:");
            students.ForEach(s => Console.WriteLine($"  {s}"));

            // Dictionary grouping by major
            Dictionary<string, List<Student>> studentsByMajor = new Dictionary<string, List<Student>>();
            foreach (Student student in students)
            {
                if (!studentsByMajor.ContainsKey(student.Major))
                    studentsByMajor[student.Major] = new List<Student>();
                
                studentsByMajor[student.Major].Add(student);
            }

            Console.WriteLine("\nStudents grouped by major:");
            foreach (var major in studentsByMajor)
            {
                Console.WriteLine($"  {major.Key}:");
                major.Value.ForEach(s => Console.WriteLine($"    {s}"));
            }

            // Finding students with high GPA
            List<Student> highPerformers = new List<Student>();
            foreach (Student student in students)
            {
                if (student.GPA >= 3.7)
                    highPerformers.Add(student);
            }

            Console.WriteLine("\nHigh performers (GPA >= 3.7):");
            highPerformers.ForEach(s => Console.WriteLine($"  {s}"));

            // Sorting students by GPA
            students.Sort((s1, s2) => s2.GPA.CompareTo(s1.GPA)); // Descending order
            Console.WriteLine("\nStudents sorted by GPA (descending):");
            students.ForEach(s => Console.WriteLine($"  {s}"));
        }
    }
}
