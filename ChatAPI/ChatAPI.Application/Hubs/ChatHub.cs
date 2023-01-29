using AutoMapper;
using ChatAPI.Application.Contracts;
using ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage;
using ChatAPI.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Security.Claims;

namespace ChatAPI.Application.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private readonly IMessageReposiory _messageReposiory;
        private static Dictionary<Guid, UserData> _users = new Dictionary<Guid, UserData>();

        public ChatHub(IMessageReposiory messageReposiory)
        {
            _messageReposiory = messageReposiory;
        }

        public override Task OnConnectedAsync()
        {
            Guid userId = Guid.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = new UserData()
            {
                ConnectedAt = DateTime.Now,
                ConnectionId = Context.ConnectionId
            };
            _users.TryAdd(userId, user);
            
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(SendMessageDto.Request request)
        {
            if (_users.TryGetValue(request.ReceiverId, out UserData user))
            {
                var message = await _messageReposiory.SendMessageAsync(request);
                await Clients.Client(user.ConnectionId).SendAsync("ReceiveMessage", message);
            }
        }
        //public async Task JoinGroup(string groupName)
        //{
        //    Guid userId = Guid.Parse(Context.User.FindFirstValue(ClaimTypes.NameIdentifier));
        //    UserData user;

        //    if (_users.TryGetValue(userId, out user))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //        user.GroupName = groupName;
        //        _users[userId] = user;
        //    }
        //}

        //public async Task JoinGroup(string groupName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //}

        //public async Task SendMessageToGroup(string groupName, SendMessageDto.Request request)
        //{
        //    var message = _messageReposiory.SendMessage(request);
        //    await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        //}

    }




    //public async Task JoinGroup(string gName, string user)
    //{
    //    //await Groups.AddToGroupAsync(Context.ConnectionId, user);
    //    Clients.OthersInGroup(gName).newMember(user, gName);
    //}
    //public async Task SendToGroup(string gName, string username, string message)
    //{
    //    await Clients.Group(gName).SendAsync(username, gName, message);
    //}
}
