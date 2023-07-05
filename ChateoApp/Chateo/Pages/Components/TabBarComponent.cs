using Chateo.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages.Components;


class TabBarComponent : Component
{
    private Page _page;
    private Action<Page>? _action;

    public TabBarComponent Page(Page page)
    {
        _page = page;
        return this;
    }

    public TabBarComponent OnPageChanged(Action<Page> action)
    {
        _action = action;
        return this;
    }

    public override VisualNode Render()
    {
        return new Grid("83", "* * *")
        {
            RenderButtonIcon(Pages.Page.Contacts),

            RenderButtonIcon(Pages.Page.Chats)
                .GridColumn(1),

            RenderButtonIcon(Pages.Page.Settings)
                .GridColumn(2),
        }
        .BackgroundColor(Theme.Current.Background)
        .Shadow(new Shadow());
    }

    static Icon GetIconFromPage(Page page)
        => page switch
        {
            Pages.Page.Chats => Icon.ChatsMenu,
            Pages.Page.Contacts => Icon.Contacts,
            Pages.Page.Settings => Icon.Dots,
            _ => throw new NotSupportedException(),
        };

    Grid RenderButtonIcon(Page page)
        => _page == page ?
        new Grid()
        {
            Theme.Current.Label(page.ToString())
                .HorizontalTextAlignment(TextAlignment.Center)
                .VerticalTextAlignment(TextAlignment.Center),

            new Ellipse()
                .HeightRequest(8)
                .WidthRequest(8)
                .Fill(Theme.Current.Foreground)
                .VEnd()
                .Margin(16)
        }
        :
        new ()
        {
            new Button()
                .BackgroundColor(Colors.Transparent)
                .BorderColor(Colors.Transparent)
                .BorderWidth(0)
                .OnClicked(()=> _action?.Invoke(page)),

            Theme.Current.Image(GetIconFromPage(page))
                .HCenter()
                .VCenter()
                .HeightRequest(44)
                .WidthRequest(58)
                .OnTapped(()=> _action?.Invoke(page))
        };
}
