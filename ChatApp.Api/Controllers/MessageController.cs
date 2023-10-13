using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;
using ChatApp.DAL.Entities;
using ChatApp.Requests;
using ChatApp.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, IMapper mapper, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomResponse>> Get(Guid id)
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

        [HttpPost("CreateFor/{userId}")]
        public async Task<ActionResult<MessageResponse>> Create(Guid id, [FromBody]MessageRequest viewModel, CancellationToken cancellationToken)
        {
            var msg = await _messageService.CreateAsync(id, _mapper.Map<MessageRequest, MessageDTO>(viewModel), cancellationToken);
            if (msg == null)
                return BadRequest();

            // Broadcast the message
            var createdMessage = _mapper.Map<MessageDTO, MessageResponse>(msg);
            //await _hubContext.Clients.Group(room.Name).SendAsync("newMessage", createdMessage);

            return CreatedAtAction(nameof(Get), new { id = msg.Id }, createdMessage);
        }

        [HttpDelete("Delete/{messageId}/By/{userId}")]
        public async Task<IActionResult> Delete([FromHeader] Guid messageId, [FromHeader] Guid userId, CancellationToken cancellationToken)
        {
            var message = await _messageService.DeleteAsync(messageId, userId, cancellationToken);

            if (message == false)
                return NotFound();            

            //await _hubContext.Clients.All.SendAsync("removeChatMessage", messagesResponse.Id);

            return Ok();
        }    
    }
}