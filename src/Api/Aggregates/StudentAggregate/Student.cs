using EFCoreEncapsulation.Api.Aggregates.CourseAggregate;

namespace EFCoreEncapsulation.Api.Aggregates.StudentAggregate;

// Aggregate root
public class Student
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<SportsEnrollment> SportsEnrollments { get; set; }

    public string EnrollIn(Course course, Grade grade)
    {
        // This is why it's important to always load the entire aggregate from DB with all of its related data.
        if (Enrollments.Any(e => e.CourseId == course.Id))
        {
            return $"Student has been already enrolled in course with ID {course.Id}";
        }
        
        Enrollments.Add(new Enrollment
        {
            Student = this,
            CourseId = course.Id,
            Grade = grade
        });

        return "OK";
    }
}
