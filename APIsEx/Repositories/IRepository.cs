namespace APIsEx.Repositories
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        List<T> GetAll<T>()where T:class;
        Task<bool> SaveChangesAsync();
    }
}
