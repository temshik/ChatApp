using ChatApp.Bll.DTOs;

namespace ChatApp.Bll.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDTO> GetAsync(Guid id);
        Task<IEnumerable<MessageDTO>> GetMessagesAsync(string roomName);
        Task<MessageDTO> CreateAsync(MessageDTO messageDTO, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
