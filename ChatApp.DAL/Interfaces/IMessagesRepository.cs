using ChatApp.DAL.Entities;

namespace ChatApp.DAL.Interfaces
{
    public interface IMessagesRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesByRoom(Guid roomId);
        Task<Message?> GetMessagesById(Guid messageId, Guid userId);
    }
}
