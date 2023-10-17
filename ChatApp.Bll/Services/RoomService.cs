using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Hubs;
using ChatApp.Bll.Interfaces;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ChatApp.Bll.Services
{
    public class RoomService : IRoomService
    {
        private IUnitOfWork Database { get; set; }
        public readonly ILogger<RoomService> _logger;
        private IMapper _mapper { get; set; }
        private readonly IHubContext<ChatHub> _hubContext;

        public RoomService(IUnitOfWork uow, ILogger<RoomService> logger, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            Database = uow;
            _logger = logger;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public bool IsRoomExists(string name)
        {
            return Database.RoomSet.IsRoomExists(name);            
        }

        public async Task<IEnumerable<RoomDTO>?> GetAsync()
        {
            try
            {
                var rooms = await Database.RoomSet.Get();
                if (rooms == null)
                    return null;

                var roomDTOs = _mapper.Map<IEnumerable<RoomDTO>>(rooms);
                return roomDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(GetAsync)} action {ex}");
                return null;
            }
        }

        public async Task<RoomDTO?> GetAsync(Guid id)
        {
            try
            {
                var room = await Database.RoomSet.GetByIdAsync(id);
                if (room == null)
                    return null;

                var roomDTO = _mapper.Map<RoomDTO>(room);
                return roomDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(GetAsync)} action {ex}");
                return null;
            }
        }

        public async Task<RoomDTO?> EditAsync(Guid roomId, Guid userId, string roomName, CancellationToken cancellationToken)
        {
            try
            {
                var room = await Database.RoomSet.GetRoomById(roomId, userId);
                if (room == null)
                    return null;

                room.Name = roomName;

                Database.RoomSet.Update(room);
                await Database.SaveAsync(cancellationToken);

                var roomDTO = _mapper.Map<RoomDTO>(room);

                await _hubContext.Clients.All.SendAsync("updateChatRoom", roomDTO);

                return roomDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(EditAsync)} action {ex}");
                return null;
            }
        }

        public async Task<RoomDTO?> CreateAsync(Guid userId, RoomDTO roomDTO, CancellationToken cancellationToken)
        {
            try
            {
                var user = Database.UserSet.Find(u => u.Id == userId);
                if(user == null) 
                    return null;

                var room = new Room()
                {
                    Id = Guid.NewGuid(),
                    Name = roomDTO.Name,
                    AdminId = user.Id
                };

                await Database.RoomSet.AddAsync(room);
                await Database.SaveAsync(cancellationToken);

                var roomDto = _mapper.Map<Room, RoomDTO>(room);

                await _hubContext.Clients.All.SendAsync("addChatRoom", roomDto);

                return roomDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(CreateAsync)} action {ex}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid roomId, Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var room = await Database.RoomSet.GetRoomById(roomId, userId);
                if (room == null)
                    return false;

                Database.RoomSet.Remove(room);
                await Database.SaveAsync(cancellationToken);

                await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
                await _hubContext.Clients.Group(room.Name).SendAsync("onRoomDeleted");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Non correct values in the {nameof(DeleteAsync)} action {ex}");
                return false;
            }
        }
    }
}
