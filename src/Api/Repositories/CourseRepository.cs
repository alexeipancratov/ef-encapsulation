namespace EFCoreEncapsulation.Api.Repositories
{
    public class CourseRepository : Repository<Course>
    {
        public CourseRepository(SchoolContext schoolContext)
            : base(schoolContext)
        {
        }
    }
}