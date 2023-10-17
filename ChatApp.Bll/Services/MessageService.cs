using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Hubs;
using ChatApp.Bll.Interfaces;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ChatApp.Bll.Services
{
    public class MessageService : IMessageService
    {
        private IUnitOfWork Database { get; set; }
        public readonly ILogger<MessageService> _logger;
        private IMapper _mapper { get; set; }
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageService(IUnitOfWork uow, ILogger<MessageService> logger, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            Database = uow;
            _logger = logger;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<MessageDTO?> GetAsync(Guid id)
        {
            try
            {
                var message = await Database.MessageSet.GetByIdAsync(id);
                if (message == null)
                    return null;

                var messageDTO = _mapper.Map<MessageDTO>(message);
                return messageDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(GetAsync)} action {ex}");
                return null;
            }
        }

        public async Task<IEnumerable<MessageDTO>?> GetMessagesAsync(string roomName)
        {
            try
            {
                var room = Database.RoomSet.Find(r => r.Name == roomName);
                if (room == null)
                    return null;

                var messages = await Database.MessageSet.GetMessagesByRoom(room.Id);
                if (messages == null)
                    return null;

                var messagesDTOs = _mapper.Map<IEnumerable<MessageDTO>>(messages);

                return messagesDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(GetMessagesAsync)} action {ex}");
                return null;
            }
        }

        public async Task<MessageDTO?> CreateAsync(Guid userId, MessageDTO messageDTO, CancellationToken cancellationToken)
        {
            try
            {
                var user = Database.UserSet.Find(u => u.Id == userId);
                if (user == null)                
                    return null;

                var room = Database.RoomSet.Find(r => r.Name == messageDTO.Room);
                if (room == null)
                    return null;

                var msg = new Message()
                {
                    Content = Regex.Replace(messageDTO.Content, @"<.*?>", string.Empty),
                    FromUserId = user.Id,
                    ToRoomId = room.Id,
                    Timestamp = DateTime.Now
                };

                await Database.MessageSet.AddAsync(msg);
                
                await Database.SaveAsync(cancellationToken);

                var messageDto = _mapper.Map<MessageDTO>(msg);

                await _hubContext.Clients.Group(room.Name).SendAsync("newMessage", messageDto);

                return messageDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(CreateAsync)} action {ex}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid messageId, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var message = await Database.MessageSet.GetMessagesById(messageId, userId);

                if (message == null)
                    return false;

                Database.MessageSet.Remove(message);

                if (await Database.SaveAsync(cancellationToken) > 0)
                    await _hubContext.Clients.All.SendAsync("removeChatMessage", message.Id);
                else return false;

                return true;
            }
            catch (Exception ex) {
                _logger.LogError($"Non correct values in the {nameof(DeleteAsync)} action {ex}");
                return false;
            }
        }
    }
}
