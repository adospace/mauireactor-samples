using Chateo.Pages.Components;
using Chateo.Resources.Styles;
using Chateo.Services;
using Chateo.Shared;
using Humanizer;
using MauiReactor;
using MauiReactor.Shapes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages;

public class ContactsPageState
{
    public ObservableCollection<UserViewModel> Users { get; set; } = new();
}

public class ContactsPage : Component<ContactsPageState>
{
    protected override void OnMounted()
    {
        var chatService = Services.GetRequiredService<IChatServer>();

        chatService.UserCreated += OnUserCreated;
        chatService.UserUpdated += OnUserUpdated;
        chatService.UserDeleted += OnUserDeleted;

        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        State.Users = new ObservableCollection<UserViewModel>(chatService.Users.Where(_ => _.Id != currentUser.Id));

        base.OnMounted();
    }

    protected override void OnWillUnmount()
    {
        var chatService = Services.GetRequiredService<IChatServer>();

        chatService.UserCreated -= OnUserCreated;
        chatService.UserUpdated -= OnUserUpdated;
        chatService.UserDeleted -= OnUserDeleted;

        base.OnWillUnmount();
    }

    public override VisualNode Render()
    {
        return new Grid("56,68,*", "*")
        {
            new Grid("24", "*, 24")
            {
                Theme.Current.Label("Contacts")
                    .FontSize(18),

                Theme.Current.Image(Icon.Plus)
                    .GridColumn(1)
            }
            .VEnd()
            .Margin(0,13),

            new Border
            {
                new Grid
                {
                    Theme.Current.Image(Icon.Search)
                        .HeightRequest(24)
                        .HStart()
                        .Margin(8),

                    Theme.Current.Entry()
                        .Placeholder("Search")
                        .Margin(32,0,4,0)
                }
            }
            .BackgroundColor(Theme.Current.MediumBackground)
            .StrokeShape(new RoundRectangle().CornerRadius(4))
            .HeightRequest(36)
            .Margin(0, 16)
            .GridRow(1),

            new CollectionView()
                .ItemsSource(State.Users, RenderContactItem)
                .GridRow(2)
        }
        .Margin(24, 16);
    }

    private VisualNode RenderContactItem(UserViewModel user)
    {
        var lastSeen = DateTime.Now - user.LastSeen;
        var online = lastSeen.TotalMinutes < 2;

        return new Grid("68,2", "56, *")
        {
            new Rectangle()
                .Fill(Theme.Current.Neutral)
                .GridColumnSpan(2)
                .GridRow(1)
                .VEnd(),

            new Image($"{user.Avatar}.png")
                .Margin(0,0,0,12),

            new VStack
            {
                Theme.Current.Label($"{user.FirstName} {user.LastName}")
                    .FontSize(14)
                    .HeightRequest(24)
                    .VerticalTextAlignment(TextAlignment.Center),

                Theme.Current.Label(online ? "Online" : $"Last seen {lastSeen.Humanize(1)} ago")
                    .TextColor(Theme.Current.MediumForeground)
                    .FontSize(12)
                    .HeightRequest(20)
                    .VerticalTextAlignment(TextAlignment.Center)
            }
            .Margin(12,0,0,12)
            .GridColumn (1),
        }
        .OnTapped(()=> OnOpenUserChatPage(user));
    }

    private void OnUserDeleted(object? sender, Guid id)
    {
        var userToDeleted = State.Users.FirstOrDefault(_ => _.Id == id);
        if (userToDeleted != null)
        {
            SetState(s => s.Users.Remove(userToDeleted));
        }
    }

    private void OnUserUpdated(object? sender, UserViewModel user)
    {
        var userToReplace = State.Users.FirstOrDefault(_ => _.Id == user.Id);
        if (userToReplace != null)
        {
            SetState(s =>
            {
                s.Users.Remove(userToReplace);
                s.Users.Add(user);
            });
        }
    }

    private void OnUserCreated(object? sender, UserViewModel user)
    {
        SetState(s => s.Users.Add(user));
    }

    private async void OnOpenUserChatPage(UserViewModel otherUser)
    {
        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        await MauiControls.Shell.Current.GoToAsync<UserChatPageProps>("chat", props =>
        {
            props.OtherUser = otherUser;
            props.CurrentUser = currentUser;
        });
    }
}
