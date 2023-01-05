namespace EFCoreEncapsulation.Api.Abstract
{
    public abstract class Repository<T> where T : class
    {
        protected readonly SchoolContext SchoolContext;

        protected Repository(SchoolContext schoolContext)
        {
            SchoolContext = schoolContext;
        }

        public virtual T GetById(long id)
        {
            return SchoolContext.Set<T>().Find(id);
        }

        public virtual void Save(T entity)
        {
            SchoolContext.Set<T>().Add(entity);
        }
    }
}