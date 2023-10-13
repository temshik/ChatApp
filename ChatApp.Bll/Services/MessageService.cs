using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using System.Text.RegularExpressions;

namespace ChatApp.Bll.Services
{
    public class MessageService : IMessageService
    {
        private IUnitOfWork Database { get; set; }
        private IMapper _mapper { get; set; }
        public MessageService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }
        
        public async Task<MessageDTO> GetAsync(Guid id)
        {        
           var message = await Database.MessageSet.GetById(id);

           var messageDTO = _mapper.Map<MessageDTO>(message);
            return messageDTO;
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync(string roomName)
        {
            var room = Database.RoomSet.Find(r => r.Name == roomName);

            var messages = Database.MessageSet.GetMessagesByRoom(room.Id);

            var messagesDTOs = _mapper.Map<IEnumerable<MessageDTO>>(messages);
            
            return messagesDTOs;
        }

        public async Task<MessageDTO> CreateAsync(Guid userId,MessageDTO messageDTO, CancellationToken cancellationToken)
        {
            var user = Database.UserSet.Find(u => u.Id == userId);
            var room = Database.RoomSet.Find(r => r.Name == messageDTO.Room);
            if (room == null)
                return null;

            var msg = new Message()
            {
                Content = Regex.Replace(messageDTO.Content, @"<.*?>", string.Empty),
                FromUser = user,
                ToRoom = room,
                Timestamp = DateTime.Now
            };

            await Database.MessageSet.Add(msg);
            await Database.SaveAsync(cancellationToken);

            var messageDto = _mapper.Map<MessageDTO>(msg);
            return messageDto;
        }

        public async Task<bool> DeleteAsync(Guid messageId, Guid userId, CancellationToken cancellationToken)
        {
             var message = await Database.MessageSet.GetMessagesById(messageId, userId);

            var result = Database.MessageSet.Remove(message);

            await Database.SaveAsync(cancellationToken);
            return result;
        }
    }
}
