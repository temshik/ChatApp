using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;
using ChatApp.Requests;
using ChatApp.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
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

        [HttpGet("Get/{roomId}")]
        public async Task<ActionResult<RoomResponse>> GetById(Guid roomId)
        {
            var room = await _roomService.GetAsync(roomId);
            if (room == null)
                return NotFound();

            var roomViewModel = _mapper.Map<RoomDTO, RoomResponse>(room);
            return Ok(roomViewModel);
        }

        [HttpPost("CreateBy")]
        public async Task<ActionResult<RoomResponse>> Create([FromHeader] Guid userId, RoomRequest viewModel, CancellationToken cancellationToken)
        {
            if (_roomService.IsRoomExists(viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var room = await _roomService.CreateAsync(userId, _mapper.Map<RoomRequest, RoomDTO>(viewModel), cancellationToken);
            if (room == null)
                return BadRequest();

            var createdRoom = _mapper.Map<RoomDTO, RoomResponse>(room);

            return CreatedAtAction(nameof(Get), new { id = room.Id }, createdRoom);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Edit([FromHeader] Guid roomId, [FromHeader] Guid userId, RoomRequest viewModel, CancellationToken cancellationToken)
        {
            if (_roomService.IsRoomExists(viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var room = await _roomService.EditAsync(roomId, userId, viewModel.Name, cancellationToken);
            if (room == null)
                return NotFound();

            var updatedRoom = _mapper.Map<RoomDTO, RoomResponse>(room);

            return CreatedAtAction(nameof(Get), new { id = room.Id }, updatedRoom);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromHeader] Guid roomId, [FromHeader] Guid userId, CancellationToken cancellationToken)
        {
            var room = await _roomService.DeleteAsync(roomId, userId, cancellationToken);

            if (room == false)
                return NotFound();

            return Ok();
        }
    }
}
