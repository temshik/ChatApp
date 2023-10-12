using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ChatApp.DAL.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using ChatApp.Response;
using ChatApp.Requests;

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
        public async Task<ActionResult<RoomResponse>> Get(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return NotFound();

            var messageViewModel = _mapper.Map<Message, MessageViewModel>(message);
            return Ok(messageViewModel);
        }

        [HttpGet("Room/{roomName}")]
        public IActionResult GetMessages(string roomName)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Name == roomName);
            if (room == null)
                return BadRequest();

            var messages = _context.Messages.Where(m => m.ToRoomId == room.Id)
                .Include(m => m.FromUser)
                .Include(m => m.ToRoom)
                .OrderByDescending(m => m.Timestamp)
                .Take(20)
                .AsEnumerable()
                .Reverse()
                .ToList();

            var messagesViewModel = _mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(messages);

            return Ok(messagesViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse>> Create(MessageRequest viewModel)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var room = _context.Rooms.FirstOrDefault(r => r.Name == viewModel.Room);
            if (room == null)
                return BadRequest();

            var msg = new Message()
            {
                Content = Regex.Replace(viewModel.Content, @"<.*?>", string.Empty),
                FromUser = user,
                ToRoom = room,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            // Broadcast the message
            var createdMessage = _mapper.Map<Message, MessageViewModel>(msg);
            await _hubContext.Clients.Group(room.Name).SendAsync("newMessage", createdMessage);

            return CreatedAtAction(nameof(Get), new { id = msg.Id }, createdMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Messages
                .Include(u => u.FromUser)
                .Where(m => m.Id == id && m.FromUser.UserName == User.Identity.Name)
                .FirstOrDefaultAsync();

            if (message == null)
                return NotFound();

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("removeChatMessage", message.Id);

            return Ok();
        }    
    }
}