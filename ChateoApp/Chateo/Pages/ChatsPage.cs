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

record ChatItem(UserViewModel User, MessageViewModel[] Messages);

class ChatsPageState
{
    public bool IsLoading { get; set; }

    public ObservableCollection<ChatItem> Items { get; set; } = new();

    public ChatItem? SelectedItem { get; set; }
}

class ChatsPage : Component<ChatsPageState>
{

    protected override async void OnMountedOrPropsChanged()
    {
        var chatServer = Services.GetRequiredService<IChatServer>();

        State.IsLoading = true;

        var messages = await chatServer.GetAllMessages();
        var users = await chatServer.GetAllUsers();

        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        var myMessages = messages.Where(_ => _.ToUserId == currentUser.Id).ToArray();

        var items = users
            .Select(_ => new ChatItem(User: _, Messages: myMessages.Where(x => x.FromUserId == _.Id).OrderBy(_ => _.TimeStamp).ToArray()))
            .Where(_ => _.Messages.Any())
            .ToArray();

        chatServer.MessageCreatedCallback = this.OnNewMessage;

        SetState(s =>
        {
            s.Items = new ObservableCollection<ChatItem>(items);
            s.IsLoading = false;
        });

        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        return new Grid
        {
            State.IsLoading ?
            new ActivityIndicator()
                    .IsVisible(true)
                    .IsRunning(true)
                    .HCenter()
                    .VCenter()
                    :
            new Grid("56,108,52,*, 83", "*")
            {
                RenderTitleBar(),

                new ScrollView
                {
                    new HStack(spacing: 16)
                    {
                        RenderStoryItem("Your Story", Theme.Current.BorderedImage(Icon.StoryPlus)),

                        RenderStoryItem("Story 1", Theme.Current.BorderedImage("avatar1.png")),

                        RenderStoryItem("Story 2", Theme.Current.BorderedImage("avatar2.png"))
                    }
                }
                .Margin(0,16)
                .Orientation(ScrollOrientation.Horizontal)
                .GridRow(1),

                new Rectangle()
                    .HeightRequest(2)
                    .Margin(-24,0)
                    .Fill(Theme.Current.MediumBackground)
                    .VEnd()
                    .GridRow(1),

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
                .Margin(0, 16, 0, 0)
                .GridRow(2),

                new CollectionView()
                    .ItemsSource(State.Items, RenderChatItem)
                    .Margin(0, 16)
                    .GridRow (3),
            }
        }
        .Margin(24, 16);
        ;
    }

    VisualNode RenderTitleBar()
    {
        return new Grid("24", "*, 32, 24")
        {
            Theme.Current.Label("Chats")
                .FontSize(18),

            Theme.Current.Image(Icon.Comment)
                .GridColumn(1)
                .Margin(0,0,8,0),

            Theme.Current.Image(Icon.Check)
                .GridColumn(2)
        }
        .VEnd()
        .Margin(0, 13);
    }

    VisualNode RenderStoryItem(string label, Border image)
        => new Grid("56, 20", "56")
        {
            image,

            Theme.Current.Label(label)
                .FontSize(10)
                .Margin(0,4,0,0)
                .HorizontalTextAlignment(TextAlignment.Center)
                .GridRow(1)
        };

    VisualNode RenderChatItem(ChatItem item)
    {
        var notSeen = item.Messages.Count(_ => _.ReadTimeStamp == null);

        return new Grid("*, *", "56, *")
        {
            Theme.Current.Avatar(item.User.Avatar, (DateTime.Now - item.User.LastSeen).TotalMinutes < 2)
                .GridRowSpan(2),

            new Grid("*", "*, Auto")
            {
                Theme.Current.Label($"{item.User.FirstName} {item.User.LastName}")
                    .FontSize(14)
                    .VerticalTextAlignment(TextAlignment.Center),

                Theme.Current.Label(item.User.LastSeen.Humanize())
                    .TextColor(Theme.Current.MediumForeground)
                    .VerticalTextAlignment(TextAlignment.Center)
                    .GridColumn(1)
            }
            .Margin(12,0,0,0)
            .HFill()
            .GridColumn(1),

            new Grid("*", "*, 22")
            {
                Theme.Current.Label(item.Messages[0].Content)
                    .FontSize(12)
                    .TextColor(Theme.Current.MediumForeground)
                    .VerticalTextAlignment(TextAlignment.Center),

                notSeen > 0 ?
                new Grid
                {
                    new Ellipse()
                        .Fill(Theme.Current.MediumBackground),

                    Theme.Current.Label(notSeen.ToString())
                        .VerticalTextAlignment(TextAlignment.Center)
                        .HorizontalTextAlignment(TextAlignment.Center)
                }
                .GridColumn(1)
                : null
            }
            .Margin(12,0,0,0)
            .HFill()
            .GridColumn(1)
            .GridRow(2)
        }
        .OnTapped(() => OnOpenUserChatPage(item))
        .Margin(0, 16, 0, 0);
    }

    private async void OnOpenUserChatPage(ChatItem item)
    {
        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        await MauiControls.Shell.Current.GoToAsync<UserChatPageProps>("chat", props =>
        {
            props.OtherUser = item.User;
            props.CurrentUser = currentUser;
        });
    }

    private async void OnNewMessage(MessageViewModel model)
    {
        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();
        if (model.ToUserId != currentUser.Id)
        {
            return;
        }    

        var existingItem = State.Items.FirstOrDefault(_ => _.User.Id == model.FromUserId);
        if (existingItem != null)
        {
            var newMessageList = existingItem.Messages.ToList();
            newMessageList.Add(model);
            var newItem = new ChatItem(existingItem.User, newMessageList.ToArray());

            SetState(s =>
            {
                var indexToReplace = s.Items.IndexOf(existingItem);
                s.Items[indexToReplace] = newItem;
            });
        }
        else
        {
            var chatServer = Services.GetRequiredService<IChatServer>();
            var messages = await chatServer.GetAllMessages();
            var users = await chatServer.GetAllUsers();

            var myMessages = messages.Where(_ => _.ToUserId == currentUser.Id).ToArray();

            var items = users
                .Select(_ => new ChatItem(User: _, Messages: myMessages.Where(x => x.FromUserId == _.Id).OrderBy(_ => _.TimeStamp).ToArray()))
                .Where(_ => _.Messages.Any())
                .ToArray();

            chatServer.MessageCreatedCallback = this.OnNewMessage;

            SetState(s =>
            {
                s.Items = new ObservableCollection<ChatItem>(items);
            });
        }
    }

}
