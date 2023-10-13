using ChatApp.DAL.EF;
using ChatApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatApp.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly ILogger _logger;
        protected ApplicationContext _context;
        internal DbSet<T> _dbSet;

        public GenericRepository(ApplicationContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<bool> Add(T item)
        {
            await _dbSet.AddAsync(item);
            return true;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(GenericRepository<T>));
                throw;
            }
        }

        public virtual T Find(Func<T, bool> predicate)
        {
            try
            {
                return _dbSet.AsNoTracking().FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(GenericRepository<T>));
                throw;
            }
        }

        public virtual async Task<T?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual bool Remove(T item)
        {
            try
            {
                _dbSet.Remove(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(GenericRepository<T>));
                throw;
            }
        }
    }
}
