namespace ChatApp.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>?> All();
        Task<T?> GetByIdAsync(Guid id);
        T? Find(Func<T, Boolean> predicate);
        Task AddAsync(T item);
        void Remove(T item);
        void Update(T item);
    }
}
