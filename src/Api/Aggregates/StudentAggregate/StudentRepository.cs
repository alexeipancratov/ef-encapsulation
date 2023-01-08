using EFCoreEncapsulation.Api.Abstract;
using EFCoreEncapsulation.Api.Aggregates.StudentAggregate.Dto;
using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api.Aggregates.StudentAggregate;

public class StudentRepository : Repository<Student>
{
    public StudentRepository(SchoolContext schoolContext)
        : base(schoolContext)
    {
    }

    public IReadOnlyList<Student> GetAll(string emailDomain)
    {
        IQueryable<Student> students = SchoolContext.Set<Student>();

        if (!string.IsNullOrWhiteSpace(emailDomain))
        {
            students = students.Where(s => s.Email.EndsWith(emailDomain));
        }

        return students.ToList();
    }

    //public Student GetByIdSplitQueries(long id)
    //{
    //    return SchoolContext.Students
    //        .Include(s => s.Enrollments)
    //        .ThenInclude(e => e.Course)
    //        .Include(s => s.SportsEnrollments)
    //        .ThenInclude(se => se.Sports)
    //        .AsSplitQuery()
    //        .SingleOrDefault(s => s.Id == id);
    //}

    // NOTE: A separate repository could be introduced for Read operations. CQRS doesn't specify the exact implementation.
    // The main thing here is that we avoided the Read/Write model mismatch.
    public StudentDto GetDto(long id)
    {
        var student = SchoolContext.Set<Student>().Find(id);

        var enrollments = SchoolContext.Set<EnrollmentData>()
            .FromSqlInterpolated($@"
                SELECT e.StudentID, e.Grade, c.Name Course
                FROM dbo.Enrollment e
                INNER JOIN dbo.Course c ON e.CourseID = c.CourseID
                WHERE e.StudentID = {id}")
            .ToList();

        return new StudentDto
        {
            StudentId = id,
            Name = student.Name,
            Email = student.Email,
            Enrollments = enrollments.Select(e => new EnrollmentDto
            {
                Course = e.Course,
                Grade = ((Grade)e.Grade).ToString()
            }).ToList()
        };
    }

    // This approach is identical but produces cleaner SQL queries.
    // NOTE: When retrieving aggregates, retrieve all related data as well (to the point of aggregate boundaries).
    // NOTE: If you care about performance too much, you can also add some params to retrieve related collections if needed.
    public override Student GetById(long id)
    {
        var student = SchoolContext.Set<Student>().Find(id);

        if (student == null)
        {
            return null;
        }

        SchoolContext.Entry(student).Collection(s => s.Enrollments).Load();
        SchoolContext.Entry(student).Collection(s => s.SportsEnrollments).Load();

        return student;
    }

    // Could be used for register, enrolling student in a course, to update a student
    public override void Save(Student student)
    {
        SchoolContext.Set<Student>().Add(student);
        SchoolContext.Set<Student>().Update(student);
        SchoolContext.Set<Student>().Attach(student);
    }
}