using ChatApp.Bll.DTOs;
using ChatApp.Bll.Extensions;
using ChatApp.Bll.Interfaces;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;

namespace ChatApp.Bll.Services
{
    public class MessageService : IMessageService
    {
        private IUnitOfWork Database { get; set; }
        public MessageService(IUnitOfWork uow)
        {
            Database = uow;
        }

        /// <summary>
        /// Метод сервиса для декомпозиции кода
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        public async Task<List<News>> SeedDateOnliner(JsonConfigDTO configDTO, DateTime dateTimeStart, DateTime dateTimeEnd, CancellationToken cancellationToken)
        {


        }

        /// <summary>
        /// Метод сервиса для декомпозиции кода
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        public async Task<List<News>> SeedDateBelta(JsonConfigDTO configDTO, DateTime dateTimeStart, DateTime dateTimeEnd, CancellationToken cancellationToken)
        {

        }

        public IEnumerable<News> GetNewsByDate(DateTime dateTimeStart, DateTime dateTimeEnd, CancellationToken cancellationToken)
        {

        }

        /// <summary>
        /// Метод сервиса для декомпозиции кода
        /// </summary>
        /// <returns></returns>
        public async Task<List<News>> SeedNews(JsonConfigDTO configDTO, string link, CancellationToken cancellationToken)
        {

        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
