using EFCoreEncapsulation.Api.Abstract;

namespace EFCoreEncapsulation.Api.Aggregates.CourseAggregate
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(SchoolContext schoolContext)
            : base(schoolContext)
        {
        }
    }
}