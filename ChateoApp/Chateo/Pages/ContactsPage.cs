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
    public bool IsLoading { get; set; }

    public ObservableCollection<UserViewModel> Users { get; set; } = new();
}

public class ContactsPage : Component<ContactsPageState>
{
    protected override async void OnMountedOrPropsChanged()
    {
        var chatService = Services.GetRequiredService<IChatServer>();
        
        State.IsLoading = true;

        chatService.UserCreatedCallback = this.OnNewUser;
        chatService.UserUpdatedCallback = this.OnUserUpdated;
        chatService.UserDeletedCallback = this.OnUserDeleted;

        var allUsers = await chatService.GetAllUsers();

        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        SetState(s =>
        {
            s.Users = new ObservableCollection<UserViewModel>(allUsers.Where(_ => _.Id != currentUser.Id));
            s.IsLoading = false;
        });
    }

    public override VisualNode Render()
    {
        if (State.IsLoading)
        {
            return new ActivityIndicator()
                    .IsVisible(true)
                    .IsRunning(true)
                    .HCenter()
                    .VCenter(); 
        }

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

    private VisualNode RenderContactItem(UserViewModel model)
    {
        var lastSeen = DateTime.Now - model.LastSeen;
        var online = lastSeen.TotalMinutes < 2;

        return new Grid("68", "56, *")
        {
            new Image($"/images/{model.Avatar}.png")
                .Margin(0,0,0,12),

            new VStack
            {
                Theme.Current.Label($"{model.FirstName} {model.LastName}")
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

            new Rectangle()
                .HeightRequest(1)
                .Fill(Theme.Current.Neutral)
                .GridColumnSpan(2)
                .VEnd()
        };
    }

    private void OnNewUser(UserViewModel user)
    {
        SetState(s => s.Users.Add(user));
    }

    private void OnUserDeleted(Guid id)
    {
        var userToDeleted = State.Users.FirstOrDefault(_ => _.Id == id);
        if (userToDeleted != null)
        {
            SetState(s => s.Users.Remove(userToDeleted));
        }        
    }

    private void OnUserUpdated(UserViewModel user)
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

}
