using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;
using ChatApp.Requests;
using ChatApp.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageService messageService, IMapper mapper, ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<RoomResponse>> Get([FromRoute] Guid id)
        {
            var message = await _messageService.GetAsync(id);
            if (message == null)
                return NotFound();

            var messageResponse = _mapper.Map<MessageDTO, MessageResponse>(message);
            return Ok(messageResponse);
        }

        [HttpGet("Room/{roomName}")]
        public async Task<IActionResult> GetMessages(string roomName)
        {
            var messages = await _messageService.GetMessagesAsync(roomName);
            if (messages == null)
                return BadRequest();

            var messagesResponse = _mapper.Map<IEnumerable<MessageDTO>, IEnumerable<MessageResponse>>(messages);

            return Ok(messagesResponse);
        }

        [HttpPost("CreateFor/{userId:Guid}")]
        public async Task<ActionResult<MessageResponse>> Create([FromRoute] Guid userId, [FromBody] MessageRequest viewModel, CancellationToken cancellationToken)
        {
            var msg = await _messageService.CreateAsync(userId, _mapper.Map<MessageRequest, MessageDTO>(viewModel), cancellationToken);
            if (msg == null)
                return BadRequest();

            // Broadcast the message
            var createdMessage = _mapper.Map<MessageDTO, MessageResponse>(msg);

            return CreatedAtAction(nameof(Get), new { id = msg.Id }, createdMessage);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromHeader] Guid messageId, [FromHeader] Guid userId, CancellationToken cancellationToken)
        {
            var message = await _messageService.DeleteAsync(messageId, userId, cancellationToken);

            if (message == false)
                return NotFound();

            return Ok();
        }
    }
}