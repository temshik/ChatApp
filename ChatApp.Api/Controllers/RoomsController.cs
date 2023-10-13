using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;
using ChatApp.Requests;
using ChatApp.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomsController> _logger;

        public RoomsController(IRoomService roomService, IMapper mapper, ILogger<RoomsController> logger)
        {
            _roomService = roomService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomResponse>>> Get()
        {
            var rooms = await _roomService.GetAsync();

            var roomsViewModel = _mapper.Map<IEnumerable<RoomDTO>, IEnumerable<RoomResponse>>(rooms);

            return Ok(roomsViewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomResponse>> Get(Guid id)
        {
            var room = await _roomService.GetAsync(id);
            if (room == null)
                return NotFound();

            var roomViewModel = _mapper.Map<RoomDTO, RoomResponse>(room);
            return Ok(roomViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<RoomResponse>> Create(RoomRequest viewModel, CancellationToken cancellationToken)
        {
            if (_roomService.IsRoomExists(viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var room = await _roomService.CreateAsync(_mapper.Map<RoomRequest, RoomDTO>(viewModel), cancellationToken);
            if (room == null)
                return BadRequest();

            var createdRoom = _mapper.Map<RoomDTO, RoomResponse>(room);
            //await _hubContext.Clients.All.SendAsync("addChatRoom", createdRoom);

            return CreatedAtAction(nameof(Get), new { id = room.Id }, createdRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, RoomRequest viewModel, CancellationToken cancellationToken)
        {
            if (_roomService.IsRoomExists(viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var room = await _roomService.EditAsync(id, viewModel.Name, cancellationToken);
            if (room == null)
                return NotFound();

            var updatedRoom = _mapper.Map<RoomDTO, RoomResponse>(room);
            //await _hubContext.Clients.All.SendAsync("updateChatRoom", updatedRoom);

            return CreatedAtAction(nameof(Get), new { id = room.Id }, updatedRoom);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var room = await _roomService.DeleteAsync(id, cancellationToken);

            if (room == false)
                return NotFound();

            //await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
            //await _hubContext.Clients.Group(room.Name).SendAsync("onRoomDeleted");

            return Ok();
        }
    }
}
