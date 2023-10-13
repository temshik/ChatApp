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

        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<RoomResponse>>> Get()
        {
            var rooms = await _roomService.GetAsync();

            var roomsViewModel = _mapper.Map<IEnumerable<RoomDTO>, IEnumerable<RoomResponse>>(rooms);

            return Ok(roomsViewModel);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<RoomResponse>> Get([FromHeader] Guid id)
        {
            var room = await _roomService.GetAsync(id);
            if (room == null)
                return NotFound();

            var roomViewModel = _mapper.Map<RoomDTO, RoomResponse>(room);
            return Ok(roomViewModel);
        }

        [HttpPost("CreateFor/{userId}")]
        public async Task<ActionResult<RoomResponse>> Create([FromHeader] Guid userId, RoomRequest viewModel, CancellationToken cancellationToken)
        {
            if (_roomService.IsRoomExists(viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var room = await _roomService.CreateAsync(userId, _mapper.Map<RoomRequest, RoomDTO>(viewModel), cancellationToken);
            if (room == null)
                return BadRequest();

            var createdRoom = _mapper.Map<RoomDTO, RoomResponse>(room);
            //await _hubContext.Clients.All.SendAsync("addChatRoom", createdRoom);

            return CreatedAtAction(nameof(Get), new { id = room.Id }, createdRoom);
        }

        [HttpPut("Edit/{roomId}/{userId}")]
        public async Task<IActionResult> Edit([FromHeader] Guid roomId, [FromHeader] Guid userId, RoomRequest viewModel, CancellationToken cancellationToken)
        {
            if (_roomService.IsRoomExists(viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var room = await _roomService.EditAsync(roomId, userId, viewModel.Name, cancellationToken);
            if (room == null)
                return NotFound();

            var updatedRoom = _mapper.Map<RoomDTO, RoomResponse>(room);
            //await _hubContext.Clients.All.SendAsync("updateChatRoom", updatedRoom);

            return CreatedAtAction(nameof(Get), new { id = room.Id }, updatedRoom);
        }

        [HttpDelete("Delete/{roomId}/By/{userId}")]
        public async Task<IActionResult> Delete([FromHeader] Guid roomId, [FromHeader] Guid userId, CancellationToken cancellationToken)
        {
            var room = await _roomService.DeleteAsync(roomId, userId, cancellationToken);

            if (room == false)
                return NotFound();

            //await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
            //await _hubContext.Clients.Group(room.Name).SendAsync("onRoomDeleted");

            return Ok();
        }
    }
}
