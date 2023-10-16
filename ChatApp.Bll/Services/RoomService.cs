using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Hubs;
using ChatApp.Bll.Interfaces;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Bll.Services
{
    public class RoomService : IRoomService
    {
        private IUnitOfWork Database { get; set; }
        private IMapper _mapper { get; set; }
        private readonly IHubContext<ChatHub> _hubContext;
        public RoomService(IUnitOfWork uow, IMapper mapper, IHubContext<ChatHub> hubContext)
        {
            Database = uow;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public bool IsRoomExists(string name)
        {
            return Database.RoomSet.IsRoomExists(name);
        }

        public async Task<IEnumerable<RoomDTO>> GetAsync()
        {
            var rooms = await Database.RoomSet.Get();

            var roomDTOs = _mapper.Map<IEnumerable<RoomDTO>>(rooms);
            return roomDTOs;
        }

        public async Task<RoomDTO> GetAsync(Guid id)
        {
            var room = await Database.RoomSet.GetById(id);

            var roomDTO = _mapper.Map<RoomDTO>(room);
            return roomDTO;
        }

        public async Task<RoomDTO> EditAsync(Guid roomId, Guid userId, string roomName, CancellationToken cancellationToken)
        {
            var room = await Database.RoomSet.GetRoomById(roomId, userId);
            room.Name = roomName;

            if (Database.RoomSet.Update(room))
                await Database.SaveAsync(cancellationToken);

            var roomDTO = _mapper.Map<RoomDTO>(room);

            await _hubContext.Clients.All.SendAsync("updateChatRoom", roomDTO);

            return roomDTO;
        }

        public async Task<RoomDTO> CreateAsync(Guid userId, RoomDTO roomDTO, CancellationToken cancellationToken)
        {
            var user = Database.UserSet.Find(u => u.Id == userId);
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

        public async Task<bool> DeleteAsync(Guid roomId, Guid userId, CancellationToken cancellationToken)
        {
            var room = await Database.RoomSet.GetRoomById(roomId, userId);

            if (Database.RoomSet.Remove(room))
                await Database.SaveAsync(cancellationToken);

            await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
            await _hubContext.Clients.Group(room.Name).SendAsync("onRoomDeleted");

            return true;
        }
    }
}
