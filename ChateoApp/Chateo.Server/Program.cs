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

    chatHub.Clients.All.SendAsync("UserCreated", new UserViewModel(createModel.Id, createModel.FirstName, createModel.LastName, createModel.Avatar, now));
});

app.MapPost("/users/update", (IHubContext<ChatHub> chatHub, UserUpdatedModel updateModel) =>
{
    var now = DateTime.Now;
    var user = User.All.First(_ => _.Id == updateModel.Id);
    user.TypingToUserId = updateModel.TypingToUserId;
    user.LastSeen = now;

    chatHub.Clients.All.SendAsync("UserUpdated", new UserViewModel(user.Id, user.FirstName, user.LastName, user.Avatar, user.LastSeen));
});

app.MapDelete("/users", (IHubContext<ChatHub> chatHub, Guid id) =>
{
    User.All.Remove(User.All.First(_ => _.Id == id));

    chatHub.Clients.All.SendAsync("UserDeleted", id);
});

app.MapGet("/messages", () => Message.All.Select(_=> new MessageViewModel(_.Id, _.FromUserId, _.ToUserId, _.Content, _.TimeStamp)));

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

    chatHub.Clients.All.SendAsync("MessageCreated", new MessageViewModel(createModel.Id, createModel.FromUserId, createModel.ToUserId, createModel.Content, now));
});

app.MapHub<ChatHub>("/chat-hub");

app.Run();

