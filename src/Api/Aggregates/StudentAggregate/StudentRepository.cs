using EFCoreEncapsulation.Api.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api.Aggregates.StudentAggregate;

public class StudentRepository : Repository<Student>
{
    public StudentRepository(SchoolContext schoolContext)
        : base(schoolContext)
    {
    }

    public Student GetByIdSplitQueries(long id)
    {
        return SchoolContext.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .Include(s => s.SportsEnrollments)
            .ThenInclude(se => se.Sports)
            .AsSplitQuery()
            .SingleOrDefault(s => s.Id == id);
    }

    // This approach is identical but produces cleaner SQL queries.
    public override Student GetById(long id)
    {
        var student = SchoolContext.Students.Find(id);

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
        SchoolContext.Students.Add(student);
        SchoolContext.Students.Update(student);
        SchoolContext.Students.Attach(student);
    }
}