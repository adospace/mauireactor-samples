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
            .WithUrl($"{_httpClient.BaseAddress}/ChatHub")
            .Build();

        _connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await _connection.StartAsync();
        };
    }

    public async Task<UserViewModel> CreateUser(Guid id, string firstName, string lastName)
    {
        var res = await _httpClient.PostAsJsonAsync("/users/create", new UserCreateModel(id, firstName, lastName));

        res.EnsureSuccessStatusCode();

        return new UserViewModel(id, firstName, lastName);
    }

    public async Task StartListener()
    {
        _connection.On("MessageCreated",
            (MessageCreateModel model) => MessageCreatedCallback?.Invoke(model));
        _connection.On("UserCreated",
            (UserCreateModel model) => UserCreatedCallback?.Invoke(model));
        _connection.On("UserUpdated",
            (UserUpdatedModel model) => UserUpdatedCallback?.Invoke(model));
        _connection.On("NotifyUserDeleted",
            (Guid id) => UserDeletedCallback?.Invoke(id));

        await _connection.StartAsync();
    }

    public async Task StopListener()
    {
        await _connection.StopAsync();
    }

    public Action<MessageCreateModel>? MessageCreatedCallback { get; set; }

    public Action<UserCreateModel>? UserCreatedCallback { get; set; }

    public Action<UserUpdatedModel>? UserUpdatedCallback { get; set; }

    public Action<Guid>? UserDeletedCallback { get; set; }

}
