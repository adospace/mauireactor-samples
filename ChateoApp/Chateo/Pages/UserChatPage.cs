using Chateo.Resources.Styles;
using Chateo.Services;
using Chateo.Shared;
using Humanizer;
using MauiReactor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Dispatching;
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

    public float KeyboardHeight { get; set; }

    //Hack also required for CollectionView under iOS .net7 (https://github.com/dotnet/maui/pull/14951)
    public double BodyHeight { get; set; }

    //Page padding under iOS
    public Thickness PagePadding { get; set; }

    public double TitleBarHeight { get; set; }

    public double EntryBoxHeight { get; set; }
}

class UserChatPageProps
{
    public UserViewModel? CurrentUser { get; set; }

    public UserViewModel? OtherUser { get; set; }
}

class UserChatPage : Component<UserChatPageState, UserChatPageProps>
{
#if IOS
    const double EntryBoxHeight = 128;
#else
    const double EntryBoxHeight = 58;
#endif

    protected override async void OnMounted()
    {
        var keyboardInteractionService = Services.GetRequiredService<IKeyboardInteractionService>();
        var chatServer = Services.GetRequiredService<IChatServer>();

        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        var myMessages = chatServer.Messages
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


        chatServer.MessageCreated += OnMessageCreated;
        chatServer.MessageUpdated += OnMessageUpdated;

        keyboardInteractionService.KeyboardHeightChanged += KeyboardInteractionService_KeyboardHeightChanged;

        base.OnMounted();
    }

    private void KeyboardInteractionService_KeyboardHeightChanged(object? sender, float height)
    {
        SetState(s => s.KeyboardHeight = (float)Math.Max(0, height - EntryBoxHeight - s.PagePadding.Bottom));
    }

    protected override void OnWillUnmount()
    {
        var keyboardInteractionService = Services.GetRequiredService<IKeyboardInteractionService>();
        var chatServer = Services.GetRequiredService<IChatServer>();
        
        chatServer.MessageCreated -= OnMessageCreated;
        chatServer.MessageUpdated -= OnMessageUpdated;

        keyboardInteractionService.KeyboardHeightChanged -= KeyboardInteractionService_KeyboardHeightChanged;

        base.OnWillUnmount();
    }

    public override VisualNode Render()
    {
        return ContentPage(
            (State.IsLoading || Props.OtherUser == null || Props.CurrentUser == null) ?
            ActivityIndicator()
                .IsVisible(true)
                .IsRunning(true)
                .HCenter()
                .VCenter()
                :
            Grid("Auto, *, Auto", "*",
                RenderMessages()
                    .OniOS(_=>_.TranslationY(-State.KeyboardHeight))
                    .OnAndroid(_=>_.TranslationY(State.KeyboardHeight > 0 ? State.KeyboardHeight + EntryBoxHeight : 0))
                    ,

                RenderTitleBar()
                    .OnAndroid(_=>_.TranslationY(State.KeyboardHeight > 0 ? State.KeyboardHeight + EntryBoxHeight : 0)),

                RenderEntryBox()
                    .OniOS(_=>_.TranslationY(-State.KeyboardHeight))
            )
            //Hack also required for CollectionView under iOS .net7 (https://github.com/dotnet/maui/pull/14951)
            .OnSizeChanged(size => SetState(s => s.BodyHeight = size.Height, false))
            
        )
#if IOS
        .OnAppearing((sender, args) =>
        {
            var page = (MauiControls.ContentPage?)sender;
            //NOT WORKING https://github.com/dotnet/maui/issues/2657
            //var safeAreaInsets = page?.On<iOS>().SafeAreaInsets();
            //if (safeAreaInsets != null && page != null)
            //{
            //    page.Padding = customSafe.Value;
            //}

            //HACK:
            var safeAreaInsets = Microsoft.Maui.ApplicationModel.WindowStateManager.Default.GetCurrentUIWindow()?.SafeAreaInsets;
            if (safeAreaInsets != null && page != null)
            {
                SetState(s => s.PagePadding = new Thickness(-safeAreaInsets.Value.Left, -safeAreaInsets.Value.Top, -safeAreaInsets.Value.Right, -safeAreaInsets.Value.Bottom));
            }
        })
#endif
        .Padding(State.PagePadding)
        .BackgroundColor(Theme.Current.Background)
        .Set(MauiControls.Shell.NavBarIsVisibleProperty, false)

        .OniOS(_=>_.Set(MauiControls.PlatformConfiguration.iOSSpecific.Page.UseSafeAreaProperty, false))

        ;
    }

