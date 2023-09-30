using Chateo.Shared;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chateo.Services;

public interface IChatServer
{
    Task StartListener();

    Task StopListener();
    
    UserViewModel[] Users { get; }

    MessageViewModel[] Messages { get; }

    Task<UserViewModel> CreateUser(Guid id, string firstName, string lastName, string avatar);

    Task<MessageViewModel> CreateMessage(Guid id, Guid FromUserId, Guid ToUserId, string Content);

    Task<MessageViewModel> UpdateMessage(Guid id);

    event EventHandler<MessageViewModel>? MessageCreated;

    event EventHandler<UserViewModel>? UserCreated;

    event EventHandler<Guid>? UserDeleted;

    event EventHandler<UserViewModel>? UserUpdated;

    event EventHandler<MessageViewModel>? MessageUpdated;

}