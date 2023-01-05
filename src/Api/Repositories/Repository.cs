namespace EFCoreEncapsulation.Api.Repositories
{
    public abstract class Repository<T> where T : class
    {
        protected readonly SchoolContext _schoolContext;

        public Repository(SchoolContext schoolContext)
        {
            _schoolContext = schoolContext;
        }

        public virtual T GetById(long id)
        {
            return _schoolContext.Set<T>().Find(id);
        }

        public virtual void Save(T entity)
        {
            _schoolContext.Set<T>().Add(entity);
        }
    }
}