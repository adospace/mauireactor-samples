using Chateo.Server;
using Chateo.Server.Models;
using Chateo.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSignalR()
    .AddJsonProtocol();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/users", () =>
    User.All);

app.MapPost("/users/create", (UserCreateModel createModel)=>
    User.All.Add(new User
    {
        Id = createModel.Id,
        FirstName = createModel.FirstName, 
        LastName = createModel.LastName 
    }));

app.MapPost("/users/update", (UserUpdatedModel updateModel) =>
{
    var user = User.All.First(_ => _.Id == updateModel.Id);
    
});

app.MapDelete("/users", (Guid id) => User.All.Remove(User.All.First(_ => _.Id == id)));

app.MapGet("/messages", (Guid fromUserId, Guid ToUserId)=> Message.All.Where(_=>_.ToUserId == fromUserId && _.ToUserId == ToUserId));

app.MapPost("/message/create", (MessageCreateModel createModel) =>
{
    Message.All.Add(new Message
    {
        Id = createModel.Id,
        Content = createModel.Content,
        FromUserId = createModel.FromUserId,
        ToUserId = createModel.ToUserId,
        TimeStamp = DateTime.Now,
    });
});

app.MapHub<ChatHub>("/chat-hub");

app.Run();

