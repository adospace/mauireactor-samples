using Chateo.Pages.Components;
using Chateo.Resources.Styles;
using MauiReactor;
using System;

namespace Chateo.Pages;

enum PageType
{
    Contacts,

    Chats,

    Settings
}

class HomePageState
{
    public PageType CurrentPage { get; set; }
}

class HomePage : Component<HomePageState>
{
    protected override void OnMountedOrPropsChanged()
    {
        Routing.RegisterRoute<UserChatPage>("chat");

        base.OnMountedOrPropsChanged();
    }


    public override VisualNode Render()
    {
        return Shell(
            ContentPage("Home",
                Grid("*, 83", "*",
                    RenderCurrentPage(),

                    new TabBarComponent()
                        .Page(State.CurrentPage)
                        .OnPageChanged(page => SetState(s => s.CurrentPage = page))
                        .GridRow(1)
                )
            )
            .BackgroundColor(Theme.Current.Background)
            .Set(MauiControls.Shell.NavBarIsVisibleProperty, false)
        );
    }

    VisualNode RenderCurrentPage()
        => State.CurrentPage switch
        {
            PageType.Contacts => new ContactsPage(),
            PageType.Chats => new ChatsPage(),
            PageType.Settings => new SettingsPage(),
            _ => throw new NotSupportedException(),
        };
}