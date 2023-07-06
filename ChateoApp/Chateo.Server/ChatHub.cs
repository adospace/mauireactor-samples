using Chateo.Server.Models;
using Chateo.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Chateo.Server;


public class ChatHub : Hub
{
    //public static ChatHub? Instance { get; private set; }

    public ChatHub()
    {
        //Instance = this;
    }

    //public async Task NotifyNewMessage(Shared.MessageViewModel model)
    //{
    //    await Clients.All.SendAsync("MessageCreated", model);
    //}

    //public async Task NotifyNewUser(UserViewModel model)
    //{
    //    await Clients.All.SendAsync("UserCreated", model);
    //}

    //public async Task NotifyUserUpdated(UserViewModel model)
    //{
    //    await Clients.All.SendAsync("UserUpdated", model);
    //}

    //public async Task NotifyUserDeleted(Guid id)
    //{
    //    await Clients.All.SendAsync("UserDeleted", id);
    //}
}
