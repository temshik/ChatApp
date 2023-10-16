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
        public async Task<bool> SaveAsync(CancellationToken cancellationToken)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
