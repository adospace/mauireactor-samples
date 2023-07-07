using Chateo.Server;
using Chateo.Server.Models;
using Chateo.Shared;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSignalR()
    .AddJsonProtocol();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/users", () =>
    User.All.Select(_ => new UserViewModel(_.Id, _.FirstName, _.LastName, _.Avatar, _.LastSeen)));

app.MapPost("/users/create", (IHubContext<ChatHub> chatHub, UserCreateModel createModel) =>
{
    var now = DateTime.Now;
    User.All.Add(new User
    {
        Id = createModel.Id,
        FirstName = createModel.FirstName,
        LastName = createModel.LastName,
        Avatar = createModel.Avatar,
        LastSeen = now,
    });

    var userCreated = new UserViewModel(createModel.Id, createModel.FirstName, createModel.LastName, createModel.Avatar, now);
    
    chatHub.Clients.All.SendAsync("UserCreated", userCreated);

    return userCreated;
});

app.MapPost("/users/update", (IHubContext<ChatHub> chatHub, UserUpdatedModel updateModel) =>
{
    var now = DateTime.Now;
    var user = User.All.First(_ => _.Id == updateModel.Id);
    user.TypingToUserId = updateModel.TypingToUserId;
    user.LastSeen = now;

    var userUpdated = new UserViewModel(user.Id, user.FirstName, user.LastName, user.Avatar, user.LastSeen);

    chatHub.Clients.All.SendAsync("UserUpdated", userUpdated);

    return userUpdated;
});

app.MapDelete("/users", (IHubContext<ChatHub> chatHub, Guid id) =>
{
    User.All.Remove(User.All.First(_ => _.Id == id));

    chatHub.Clients.All.SendAsync("UserDeleted", id);
});

app.MapGet("/messages", () => Message.All.Select(_=> new MessageViewModel(_.Id, _.FromUserId, _.ToUserId, _.Content, _.TimeStamp, _.ReadTimeStamp)));

app.MapPost("/messages/create", (IHubContext<ChatHub> chatHub, MessageCreateModel createModel) =>
{
    var now = DateTime.Now;
    Message.All.Add(new Message
    {
        Id = createModel.Id,
        Content = createModel.Content,
        FromUserId = createModel.FromUserId,
        ToUserId = createModel.ToUserId,
        TimeStamp = now,
    });

    var newMessage = new MessageViewModel(createModel.Id, createModel.FromUserId, createModel.ToUserId, createModel.Content, now, null);

    chatHub.Clients.All.SendAsync("MessageCreated", newMessage);

    return newMessage;
});

app.MapPost("/messages/update", (IHubContext<ChatHub> chatHub, MessageUpdateModel updateModel) =>
{
    var now = DateTime.Now;
    var message = Message.All.First(_ => _.Id == updateModel.Id);

    message.ReadTimeStamp = DateTime.Now;

    var updatedMessage = new MessageViewModel(message.Id, message.FromUserId, message.ToUserId, message.Content, message.TimeStamp, message.ReadTimeStamp);

    chatHub.Clients.All.SendAsync("MessageUpdated", updatedMessage);

    return updatedMessage;
});

app.MapHub<ChatHub>("/chat-hub");

app.Run();

