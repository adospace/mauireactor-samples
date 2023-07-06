using Chateo.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Services.Implementation;

internal class ChatServer : IChatServer
{
    private readonly HttpClient _httpClient;
    private readonly HubConnection _connection;

    public ChatServer(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ChatServer");
        _connection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}chat-hub")
            .Build();

        _connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await _connection.StartAsync();
        };
    }

    public async Task<UserViewModel> CreateUser(Guid id, string firstName, string lastName, string avatar)
    {
        var res = await _httpClient.PostAsJsonAsync("/users/create", new UserCreateModel(id, firstName, lastName, avatar));

        res.EnsureSuccessStatusCode();

        var allUsers = await GetAllUsers();

        return allUsers.First(_ => _.Id == id);
    }
    public async Task<UserViewModel[]> GetAllUsers()
    {
        return await _httpClient.GetFromJsonAsync<UserViewModel[]>("/users") ?? throw new InvalidOperationException();
    }

    public async Task StartListener()
    {
        if (_connection.State != HubConnectionState.Disconnected)
        {
            return;
        }

        _connection.On("MessageCreated",
            (MessageViewModel model) => MessageCreatedCallback?.Invoke(model));
        _connection.On("UserCreated",
            (UserViewModel model) => UserCreatedCallback?.Invoke(model));
        _connection.On("UserUpdated",
            (UserViewModel model) => UserUpdatedCallback?.Invoke(model));
        _connection.On("NotifyUserDeleted",
            (Guid id) => UserDeletedCallback?.Invoke(id));

        await _connection.StartAsync();
    }

    public async Task StopListener()
    {
        if (_connection.State == HubConnectionState.Disconnected)
        {
            return;
        }

        await _connection.StopAsync();
    }

    public async Task<MessageViewModel[]> GetAllMessages()
    {
        return await _httpClient.GetFromJsonAsync<MessageViewModel[]>("/messages") ?? throw new InvalidOperationException();
    }

    public Action<MessageViewModel>? MessageCreatedCallback { get; set; }

    public Action<UserViewModel>? UserCreatedCallback { get; set; }

    public Action<UserViewModel>? UserUpdatedCallback { get; set; }

    public Action<Guid>? UserDeletedCallback { get; set; }

}
