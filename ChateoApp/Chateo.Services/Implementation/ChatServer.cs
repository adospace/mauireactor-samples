using Chateo.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Services.Implementation;

internal class ChatServer : IChatServer
{
    private readonly HttpClient _httpClient;
    private readonly HubConnection _connection;
    private List<UserViewModel>? _users;
    private List<MessageViewModel>? _messages;

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

    public event EventHandler<MessageViewModel>? MessageCreated;
    public event EventHandler<UserViewModel>? UserCreated;
    public event EventHandler<Guid>? UserDeleted;
    public event EventHandler<UserViewModel>? UserUpdated;
    public event EventHandler<MessageViewModel>? MessageUpdated;

    public UserViewModel[] Users => _users?.ToArray() ?? throw new InvalidOperationException();

    public MessageViewModel[] Messages => _messages?.ToArray() ?? throw new InvalidOperationException();

    public async Task<UserViewModel> CreateUser(Guid id, string firstName, string lastName, string avatar)
    {
        var res = await _httpClient.PostAsJsonAsync("/users/create", new UserCreateModel(id, firstName, lastName, avatar));

        res.EnsureSuccessStatusCode();

        return await res.Content.ReadFromJsonAsync<UserViewModel>() ?? throw new InvalidOperationException();
    }

    public async Task<MessageViewModel> CreateMessage(Guid id, Guid fromUserId, Guid toUserId, string content)
    {
        var res = await _httpClient.PostAsJsonAsync("/messages/create", new MessageCreateModel(id, fromUserId, toUserId, content));

        res.EnsureSuccessStatusCode();

        return await res.Content.ReadFromJsonAsync<MessageViewModel>() ?? throw new InvalidOperationException();
    }

    public async Task<MessageViewModel> UpdateMessage(Guid id)
    {
        var res = await _httpClient.PostAsJsonAsync("/messages/update", new MessageUpdateModel(id));

        res.EnsureSuccessStatusCode();

        return await res.Content.ReadFromJsonAsync<MessageViewModel>() ?? throw new InvalidOperationException();
    }

    public async Task StartListener()
    {
        if (_connection.State != HubConnectionState.Disconnected)
        {
            return;
        }

        _users = await _httpClient.GetFromJsonAsync<List<UserViewModel>>("/users") ?? throw new InvalidOperationException();
        _messages = await _httpClient.GetFromJsonAsync<List<MessageViewModel>>("/messages") ?? throw new InvalidOperationException();

        _connection.On("UserCreated", (UserViewModel model) => OnUserCreated(model));
        _connection.On("UserUpdated", (UserViewModel model) => OnUserUpdated(model));
        _connection.On("UserDeleted", (Guid id) => OnUserDeleted(id));

        _connection.On("MessageCreated", (MessageViewModel model) => OnMessageCreated(model));
        _connection.On("MessageUpdated", (MessageViewModel model) => OnMessageUpdated(model));

        await _connection.StartAsync();
    }

    private void OnMessageUpdated(MessageViewModel model)
    {
        if (_messages == null)
        {
            return;
        }

        var indexOfUser = _messages.FindIndex(_ => _.Id == model.Id);

        if (indexOfUser != -1)
        {
            _messages[indexOfUser] = model;
            MessageUpdated?.Invoke(this, model);
        }
    }

    private void OnMessageCreated(MessageViewModel model)
    {
        if (_messages == null)
        {
            return;
        }

        var indexOfUser = _messages.FindIndex(_ => _.Id == model.Id);

        if (indexOfUser == -1)
        {
            _messages.Add(model);
            MessageCreated?.Invoke(this, model);
        }
    }

    private void OnUserDeleted(Guid id)
    {
        if (_users == null)
        {
            return;
        }

        var indexOfUser = _users.FindIndex(_ => _.Id == id);
        if (indexOfUser != -1)
        {
            _users.RemoveAt(indexOfUser);
            UserDeleted?.Invoke(this, id);
        }
    }

    private void OnUserUpdated(UserViewModel model)
    {
        if (_users == null)
        {
            return;
        }

        var indexOfUser = _users.FindIndex(_ => _.Id == model.Id);
        if (indexOfUser != -1)
        {
            _users[indexOfUser] = model;
            UserUpdated?.Invoke(this, model);
        }
    }

    private void OnUserCreated(UserViewModel model)
    {
        if (_users == null)
        {
            return;
        }

        var indexOfUser = _users.FindIndex(_ => _.Id == model.Id);
        
        if (indexOfUser == -1)
        {
            _users.Add(model);
            UserCreated?.Invoke(this, model);
        }
    }

    public async Task StopListener()
    {
        if (_connection.State == HubConnectionState.Disconnected)
        {
            return;
        }

        await _connection.StopAsync();
    }

}
