using EntityFrameworkDemo.Data;
using EntityFrameworkDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkDemo.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<Student?> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<List<Student>> GetStudentsByCourseAsync(int courseId);
        Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId);
        Task<bool> DropStudentFromCourseAsync(int studentId, int courseId);
        Task<decimal> CalculateStudentGPAAsync(int studentId);
    }

    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Include(s => s.Grades)
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Instructor)
                .Include(s => s.Grades)
                    .ThenInclude(g => g.Course)
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UpdateStudentAsync(Student student)
        {
            var existingStudent = await _context.Students.FindAsync(student.Id);
            if (existingStudent == null) return null;

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.GPA = student.GPA;
            existingStudent.IsActive = student.IsActive;

            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            // Soft delete
            student.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetStudentsByCourseAsync(int courseId)
        {
            return await _context.Students
                .Include(s => s.Enrollments)
                .Where(s => s.IsActive && s.Enrollments.Any(e => e.CourseId == courseId && e.Status == "Active"))
                .OrderBy(s => s.LastName)
                .ToListAsync();
        }

        public async Task<bool> EnrollStudentInCourseAsync(int studentId, int courseId)
        {
            // Check if student and course exist
            var student = await _context.Students.FindAsync(studentId);
            var course = await _context.Courses.FindAsync(courseId);
            
            if (student == null || course == null || !student.IsActive || !course.IsActive)
                return false;

            // Check if already enrolled
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (existingEnrollment != null) return false;

            // Check course capacity
            var currentEnrollmentCount = await _context.Enrollments
                .CountAsync(e => e.CourseId == courseId && e.Status == "Active");

            if (currentEnrollmentCount >= course.MaxStudents) return false;

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                Status = "Active"
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DropStudentFromCourseAsync(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId && e.Status == "Active");

            if (enrollment == null) return false;

            enrollment.Status = "Dropped";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateStudentGPAAsync(int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentId == studentId)
                .GroupBy(g => g.CourseId)
                .Select(g => g.Average(grade => grade.Percentage))
                .ToListAsync();

            if (!grades.Any()) return 0;

            var averagePercentage = grades.Average();
            
            // Convert percentage to GPA (4.0 scale)
            return averagePercentage switch
            {
                >= 97 => 4.0m,
                >= 93 => 4.0m,
                >= 90 => 3.7m,
                >= 87 => 3.3m,
                >= 83 => 3.0m,
                >= 80 => 2.7m,
                >= 77 => 2.3m,
                >= 73 => 2.0m,
                >= 70 => 1.7m,
                >= 67 => 1.3m,
                >= 65 => 1.0m,
                >= 60 => 0.7m,
                _ => 0.0m
            };
        }
    }

    public interface ICourseService
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<Course> CreateCourseAsync(Course course);
        Task<Course?> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int id);
        Task<List<Course>> GetCoursesByDepartmentAsync(string department);
        Task<List<Course>> GetCoursesByInstructorAsync(int instructorId);
    }

    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;

        public CourseService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Department)
                .ThenBy(c => c.CourseCode)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student)
                .Include(c => c.Grades)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course?> UpdateCourseAsync(Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(course.Id);
            if (existingCourse == null) return null;

            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            existingCourse.Credits = course.Credits;
            existingCourse.Department = course.Department;
            existingCourse.CourseCode = course.CourseCode;
            existingCourse.MaxStudents = course.MaxStudents;
            existingCourse.InstructorId = course.InstructorId;
            existingCourse.IsActive = course.IsActive;

            await _context.SaveChangesAsync();
            return existingCourse;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            course.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Course>> GetCoursesByDepartmentAsync(string department)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.IsActive && c.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.CourseCode)
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByInstructorAsync(int instructorId)
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .Where(c => c.IsActive && c.InstructorId == instructorId)
                .OrderBy(c => c.CourseCode)
                .ToListAsync();
        }
    }
}
