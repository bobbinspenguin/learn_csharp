using EntityFrameworkDemo.Data;
using EntityFrameworkDemo.Models;
using EntityFrameworkDemo.Services;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Entity Framework Core Demo ===\n");

            // Initialize database and services
            using var context = new SchoolContext();
            await InitializeDatabaseAsync(context);

            var studentService = new StudentService(context);
            var courseService = new CourseService(context);

            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("Choose a demonstration:");
                Console.WriteLine("1. View All Students");
                Console.WriteLine("2. View Student Details");
                Console.WriteLine("3. Add New Student");
                Console.WriteLine("4. Update Student");
                Console.WriteLine("5. View All Courses");
                Console.WriteLine("6. View Course Details");
                Console.WriteLine("7. Enroll Student in Course");
                Console.WriteLine("8. Drop Student from Course");
                Console.WriteLine("9. Add Grade");
                Console.WriteLine("10. Generate Reports");
                Console.WriteLine("11. Advanced Queries Demo");
                Console.WriteLine("12. Performance Optimization Demo");
                Console.WriteLine("13. Exit");
                Console.Write("\nEnter your choice (1-13): ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ViewAllStudentsAsync(studentService);
                            break;
                        case "2":
                            await ViewStudentDetailsAsync(studentService);
                            break;
                        case "3":
                            await AddNewStudentAsync(studentService);
                            break;
                        case "4":
                            await UpdateStudentAsync(studentService);
                            break;
                        case "5":
                            await ViewAllCoursesAsync(courseService);
                            break;
                        case "6":
                            await ViewCourseDetailsAsync(courseService);
                            break;
                        case "7":
                            await EnrollStudentInCourseAsync(studentService);
                            break;
                        case "8":
                            await DropStudentFromCourseAsync(studentService);
                            break;
                        case "9":
                            await AddGradeAsync(context);
                            break;
                        case "10":
                            await GenerateReportsAsync(context);
                            break;
                        case "11":
                            await AdvancedQueriesDemo(context);
                            break;
                        case "12":
                            await PerformanceOptimizationDemo(context);
                            break;
                        case "13":
                            continueProgram = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                if (continueProgram)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static async Task InitializeDatabaseAsync(SchoolContext context)
        {
            Console.WriteLine("Initializing database...");
            
            // Ensure database is created and migrations are applied
            await context.Database.EnsureCreatedAsync();
            
            Console.WriteLine("Database initialized successfully.\n");
        }

        static async Task ViewAllStudentsAsync(IStudentService studentService)
        {
            Console.WriteLine("--- All Students ---");
            
            var students = await studentService.GetAllStudentsAsync();
            
            if (!students.Any())
            {
                Console.WriteLine("No students found.");
                return;
            }

            Console.WriteLine($"{"ID",-5} {"Name",-25} {"Email",-30} {"GPA",-5} {"Courses",-10}");
            Console.WriteLine(new string('-', 75));

            foreach (var student in students)
            {
                var courseCount = student.Enrollments.Count(e => e.Status == "Active");
                Console.WriteLine($"{student.Id,-5} {student.FullName,-25} {student.Email,-30} {student.GPA,-5:F2} {courseCount,-10}");
            }

            Console.WriteLine($"\nTotal students: {students.Count}");
        }

        static async Task ViewStudentDetailsAsync(IStudentService studentService)
        {
            Console.WriteLine("--- Student Details ---");
            Console.Write("Enter student ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }

            var student = await studentService.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"\nStudent Information:");
            Console.WriteLine($"  ID: {student.Id}");
            Console.WriteLine($"  Name: {student.FullName}");
            Console.WriteLine($"  Email: {student.Email}");
            Console.WriteLine($"  Age: {student.Age}");
            Console.WriteLine($"  Enrollment Date: {student.EnrollmentDate:yyyy-MM-dd}");
            Console.WriteLine($"  GPA: {student.GPA:F2}");

            var activeEnrollments = student.Enrollments.Where(e => e.Status == "Active").ToList();
            if (activeEnrollments.Any())
            {
                Console.WriteLine($"\nEnrolled Courses ({activeEnrollments.Count}):");
                foreach (var enrollment in activeEnrollments)
                {
                    Console.WriteLine($"  • {enrollment.Course.CourseCode}: {enrollment.Course.Title}");
                    Console.WriteLine($"    Instructor: {enrollment.Course.Instructor.FullName}");
                    Console.WriteLine($"    Credits: {enrollment.Course.Credits}");
                }
            }

            if (student.Grades.Any())
            {
                Console.WriteLine($"\nRecent Grades:");
                var recentGrades = student.Grades.OrderByDescending(g => g.DateGraded).Take(5);
                foreach (var grade in recentGrades)
                {
                    Console.WriteLine($"  • {grade.Course.CourseCode} - {grade.AssignmentName}: {grade.Points}/{grade.MaxPoints} ({grade.LetterGrade})");
                }
            }
        }

        static async Task AddNewStudentAsync(IStudentService studentService)
        {
            Console.WriteLine("--- Add New Student ---");
            
            Console.Write("First Name: ");
            string firstName = Console.ReadLine() ?? "";
            
            Console.Write("Last Name: ");
            string lastName = Console.ReadLine() ?? "";
            
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";
            
            Console.Write("Date of Birth (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth,
                EnrollmentDate = DateTime.Now,
                GPA = 0.0m
            };

            try
            {
                var createdStudent = await studentService.CreateStudentAsync(student);
                Console.WriteLine($"Student created successfully with ID: {createdStudent.Id}");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error creating student: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        static async Task UpdateStudentAsync(IStudentService studentService)
        {
            Console.WriteLine("--- Update Student ---");
            Console.Write("Enter student ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }

            var student = await studentService.GetStudentByIdAsync(studentId);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Current information for {student.FullName}:");
            Console.WriteLine($"  Email: {student.Email}");
            Console.WriteLine($"  GPA: {student.GPA:F2}");

            Console.Write($"New Email (current: {student.Email}): ");
            string newEmail = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(newEmail))
                student.Email = newEmail;

            Console.Write($"New GPA (current: {student.GPA:F2}): ");
            string gpaInput = Console.ReadLine() ?? "";
            if (decimal.TryParse(gpaInput, out decimal newGPA))
                student.GPA = newGPA;

            try
            {
                await studentService.UpdateStudentAsync(student);
                Console.WriteLine("Student updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error updating student: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        static async Task ViewAllCoursesAsync(ICourseService courseService)
        {
            Console.WriteLine("--- All Courses ---");
            
            var courses = await courseService.GetAllCoursesAsync();
            
            if (!courses.Any())
            {
                Console.WriteLine("No courses found.");
                return;
            }

            var groupedCourses = courses.GroupBy(c => c.Department);

            foreach (var department in groupedCourses.OrderBy(g => g.Key))
            {
                Console.WriteLine($"\n{department.Key} Department:");
                Console.WriteLine($"{"Code",-10} {"Title",-30} {"Credits",-8} {"Enrolled",-10} {"Instructor",-20}");
                Console.WriteLine(new string('-', 78));

                foreach (var course in department.OrderBy(c => c.CourseCode))
                {
                    var enrolledCount = course.Enrollments.Count(e => e.Status == "Active");
                    Console.WriteLine($"{course.CourseCode,-10} {course.Title,-30} {course.Credits,-8} {enrolledCount}/{course.MaxStudents,-4} {course.Instructor.FullName,-20}");
                }
            }
        }

        static async Task ViewCourseDetailsAsync(ICourseService courseService)
        {
            Console.WriteLine("--- Course Details ---");
            Console.Write("Enter course ID: ");
            
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            var course = await courseService.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.WriteLine($"\nCourse Information:");
            Console.WriteLine($"  Code: {course.CourseCode}");
            Console.WriteLine($"  Title: {course.Title}");
            Console.WriteLine($"  Description: {course.Description}");
            Console.WriteLine($"  Credits: {course.Credits}");
            Console.WriteLine($"  Department: {course.Department}");
            Console.WriteLine($"  Instructor: {course.Instructor.FullName}");
            Console.WriteLine($"  Max Students: {course.MaxStudents}");

            var activeEnrollments = course.Enrollments.Where(e => e.Status == "Active").ToList();
            Console.WriteLine($"  Currently Enrolled: {activeEnrollments.Count}");

            if (activeEnrollments.Any())
            {
                Console.WriteLine($"\nEnrolled Students:");
                foreach (var enrollment in activeEnrollments.OrderBy(e => e.Student.LastName))
                {
                    Console.WriteLine($"  • {enrollment.Student.FullName} ({enrollment.Student.Email})");
                }
            }
        }

        static async Task EnrollStudentInCourseAsync(IStudentService studentService)
        {
            Console.WriteLine("--- Enroll Student in Course ---");
            
            Console.Write("Enter student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }

            Console.Write("Enter course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            bool success = await studentService.EnrollStudentInCourseAsync(studentId, courseId);
            
            if (success)
            {
                Console.WriteLine("Student enrolled successfully.");
            }
            else
            {
                Console.WriteLine("Failed to enroll student. Check if student/course exists, course is full, or student is already enrolled.");
            }
        }

        static async Task DropStudentFromCourseAsync(IStudentService studentService)
        {
            Console.WriteLine("--- Drop Student from Course ---");
            
            Console.Write("Enter student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }

            Console.Write("Enter course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            bool success = await studentService.DropStudentFromCourseAsync(studentId, courseId);
            
            if (success)
            {
                Console.WriteLine("Student dropped successfully.");
            }
            else
            {
                Console.WriteLine("Failed to drop student. Check if the enrollment exists.");
            }
        }

        static async Task AddGradeAsync(SchoolContext context)
        {
            Console.WriteLine("--- Add Grade ---");
            
            Console.Write("Enter student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }

            Console.Write("Enter course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            // Verify enrollment
            var enrollment = await context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId && e.Status == "Active");
            
            if (enrollment == null)
            {
                Console.WriteLine("Student is not enrolled in this course.");
                return;
            }

            Console.Write("Assignment Name: ");
            string assignmentName = Console.ReadLine() ?? "";

            Console.Write("Points Earned: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal points))
            {
                Console.WriteLine("Invalid points value.");
                return;
            }

            Console.Write("Max Points: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal maxPoints))
            {
                Console.WriteLine("Invalid max points value.");
                return;
            }

            var grade = new Grade
            {
                StudentId = studentId,
                CourseId = courseId,
                AssignmentName = assignmentName,
                Points = points,
                MaxPoints = maxPoints,
                DateGraded = DateTime.Now
            };

            context.Grades.Add(grade);
            await context.SaveChangesAsync();

            Console.WriteLine($"Grade added successfully. Letter Grade: {grade.LetterGrade} ({grade.Percentage:F1}%)");
        }

        static async Task GenerateReportsAsync(SchoolContext context)
        {
            Console.WriteLine("--- Generate Reports ---");
            Console.WriteLine("1. Department Summary");
            Console.WriteLine("2. Student Performance Report");
            Console.WriteLine("3. Course Enrollment Report");
            Console.Write("Choose report type (1-3): ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    await GenerateDepartmentSummaryAsync(context);
                    break;
                case "2":
                    await GenerateStudentPerformanceReportAsync(context);
                    break;
                case "3":
                    await GenerateCourseEnrollmentReportAsync(context);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static async Task GenerateDepartmentSummaryAsync(SchoolContext context)
        {
            Console.WriteLine("\n--- Department Summary Report ---");

            var departmentStats = await context.Courses
                .Include(c => c.Enrollments)
                .Where(c => c.IsActive)
                .GroupBy(c => c.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    CourseCount = g.Count(),
                    TotalEnrollments = g.Sum(c => c.Enrollments.Count(e => e.Status == "Active")),
                    AverageEnrollment = g.Average(c => c.Enrollments.Count(e => e.Status == "Active")),
                    TotalCredits = g.Sum(c => c.Credits)
                })
                .OrderBy(d => d.Department)
                .ToListAsync();

            Console.WriteLine($"{"Department",-20} {"Courses",-8} {"Students",-10} {"Avg/Course",-10} {"Credits",-8}");
            Console.WriteLine(new string('-', 66));

            foreach (var dept in departmentStats)
            {
                Console.WriteLine($"{dept.Department,-20} {dept.CourseCount,-8} {dept.TotalEnrollments,-10} {dept.AverageEnrollment,-10:F1} {dept.TotalCredits,-8}");
            }
        }

        static async Task GenerateStudentPerformanceReportAsync(SchoolContext context)
        {
            Console.WriteLine("\n--- Student Performance Report ---");

            var studentPerformance = await context.Students
                .Include(s => s.Grades)
                .Include(s => s.Enrollments)
                .Where(s => s.IsActive)
                .Select(s => new
                {
                    s.Id,
                    s.FullName,
                    s.GPA,
                    CourseCount = s.Enrollments.Count(e => e.Status == "Active"),
                    GradeCount = s.Grades.Count(),
                    AverageGrade = s.Grades.Any() ? s.Grades.Average(g => g.Percentage) : 0
                })
                .OrderByDescending(s => s.GPA)
                .ToListAsync();

            Console.WriteLine($"{"Name",-25} {"GPA",-5} {"Courses",-8} {"Grades",-8} {"Avg%",-6}");
            Console.WriteLine(new string('-', 52));

            foreach (var student in studentPerformance.Take(10))
            {
                Console.WriteLine($"{student.FullName,-25} {student.GPA,-5:F2} {student.CourseCount,-8} {student.GradeCount,-8} {student.AverageGrade,-6:F1}");
            }
        }

        static async Task GenerateCourseEnrollmentReportAsync(SchoolContext context)
        {
            Console.WriteLine("\n--- Course Enrollment Report ---");

            var courseEnrollments = await context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.Instructor)
                .Where(c => c.IsActive)
                .Select(c => new
                {
                    c.CourseCode,
                    c.Title,
                    c.MaxStudents,
                    CurrentEnrollment = c.Enrollments.Count(e => e.Status == "Active"),
                    InstructorName = c.Instructor.FullName,
                    UtilizationRate = c.MaxStudents > 0 ? (double)c.Enrollments.Count(e => e.Status == "Active") / c.MaxStudents * 100 : 0
                })
                .OrderByDescending(c => c.UtilizationRate)
                .ToListAsync();

            Console.WriteLine($"{"Code",-10} {"Title",-30} {"Enrolled",-10} {"Capacity",-10} {"Util%",-6}");
            Console.WriteLine(new string('-', 66));

            foreach (var course in courseEnrollments)
            {
                Console.WriteLine($"{course.CourseCode,-10} {course.Title,-30} {course.CurrentEnrollment,-10} {course.MaxStudents,-10} {course.UtilizationRate,-6:F1}");
            }
        }

        static async Task AdvancedQueriesDemo(SchoolContext context)
        {
            Console.WriteLine("--- Advanced Queries Demo ---");

            // 1. Complex JOIN with aggregation
            Console.WriteLine("\n1. Students with their course performance:");
            var studentCoursePerformance = await context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Include(s => s.Grades)
                .Where(s => s.IsActive)
                .Select(s => new
                {
                    StudentName = s.FullName,
                    Courses = s.Enrollments
                        .Where(e => e.Status == "Active")
                        .Select(e => new
                        {
                            CourseCode = e.Course.CourseCode,
                            CourseTitle = e.Course.Title,
                            AverageGrade = s.Grades
                                .Where(g => g.CourseId == e.CourseId)
                                .Average(g => (double?)g.Percentage) ?? 0
                        })
                })
                .Take(3)
                .ToListAsync();

            foreach (var student in studentCoursePerformance)
            {
                Console.WriteLine($"\n{student.StudentName}:");
                foreach (var course in student.Courses)
                {
                    Console.WriteLine($"  • {course.CourseCode}: {course.CourseTitle} - Avg: {course.AverageGrade:F1}%");
                }
            }

            // 2. Raw SQL query
            Console.WriteLine("\n\n2. Department with most students (Raw SQL):");
            var departmentEnrollments = await context.Database
                .SqlQueryRaw<DepartmentEnrollmentResult>(@"
                    SELECT 
                        c.Department,
                        COUNT(DISTINCT e.StudentId) as StudentCount
                    FROM Courses c
                    INNER JOIN Enrollments e ON c.Id = e.CourseId
                    WHERE e.Status = 'Active' AND c.IsActive = 1
                    GROUP BY c.Department
                    ORDER BY StudentCount DESC")
                .ToListAsync();

            foreach (var dept in departmentEnrollments)
            {
                Console.WriteLine($"  {dept.Department}: {dept.StudentCount} students");
            }

            // 3. Window function equivalent using LINQ
            Console.WriteLine("\n3. Top performer in each department:");
            var topPerformers = await context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Where(s => s.IsActive && s.Enrollments.Any(e => e.Status == "Active"))
                .GroupBy(s => s.Enrollments.First(e => e.Status == "Active").Course.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    TopStudent = g.OrderByDescending(s => s.GPA).First()
                })
                .ToListAsync();

            foreach (var dept in topPerformers)
            {
                Console.WriteLine($"  {dept.Department}: {dept.TopStudent.FullName} (GPA: {dept.TopStudent.GPA:F2})");
            }
        }

        static async Task PerformanceOptimizationDemo(SchoolContext context)
        {
            Console.WriteLine("--- Performance Optimization Demo ---");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // 1. N+1 Query Problem (BAD)
            Console.WriteLine("\n1. N+1 Query Problem (inefficient):");
            stopwatch.Restart();
            
            var studentsWithoutInclude = await context.Students.Where(s => s.IsActive).ToListAsync();
            foreach (var student in studentsWithoutInclude.Take(3))
            {
                // This will cause additional queries for each student
                var enrollmentCount = await context.Enrollments
                    .CountAsync(e => e.StudentId == student.Id && e.Status == "Active");
                Console.WriteLine($"  {student.FullName}: {enrollmentCount} courses");
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Time with N+1 queries: {stopwatch.ElapsedMilliseconds}ms");

            // 2. Optimized with Include (GOOD)
            Console.WriteLine("\n2. Optimized with Include:");
            stopwatch.Restart();
            
            var studentsWithInclude = await context.Students
                .Include(s => s.Enrollments.Where(e => e.Status == "Active"))
                .Where(s => s.IsActive)
                .Take(3)
                .ToListAsync();
            
            foreach (var student in studentsWithInclude)
            {
                Console.WriteLine($"  {student.FullName}: {student.Enrollments.Count} courses");
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Time with Include: {stopwatch.ElapsedMilliseconds}ms");

            // 3. Projection for better performance
            Console.WriteLine("\n3. Using projection for large datasets:");
            stopwatch.Restart();
            
            var studentSummaries = await context.Students
                .Where(s => s.IsActive)
                .Select(s => new
                {
                    s.Id,
                    s.FullName,
                    s.GPA,
                    EnrollmentCount = s.Enrollments.Count(e => e.Status == "Active")
                })
                .Take(5)
                .ToListAsync();
            
            foreach (var summary in studentSummaries)
            {
                Console.WriteLine($"  {summary.FullName}: GPA {summary.GPA:F2}, {summary.EnrollmentCount} courses");
            }
            
            stopwatch.Stop();
            Console.WriteLine($"Time with projection: {stopwatch.ElapsedMilliseconds}ms");

            // 4. AsNoTracking for read-only scenarios
            Console.WriteLine("\n4. Using AsNoTracking for read-only data:");
            stopwatch.Restart();
            
            var readOnlyStudents = await context.Students
                .AsNoTracking()
                .Where(s => s.IsActive)
                .Take(5)
                .ToListAsync();
            
            stopwatch.Stop();
            Console.WriteLine($"AsNoTracking time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Retrieved {readOnlyStudents.Count} students (read-only)");

            Console.WriteLine("\nPerformance Tips:");
            Console.WriteLine("• Use Include() to avoid N+1 queries");
            Console.WriteLine("• Use projections (Select) to fetch only needed data");
            Console.WriteLine("• Use AsNoTracking() for read-only scenarios");
            Console.WriteLine("• Consider pagination for large datasets");
            Console.WriteLine("• Use compiled queries for frequently executed queries");
        }
    }

    // Helper class for raw SQL results
    public class DepartmentEnrollmentResult
    {
        public string Department { get; set; } = string.Empty;
        public int StudentCount { get; set; }
    }
}
