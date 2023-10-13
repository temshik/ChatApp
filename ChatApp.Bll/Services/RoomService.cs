using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;

namespace ChatApp.Bll.Services
{
    public class RoomService : IRoomService
    {
        private IUnitOfWork Database { get; set; }
        private IMapper _mapper { get; set; }
        public RoomService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
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

            await Database.SaveAsync(cancellationToken);

            var roomDTO = _mapper.Map<RoomDTO>(room);
            return roomDTO;
        }

        public async Task<RoomDTO> CreateAsync(Guid userId, RoomDTO roomDTO, CancellationToken cancellationToken)
        {            
            var user = Database.UserSet.Find(u => u.Id == userId);
            var room = new Room()
            {
                Name = roomDTO.Name,
                Admin = user
            };

            await Database.RoomSet.Add(room);
            await Database.SaveAsync(cancellationToken);

            var roomDto = _mapper.Map<Room, RoomDTO>(room);

            return roomDto;
        }

        public async Task<bool> DeleteAsync(Guid roomId, Guid userId, CancellationToken cancellationToken)
        {
            var room = await Database.RoomSet.GetRoomById(roomId, userId);

            var result = Database.RoomSet.Remove(room);

            await Database.SaveAsync(cancellationToken);

            return result;
        }
    }
}
