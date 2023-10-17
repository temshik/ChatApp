using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace ChatApp.Bll.Hubs
{
    public class ChatHub : Hub
    {
        public readonly static List<UserDTO> _Connections = new List<UserDTO>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        private IUnitOfWork Database { get; set; }
        private readonly IMapper _mapper;

        public ChatHub(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper;
        }

        public async Task SendPrivate(string receiverName, string message, string sederName)
        {
            if (_ConnectionsMap.TryGetValue(receiverName, out string userId))
            {
                // Who is the sender;
                var sender = _Connections.Where(u => u.UserName == sederName).First();

                if (!string.IsNullOrEmpty(message.Trim()))
                {
                    // Build the message
                    var messageViewModel = new MessageDTO()
                    {
                        Content = Regex.Replace(message, @"<.*?>", string.Empty),
                        FromUserName = sender.UserName,
                        FromFullName = sender.FullName,
                        Avatar = sender.Avatar,
                        Room = "",
                        Timestamp = DateTime.Now
                    };

                    // Send the message
                    await Clients.Client(userId).SendAsync("newMessage", messageViewModel);
                    await Clients.Caller.SendAsync("newMessage", messageViewModel);
                }
            }
        }

        public async Task Join(string roomName, string userName)
        {
            try
            {
                var user = _Connections.Where(u => u.UserName == userName).FirstOrDefault();
                if (user != null && user.CurrentRoom != roomName)
                {
                    // Remove user from others list
                    if (!string.IsNullOrEmpty(user.CurrentRoom))
                        await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                    // Join to new chat room
                    await Leave(user.CurrentRoom);
                    await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    user.CurrentRoom = roomName;

                    // Tell others to update their list of users
                    await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
            }
        }

        public async Task Leave(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public IEnumerable<UserDTO> GetUsers(string roomName)
        {
            return _Connections.Where(u => u.CurrentRoom == roomName).ToList();
        }

        public async Task ConnectAsync(string userName)
        {
            try
            {                
                var user = Database.UserSet.Find(u => u.Name == userName);

                var userViewModel = _mapper.Map<ApplicationUser, UserDTO>(user);

                userViewModel.CurrentRoom = "";

                if (!_Connections.Any(u => u.UserName == userName))
                {
                    _Connections.Add(userViewModel);
                    _ConnectionsMap.Add(userName, Context.ConnectionId);
                }

                await Clients.Caller.SendAsync("getProfileInfo", userViewModel);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
            }
        }

        public async Task DisconnectAsync(string userName)
        {
            try
            {
                var user = _Connections.Where(u => u.UserName == userName).First();
                _Connections.Remove(user);

                // Tell other users to remove you from their list
                await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                // Remove mapping
                _ConnectionsMap.Remove(user.UserName);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
            }
        }
    }
}
