using Chateo.Shared;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Chateo.Services;

public interface IChatServer
{
    Action<MessageViewModel>? MessageCreatedCallback { get; set; }

    Action<UserViewModel>? UserCreatedCallback { get; set; }

    Action<Guid>? UserDeletedCallback { get; set; }

    Action<UserViewModel>? UserUpdatedCallback { get; set; }
    
    Task<MessageViewModel[]> GetAllMessages();

    Task<UserViewModel[]> GetAllUsers();

    Task<UserViewModel> CreateUser(Guid id, string firstName, string lastName, string avatar);

    Task StartListener();

    Task StopListener();
}