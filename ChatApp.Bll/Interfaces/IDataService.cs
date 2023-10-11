using ChatApp.Bll.DTOs;
using ChatApp.DAL.Entities;

namespace ChatApp.Bll.Interfaces
{
    public interface IDataService
    {
        Task<List<News>> SeedDateOnliner(JsonConfigDTO configDTO, DateTime dateTimeStart, DateTime dateTimeEnd, CancellationToken cancellationToken);
        Task<List<News>> SeedDateBelta(JsonConfigDTO configDTO, DateTime dateTimeStart, DateTime dateTimeEnd, CancellationToken cancellationToken);
        IEnumerable<News> GetNewsByDate(DateTime dateTimeStart, DateTime dateTimeEnd, CancellationToken cancellationToken);
        Task<List<News>> SeedNews(JsonConfigDTO configDTO, string link, CancellationToken cancellationToken);

        void Dispose();
    }
}
