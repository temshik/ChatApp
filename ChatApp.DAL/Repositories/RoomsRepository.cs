using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatApp.DAL.Repositories
{
    public class RoomsRepository : GenericRepository<Room>, IRoomsRepository
    {
        public RoomsRepository(ApplicationContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<Room>> Get()
        {
            try
            {
                return await _dbSet.AsNoTracking()
                .Include(r => r.Admin)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(RoomsRepository));
                throw;
            }
        }

        public bool IsRoomExists(string name)
        {
            try
            {
                return _dbSet.AsNoTracking().Any(r => r.Name == name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(RoomsRepository));
                throw;
            }
        }

        public async Task<Room> GetRoomById(Guid id)
        {
            try
            {
                return await _dbSet.AsNoTracking()
                .Include(u => u.Admin)
                .Where(m => m.Id == id && m.Admin.UserName == User.Identity.Name)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(RoomsRepository));
                throw;
            }
        }
    }
}
