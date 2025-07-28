using Microsoft.EntityFrameworkCore;
using EntityFrameworkDemo.Models;

namespace EntityFrameworkDemo.Data
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=school.db");
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Student configuration
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.GPA).HasPrecision(3, 2);
            });

            // Course configuration
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CourseCode).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.CourseCode).IsUnique();
                entity.Property(e => e.Department).IsRequired().HasMaxLength(50);
                
                // One-to-many relationship with Instructor
                entity.HasOne(c => c.Instructor)
                      .WithMany(i => i.Courses)
                      .HasForeignKey(c => c.InstructorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Instructor configuration
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Department).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Salary).HasPrecision(10, 2);
            });

            // Enrollment configuration
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                
                // Many-to-many relationship through Enrollment
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Composite unique index to prevent duplicate enrollments
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
            });

            // Grade configuration
            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AssignmentName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Points).HasPrecision(5, 2);
                entity.Property(e => e.MaxPoints).HasPrecision(5, 2);

                entity.HasOne(g => g.Student)
                      .WithMany(s => s.Grades)
                      .HasForeignKey(g => g.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(g => g.Course)
                      .WithMany(c => c.Grades)
                      .HasForeignKey(g => g.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Instructors
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, FirstName = "Dr. Sarah", LastName = "Johnson", Email = "s.johnson@university.edu", Department = "Computer Science", HireDate = new DateTime(2015, 8, 15), Salary = 85000, Office = "CS-201" },
                new Instructor { Id = 2, FirstName = "Prof. Michael", LastName = "Chen", Email = "m.chen@university.edu", Department = "Mathematics", HireDate = new DateTime(2012, 1, 10), Salary = 78000, Office = "MATH-305" },
                new Instructor { Id = 3, FirstName = "Dr. Emily", LastName = "Davis", Email = "e.davis@university.edu", Department = "Physics", HireDate = new DateTime(2018, 3, 20), Salary = 82000, Office = "PHY-102" }
            );

            // Seed Courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "Introduction to Programming", CourseCode = "CS101", Description = "Learn basic programming concepts using C#", Credits = 3, Department = "Computer Science", MaxStudents = 30, InstructorId = 1 },
                new Course { Id = 2, Title = "Data Structures", CourseCode = "CS201", Description = "Advanced data structures and algorithms", Credits = 4, Department = "Computer Science", MaxStudents = 25, InstructorId = 1 },
                new Course { Id = 3, Title = "Calculus I", CourseCode = "MATH101", Description = "Differential and integral calculus", Credits = 4, Department = "Mathematics", MaxStudents = 40, InstructorId = 2 },
                new Course { Id = 4, Title = "Linear Algebra", CourseCode = "MATH201", Description = "Vectors, matrices, and linear transformations", Credits = 3, Department = "Mathematics", MaxStudents = 35, InstructorId = 2 },
                new Course { Id = 5, Title = "Physics I", CourseCode = "PHY101", Description = "Mechanics and thermodynamics", Credits = 4, Department = "Physics", MaxStudents = 30, InstructorId = 3 }
            );

            // Seed Students
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@student.edu", DateOfBirth = new DateTime(2002, 5, 15), EnrollmentDate = new DateTime(2023, 9, 1), GPA = 3.8m },
                new Student { Id = 2, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@student.edu", DateOfBirth = new DateTime(2001, 8, 22), EnrollmentDate = new DateTime(2023, 9, 1), GPA = 3.6m },
                new Student { Id = 3, FirstName = "Charlie", LastName = "Brown", Email = "charlie.brown@student.edu", DateOfBirth = new DateTime(2003, 2, 10), EnrollmentDate = new DateTime(2023, 9, 1), GPA = 3.9m },
                new Student { Id = 4, FirstName = "Diana", LastName = "Wilson", Email = "diana.wilson@student.edu", DateOfBirth = new DateTime(2002, 11, 30), EnrollmentDate = new DateTime(2023, 9, 1), GPA = 3.7m },
                new Student { Id = 5, FirstName = "Eve", LastName = "Davis", Email = "eve.davis@student.edu", DateOfBirth = new DateTime(2001, 7, 18), EnrollmentDate = new DateTime(2023, 9, 1), GPA = 3.5m }
            );

            // Seed Enrollments
            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { Id = 1, StudentId = 1, CourseId = 1, EnrollmentDate = new DateTime(2023, 9, 5), Status = "Active" },
                new Enrollment { Id = 2, StudentId = 1, CourseId = 3, EnrollmentDate = new DateTime(2023, 9, 5), Status = "Active" },
                new Enrollment { Id = 3, StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2023, 9, 6), Status = "Active" },
                new Enrollment { Id = 4, StudentId = 2, CourseId = 4, EnrollmentDate = new DateTime(2023, 9, 6), Status = "Active" },
                new Enrollment { Id = 5, StudentId = 3, CourseId = 2, EnrollmentDate = new DateTime(2023, 9, 7), Status = "Active" },
                new Enrollment { Id = 6, StudentId = 3, CourseId = 3, EnrollmentDate = new DateTime(2023, 9, 7), Status = "Active" },
                new Enrollment { Id = 7, StudentId = 4, CourseId = 1, EnrollmentDate = new DateTime(2023, 9, 8), Status = "Active" },
                new Enrollment { Id = 8, StudentId = 4, CourseId = 5, EnrollmentDate = new DateTime(2023, 9, 8), Status = "Active" },
                new Enrollment { Id = 9, StudentId = 5, CourseId = 2, EnrollmentDate = new DateTime(2023, 9, 9), Status = "Active" },
                new Enrollment { Id = 10, StudentId = 5, CourseId = 4, EnrollmentDate = new DateTime(2023, 9, 9), Status = "Active" }
            );

            // Seed Grades
            modelBuilder.Entity<Grade>().HasData(
                new Grade { Id = 1, StudentId = 1, CourseId = 1, AssignmentName = "Assignment 1", Points = 85, MaxPoints = 100, DateGraded = new DateTime(2023, 10, 15) },
                new Grade { Id = 2, StudentId = 1, CourseId = 1, AssignmentName = "Midterm Exam", Points = 92, MaxPoints = 100, DateGraded = new DateTime(2023, 11, 10) },
                new Grade { Id = 3, StudentId = 2, CourseId = 1, AssignmentName = "Assignment 1", Points = 78, MaxPoints = 100, DateGraded = new DateTime(2023, 10, 15) },
                new Grade { Id = 4, StudentId = 2, CourseId = 1, AssignmentName = "Midterm Exam", Points = 88, MaxPoints = 100, DateGraded = new DateTime(2023, 11, 10) },
                new Grade { Id = 5, StudentId = 3, CourseId = 2, AssignmentName = "Programming Project", Points = 95, MaxPoints = 100, DateGraded = new DateTime(2023, 10, 20) },
                new Grade { Id = 6, StudentId = 4, CourseId = 1, AssignmentName = "Assignment 1", Points = 82, MaxPoints = 100, DateGraded = new DateTime(2023, 10, 15) }
            );
        }
    }
}