    private Grid RenderTitleBar()
    {
        return Grid("*", "24, 48, *, 32, 24",
            Theme.Current.Image(Icon.Back)
                .OnTapped(OnBackClicked),

            Theme.Current.Avatar(Props.OtherUser!.Avatar)
                .HeightRequest(24)
                .WidthRequest(24)
                .VCenter()
                .HEnd()
                .Margin(0,13)
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
        )
        .OnSizeChanged(size => SetState(s => s.TitleBarHeight = size.Height))
        .Padding(16, 13 - State.PagePadding.Top, 16, 13)
        .BackgroundColor(Theme.Current.Background)
        ;
    }

    private Grid RenderMessages() 
        => Grid(
            Border()
                .BackgroundColor(Theme.Current.MediumBackground)
                .Margin(-16, 0),

            CollectionView()
                .ItemsSource(State.Messages, RenderMessageItem)
                .ItemsUpdatingScrollMode(MauiControls.ItemsUpdatingScrollMode.KeepLastItemInView)
                .OnLoaded((sender, args) =>
                {
                    if (State.Messages.Count > 0)
                    {
                        ContainerPage?.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(100), () =>
                            ((MauiControls.CollectionView?)sender)?.ScrollTo(State.Messages[^1]));
                    }
                })
                //Hack also required for CollectionView under iOS .net7 (https://github.com/dotnet/maui/pull/14951)
                .OniOS(_=>_.HeightRequest(() => State.BodyHeight - State.TitleBarHeight - State.EntryBoxHeight - State.KeyboardHeight))
                .Margin(0, 16)
        )
        .GridRow(1);

    private Grid RenderEntryBox() 
        => Grid("*", "48,*,48",
            Theme.Current.Image(Icon.Plus)
                .OnAndroid(_ => _.Margin(12, 5))
                .OniOS(_ => _.Margin(12, 12))
                .VStart()
                ,

            Theme.Current.Entry()
                .OnTextChanged(text => SetState(s => s.CurrentMessage = text, false))
                .Text(State.CurrentMessage)
                .Placeholder("Message")
                .FontSize(14)
                .VerticalTextAlignment(TextAlignment.Center)

                .VStart()
                .GridColumn(1)
                .OnAndroid(_ => _.Margin(0, -5))
                .OniOS(_ => _.Margin(0, 5))
                ,

            Theme.Current.ImageButton(Icon.Send)
                .IsEnabled(() => !string.IsNullOrWhiteSpace(State.CurrentMessage))
                .OniOS(_ => _.Padding(12, 0))
                .OnAndroid(_ => _.WidthRequest(24))

                .OnClicked(SendMessage)
                .VStart()
                .GridColumn(2)

        )
        .HeightRequest(EntryBoxHeight - State.PagePadding.Bottom)
        .OnSizeChanged(size => SetState(s => s.EntryBoxHeight = size.Height))
        .BackgroundColor(Theme.Current.Background)

        .Padding(16, 13, 16, 13)
        .GridRow(2);

    private VisualNode RenderMessageItem(MessageViewModel item)
    {
        bool myMessage = item.FromUserId == Props.CurrentUser?.Id;

        static string FormatTimeStamp(DateTime timeStamp)
        {
            if (timeStamp.Date == DateTime.Today)
            {
                return timeStamp.ToString("t");
            }

            return timeStamp.ToString("d");
        }

        return Border(
            VStack(
                Label(item.Content)
                    .TextColor(myMessage ? Theme.Current.ForegroundAccent : Theme.Current.Foreground)
                    .FontSize(14)
                    .HeightRequest(24)
                    .VerticalTextAlignment(TextAlignment.Center)
                    .VFill()
#if IOS
                    .Margin(myMessage ? 10 : 0,0)
#endif
                    ,

                Label(!myMessage ? FormatTimeStamp(item.TimeStamp) : (item.ReadTimeStamp != null ? $"{FormatTimeStamp(item.TimeStamp)} - Read" : FormatTimeStamp(item.TimeStamp)))
                    .TextColor(myMessage ? Theme.Current.ForegroundAccent : Theme.Current.Foreground)
                    .HorizontalOptions(myMessage ? MauiControls.LayoutOptions.End : MauiControls.LayoutOptions.Start)
            )
        )
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

    private void OnMessageCreated(object? sender, MessageViewModel message)
    {
        if (!((message.ToUserId == Props.CurrentUser?.Id && message.FromUserId == Props.OtherUser?.Id) || (message.ToUserId == Props.OtherUser?.Id && message.FromUserId == Props.CurrentUser?.Id)))
        {
            return;
        }

        SetState(s => s.Messages.Add(message));
    }

    private void OnMessageUpdated(object? sender, MessageViewModel message)
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
