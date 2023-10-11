using Microsoft.Extensions.Logging;
using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.Repositories
{
    public class MessagesRepository : GenericRepository<Message>, IMessagesRepository
    {
        public MessagesRepository(ApplicationContext context, ILogger logger) : base(context, logger)
        {

        }

        public async Task<IEnumerable<Message>> GetMessagesByRoom(Guid roomId)
        {
            try
            {
                return await _dbSet.AsNoTracking().Where(m => m.ToRoomId == roomId)
                .Include(m => m.FromUser)
                .Include(m => m.ToRoom)
                .OrderByDescending(m => m.Timestamp)
                .Take(20)
                .AsEnumerable()
                .Reverse()
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(MessagesRepository));
                throw;
            }
        }

        public async Task<Message> GetMessagesById(Guid id)
        {
            try
            {
                return await _dbSet.AsNoTracking()
                .Include(u => u.FromUser)
                .Where(m => m.Id == id && m.FromUser.UserName == User.Identity.Name)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(MessagesRepository));
                throw;
            }
        }
    }
}
