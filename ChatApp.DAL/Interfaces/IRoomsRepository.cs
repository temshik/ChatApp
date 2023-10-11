using ChatApp.DAL.Entities;

namespace ChatApp.DAL.Interfaces
{
    public interface IRoomsRepository
    {
        Task<IEnumerable<Room>> Get();
        bool IsRoomExists(string name);
        Task<Message> GetRoomById(Guid id)
    }
}
