using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkDemo.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public decimal GPA { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public List<Enrollment> Enrollments { get; set; } = new();
        public List<Grade> Grades { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
    }

    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Department { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int MaxStudents { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public List<Enrollment> Enrollments { get; set; } = new();
        public List<Grade> Grades { get; set; } = new();
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;
    }

    public class Instructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public string? Office { get; set; }

        // Navigation properties
        public List<Course> Courses { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
        public int YearsOfService => DateTime.Now.Year - HireDate.Year;
    }

    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Completed, Dropped

        // Navigation properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string AssignmentName { get; set; } = string.Empty;
        public decimal Points { get; set; }
        public decimal MaxPoints { get; set; }
        public DateTime DateGraded { get; set; }
        public string? Comments { get; set; }

        // Navigation properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;

        public decimal Percentage => MaxPoints > 0 ? (Points / MaxPoints) * 100 : 0;
        public string LetterGrade => Percentage switch
        {
            >= 97 => "A+",
            >= 93 => "A",
            >= 90 => "A-",
            >= 87 => "B+",
            >= 83 => "B",
            >= 80 => "B-",
            >= 77 => "C+",
            >= 73 => "C",
            >= 70 => "C-",
            >= 67 => "D+",
            >= 63 => "D",
            >= 60 => "D-",
            _ => "F"
        };
    }
}
