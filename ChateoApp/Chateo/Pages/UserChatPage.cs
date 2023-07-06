using Chateo.Resources.Styles;
using Chateo.Services;
using Chateo.Shared;
using Humanizer;
using MauiReactor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages;

class UserChatPageState
{
    public bool IsLoading { get; set; }

    public ObservableCollection<MessageViewModel> Messages { get; set; } = new();
}

class UserChatPageProps
{
    public UserViewModel? CurrentUser { get; set; }

    public UserViewModel? OtherUser { get; set; }
}

class UserChatPage : Component<UserChatPageState, UserChatPageProps>
{
    protected override async void OnMountedOrPropsChanged()
    {
        var chatServer = Services.GetRequiredService<IChatServer>();

        State.IsLoading = true;

        var messages = await chatServer.GetAllMessages();
        var users = await chatServer.GetAllUsers();

        chatServer.MessageCreatedCallback = this.OnNewMessage;

        SetState(s =>
        {
            s.Messages = new ObservableCollection<MessageViewModel>(
                messages
                    .Where(_ => (_.ToUserId == Props.CurrentUser?.Id && _.FromUserId == Props.OtherUser?.Id) || (_.ToUserId == Props.OtherUser?.Id && _.FromUserId == Props.CurrentUser?.Id))
                    .OrderBy(_=>_.TimeStamp)
                    .ToArray());

            s.IsLoading = false;
        });

        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        if (State.IsLoading || Props.OtherUser == null || Props.CurrentUser == null)
        {
            return new ContentPage
            {
                new ActivityIndicator()
                    .IsVisible(true)
                    .IsRunning(true)
                    .HCenter()
                    .VCenter()
            };
        }

        return new ContentPage()
        {
            new Grid("58, *, 56", "*")
            {
                new Grid("*", "24, 48, *, 32, 24")
                {
                    Theme.Current.Image(Icon.Back)
                        .OnTapped(OnBackClicked),

                    Theme.Current.Avatar(Props.OtherUser.Avatar)
                        .HeightRequest(24)
                        .WidthRequest(24)
                        .VCenter()
                        .HEnd()
                        .GridColumn(1),

                    Theme.Current.Label($"{Props.OtherUser.FirstName} {Props.OtherUser.LastName}")
                        .VCenter()
                        .Margin(24,0,8,0)
                        .FontSize(18)
                        .GridColumn(2),

                    Theme.Current.Image(Icon.Search)
                        .Margin(0,0,8,0)
                        .GridColumn(3),


                    Theme.Current.Image(Icon.Menu)
                        .GridColumn(4)
                }
                .Padding(0,13),

                new Border()
                    .GridRow(1)
                    .BackgroundColor(Theme.Current.MediumBackground)
                    .Margin(-16,0),

                new CollectionView()
                    .ItemsSource(State.Messages, RenderMessageItem)
                    .ItemsUpdatingScrollMode(MauiControls.ItemsUpdatingScrollMode.KeepLastItemInView)
                    .GridRow(1)
                    .Margin(0,16),

                RenderEntryBox()
                    .GridRow(2),
            }
            .Margin(16)
        }
        .Set(MauiControls.Shell.NavBarIsVisibleProperty, false);
    }

    private Grid RenderEntryBox()
    {
        return new Grid("56", "48,*,48")
        {
            Theme.Current.Image(Icon.Plus),

            Theme.Current.Entry()
                .GridColumn(1),

            Theme.Current.Image(Icon.Send)
                .GridColumn(2)
        };
    }

    private VisualNode RenderMessageItem(MessageViewModel item)
    {
        bool myMessage = item.FromUserId == Props.CurrentUser?.Id;
        return new Border
        {
            new VStack
            {
                new Label(item.Content)
                    .TextColor(myMessage ? Theme.Current.ForegroundAccent : Theme.Current.Foreground)
                    .FontSize(14)
                    .HeightRequest(24)
                    .VerticalTextAlignment(TextAlignment.Center)
                    .VFill(),

                new Label(item.TimeStamp.Humanize())
                    .VEnd()
            }
        }
        .BackgroundColor(myMessage ? Theme.Current.Accent : Theme.Current.Background)
        .HorizontalOptions(myMessage ? MauiControls.LayoutOptions.End : MauiControls.LayoutOptions.Start)
        .StrokeCornerRadius(myMessage ? new CornerRadius(16, 16, 16, 0) : new CornerRadius(16, 16, 0, 16))
        .Padding(10)
        .Margin(0,0,0,12);
    }

    private void OnNewMessage(MessageViewModel message)
    {
        if (!((message.ToUserId == Props.CurrentUser?.Id && message.FromUserId == Props.OtherUser?.Id) || (message.ToUserId == Props.OtherUser?.Id && message.FromUserId == Props.CurrentUser?.Id)))
        {
            return;
        }


    }

    private void OnBackClicked()
    {
        if (Navigation == null)
        {
            return;
        }

        if (Navigation.NavigationStack.Count > 0)
        {
            Navigation.PopAsync();
        }
    }
}
