using Chateo.Resources.Styles;
using Chateo.Services;
using Chateo.Shared;
using Humanizer;
using MauiReactor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Platform;
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
    
    public string CurrentMessage { get; set; } = string.Empty;
}

class UserChatPageProps
{
    public UserViewModel? CurrentUser { get; set; }

    public UserViewModel? OtherUser { get; set; }
}

class UserChatPage : Component<UserChatPageState, UserChatPageProps>
{
    protected override void OnMountedOrPropsChanged()
    {
        State.IsLoading = true;

        LoadMessages();

        base.OnMountedOrPropsChanged();
    }

    private async void LoadMessages()
    {
        var chatServer = Services.GetRequiredService<IChatServer>();

        var messages = await chatServer.GetAllMessages();
        var users = await chatServer.GetAllUsers();

        chatServer.MessageCreatedCallback = OnNewMessage;
        chatServer.MessageUpdatedCallback = OnUpdatedMessage;

        var myMessages = messages
                    .Where(_ => (_.ToUserId == Props.CurrentUser?.Id && _.FromUserId == Props.OtherUser?.Id) || (_.ToUserId == Props.OtherUser?.Id && _.FromUserId == Props.CurrentUser?.Id))
                    .OrderBy(_ => _.TimeStamp)
                    .ToArray();

        //see my messages
        await Task.WhenAll(myMessages
            .Where(_ => _.ToUserId == Props.CurrentUser?.Id && _.ReadTimeStamp == null)
            .Select(_ => chatServer.UpdateMessage(_.Id)));

        SetState(s =>
        {
            s.Messages = new ObservableCollection<MessageViewModel>(myMessages);
            s.IsLoading = false;
        }, 300); //give some time to the UI to render
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
            }
            .BackgroundColor(Theme.Current.Background)
            .Set(MauiControls.Shell.NavBarIsVisibleProperty, false);
        }

        return new ContentPage()
        {
            new Grid("58, *, 56", "*")
            {
                RenderTitleBar(),

                RenderMessages(),

                RenderEntryBox(),
            }
            .Margin(16)
        }
        .BackgroundColor(Theme.Current.Background)
        .Set(MauiControls.Shell.NavBarIsVisibleProperty, false)
        ;
    }

    private Grid RenderTitleBar()
    {
        return new Grid("*", "24, 48, *, 32, 24")
        {
            Theme.Current.Image(Icon.Back)
                .OnTapped(OnBackClicked),

            Theme.Current.Avatar(Props.OtherUser!.Avatar)
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
        .Padding(0, 13);
    }

    private IEnumerable<VisualNode> RenderMessages()
    {
        return new VisualNode[]
        {
            new Border()
                .GridRow(1)
                .BackgroundColor(Theme.Current.MediumBackground)
                .Margin(-16, 0),

            new CollectionView()
                .ItemsSource(State.Messages, RenderMessageItem)
                .ItemsUpdatingScrollMode(MauiControls.ItemsUpdatingScrollMode.KeepLastItemInView)
                .OnLoaded((sender, args)=> ContainerPage?.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), ()=>
                {
                    if (State.Messages.Count > 0)
                    {
                        ((MauiControls.CollectionView?)sender)?.ScrollTo(State.Messages[State.Messages.Count-1]);
                    }
                }))
                .Margin(0, 16)
                .GridRow(1)
        };
    }

    private Grid RenderEntryBox()
    {
        return new Grid("56", "48,*,48")
        {
            Theme.Current.Image(Icon.Plus)
                .Margin(12,10),

            Theme.Current.Entry()
                .OnTextChanged(text => SetState(s => s.CurrentMessage = text, false))
                .Text(State.CurrentMessage)
                .Placeholder("Message")
                .FontSize(14)
                .VerticalTextAlignment(TextAlignment.Center)
                .GridColumn(1)
                .Margin(0,10),

            Theme.Current.ImageButton(Icon.Send)
                .IsEnabled(()=> !string.IsNullOrWhiteSpace(State.CurrentMessage))
                .Margin(12,10)
                .OnClicked(SendMessage)
                .GridColumn(2)
        }
        .GridRow(2);
    }

    private VisualNode RenderMessageItem(MessageViewModel item)
    {
        bool myMessage = item.FromUserId == Props.CurrentUser?.Id;

        string FormatTimeStamp(DateTime timeStamp)
        {
            if (timeStamp.Date == DateTime.Today)
            {
                return timeStamp.ToString("t");
            }

            return timeStamp.ToString("d");
        }

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

                new Label(myMessage ? FormatTimeStamp(item.TimeStamp) : (item.ReadTimeStamp != null ? $"{FormatTimeStamp(item.TimeStamp)} - Read" : FormatTimeStamp(item.TimeStamp)))
                    .TextColor(myMessage ? Theme.Current.ForegroundAccent : Theme.Current.Foreground)
                    .HorizontalOptions(myMessage ? MauiControls.LayoutOptions.End : MauiControls.LayoutOptions.Start),
            }
        }
        .BackgroundColor(myMessage ? Theme.Current.Accent : Theme.Current.Background)
        .HorizontalOptions(myMessage ? MauiControls.LayoutOptions.End : MauiControls.LayoutOptions.Start)
        .StrokeCornerRadius(myMessage ? new CornerRadius(16, 16, 16, 0) : new CornerRadius(16, 16, 0, 16))
        .Padding(10)
        .Margin(0,0,0,12)
        .MinimumWidthRequest(200);
    }

    private async void SendMessage()
    {
        var chatServer = Services.GetRequiredService<IChatServer>();

        await chatServer.CreateMessage(Guid.NewGuid(), Props.CurrentUser!.Id, Props.OtherUser!.Id, State.CurrentMessage);

        SetState(s => s.CurrentMessage = string.Empty);
    }

    private void OnNewMessage(MessageViewModel message)
    {
        if (!((message.ToUserId == Props.CurrentUser?.Id && message.FromUserId == Props.OtherUser?.Id) || (message.ToUserId == Props.OtherUser?.Id && message.FromUserId == Props.CurrentUser?.Id)))
        {
            return;
        }

        SetState(s => s.Messages.Add(message));
    }

    private void OnUpdatedMessage(MessageViewModel message)
    {
        if (!((message.ToUserId == Props.CurrentUser?.Id && message.FromUserId == Props.OtherUser?.Id) || (message.ToUserId == Props.OtherUser?.Id && message.FromUserId == Props.CurrentUser?.Id)))
        {
            return;
        }

        SetState(s =>
        {
            var indexOfExistingMessage = s.Messages.IndexOf(s.Messages.First(_ => _.Id == message.Id));
            s.Messages[indexOfExistingMessage] = message;
        });
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
