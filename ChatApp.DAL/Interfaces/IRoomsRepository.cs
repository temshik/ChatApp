using ChatApp.DAL.Entities;

namespace ChatApp.DAL.Interfaces
{
    public interface IRoomsRepository : IGenericRepository<Room>
    {
        Task<IEnumerable<Room>> Get();
        bool IsRoomExists(string name);
        Task<Room> GetRoomById(Guid roomId, Guid userID);
    }
}
