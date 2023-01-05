using Microsoft.EntityFrameworkCore;

namespace EFCoreEncapsulation.Api;

public class StudentRepository
{
    private readonly SchoolContext _schoolContext;
    
    public StudentRepository(SchoolContext schoolContext)
    {
        _schoolContext = schoolContext;
    }

    public Student GetById(long id)
    {
        return _schoolContext.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .Include(s => s.SportsEnrollments)
            .ThenInclude(se => se.Sports)
            //.AsSplitQuery()  -- test without it first
            .SingleOrDefault(s => s.Id == id);
    }
}