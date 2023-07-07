using Chateo.Shared;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chateo.Services;

public interface IChatServer
{
    Task<MessageViewModel[]> GetAllMessages();

    Task<UserViewModel[]> GetAllUsers();

    Task<UserViewModel> CreateUser(Guid id, string firstName, string lastName, string avatar);

    Task<MessageViewModel> CreateMessage(Guid id, Guid FromUserId, Guid ToUserId, string Content);

    Task<MessageViewModel> UpdateMessage(Guid id);

    Task StartListener();

    Task StopListener();

    Action<MessageViewModel>? MessageCreatedCallback { get; set; }

    Action<UserViewModel>? UserCreatedCallback { get; set; }

    Action<Guid>? UserDeletedCallback { get; set; }

    Action<UserViewModel>? UserUpdatedCallback { get; set; }

    public Action<MessageViewModel>? MessageUpdatedCallback { get; set; }

}