using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api;

public class StudentRepository
{
    private readonly SchoolContext _schoolContext;
    
    public StudentRepository(SchoolContext schoolContext)
    {
        _schoolContext = schoolContext;
    }

    public Student GetByIdSplitQueries(long id)
    {
        return _schoolContext.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .Include(s => s.SportsEnrollments)
            .ThenInclude(se => se.Sports)
            .AsSplitQuery()
            .SingleOrDefault(s => s.Id == id);
    }

    // This approach is identical but produces cleaner SQL queries.
    public Student GetById(long id)
    {
        var student = _schoolContext.Students.Find(id);

        if (student == null)
        {
            return null;
        }

        _schoolContext.Entry(student).Collection(s => s.Enrollments).Load();
        _schoolContext.Entry(student).Collection(s => s.SportsEnrollments).Load();

        return student;
    }
}