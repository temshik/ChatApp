using ChatApp.DAL.EF;
using ChatApp.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace ChatApp.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly ILogger _logger;
        public IMessagesRepository MessageSet { get; }
        public IRoomsRepository RoomSet { get; }
        public IUserRepository UserSet { get; }

        /// <summary>
        /// Контроллер класса для реализации патеррна UnitOfWork
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loggerFactory"></param>
        public UnitOfWork(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            MessageSet = new MessagesRepository(_context, _logger);
            RoomSet = new RoomsRepository(_context, _logger);
            UserSet = new UserRepository(_context, _logger);
        }

        /// <summary>
        /// Сохранение
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(SaveAsync)} action {ex}");
                return 0;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
