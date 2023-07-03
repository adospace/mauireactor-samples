using Chateo.Shared;
using System;
using System.Threading.Tasks;

namespace Chateo.Services;

public interface IChatServer
{
    Action<MessageCreateModel>? MessageCreatedCallback { get; set; }
    Action<UserCreateModel>? UserCreatedCallback { get; set; }
    Action<Guid>? UserDeletedCallback { get; set; }
    Action<UserUpdatedModel>? UserUpdatedCallback { get; set; }

    Task<UserViewModel> CreateUser(Guid id, string firstName, string lastName);
    Task StartListener();
    Task StopListener();
}