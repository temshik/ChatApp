using ChatApp.Bll.DTOs;

namespace ChatApp.Bll.Interfaces
{
    public interface IRoomService
    {
        bool IsRoomExists(string name);
        Task<IEnumerable<RoomDTO>> GetAsync();
        Task<RoomDTO> GetAsync(Guid id);
        Task<RoomDTO> EditAsync(Guid id, string roomName, CancellationToken cancellationToken);
        Task<RoomDTO> CreateAsync(RoomDTO roomDTO, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
