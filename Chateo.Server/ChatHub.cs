using Chateo.Server.Models;
using Chateo.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Chateo.Server;


public class ChatHub : Hub
{
    private readonly List<User> _users = new ();
    

    public async Task RegisterUser(UserCreateModel createModel)
    {
        _users.Add(new User
        {
            Id = createModel.Id,
            FirstName = createModel.FirstName,
            LastName = createModel.LastName
        });

        var caller = Clients.Caller;

        

        await Clients.All.SendAsync("NewUser", new UserUpdatedModel(createModel.Id, createModel.FirstName, createModel.LastName));

    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
