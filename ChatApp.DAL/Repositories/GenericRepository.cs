using ChatApp.DAL.EF;
using ChatApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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
        public virtual async Task AddAsync(T item)
        {
            try
            {
                await _dbSet.AddAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(AddAsync)} action {ex}");
            }
        }

        public virtual async Task<IEnumerable<T>?> All()
        {
            try
            {
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(GenericRepository<T>));
                return null;
            }
        }

        public virtual T? Find(Func<T, bool> predicate)
        {
            try
            {
                return _dbSet.AsNoTracking().FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(GenericRepository<T>));
                return null;
            }
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetByIdAsync)} action {ex}");
                return null;
            }
        }

        public virtual void Remove(T item)
        {
            try
            {
                _dbSet.Remove(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(GenericRepository<T>));
            }
        }

        public virtual void Update(T item)
        {
            try
            {
                _dbSet.Update(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(GenericRepository<T>));
            }
        }
    }
}
