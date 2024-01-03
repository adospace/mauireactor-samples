using Chateo.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages.Components;


partial class TabBarComponent : Component
{
    [Prop]
    PageType _page;

    [Prop]
    private Action<PageType>? _onPageChanged;

    public override VisualNode Render()
    {
        return Grid("*", "* * *",
            Border()
                .HeightRequest(2)
                .BackgroundColor(Theme.Current.MediumForeground)
                .GridColumnSpan(3)
                .VStart(),

            RenderButtonIcon(PageType.Contacts),

            RenderButtonIcon(PageType.Chats)
                .GridColumn(1),

            RenderButtonIcon(PageType.Settings)
                .GridColumn(2)
        )
        .BackgroundColor(Theme.Current.Background);
    }

    static Icon GetIconFromPage(PageType page)
        => page switch
        {
            PageType.Chats => Icon.ChatsMenu,
            PageType.Contacts => Icon.Contacts,
            PageType.Settings => Icon.Dots,
            _ => throw new NotSupportedException(),
        };

    Grid RenderButtonIcon(PageType page)
        => _page == page ?
        Grid(
            Theme.Current.Label(page.ToString())
                .HorizontalTextAlignment(TextAlignment.Center)
                .VerticalTextAlignment(TextAlignment.Center),

            Ellipse()
                .HeightRequest(8)
                .WidthRequest(8)
                .Fill(Theme.Current.Foreground)
                .VEnd()
                .Margin(16)
        )
        :
        Grid(
            Button()
                .BackgroundColor(Colors.Transparent)
                .BorderColor(Colors.Transparent)
                .BorderWidth(0)
                .OnClicked(()=> _onPageChanged?.Invoke(page)),

            Theme.Current.Image(GetIconFromPage(page))
                .HCenter()
                .VCenter()
                .HeightRequest(44)
                .WidthRequest(58)
                .OnTapped(()=> _onPageChanged?.Invoke(page))
        );
}
