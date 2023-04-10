using Marvelous.Models;
using Marvelous.Pages.Components;
using Marvelous.Services;
using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.Pages;

class MainPageState
{
    public WonderType CurrentWonderType { get; set; }
    public NavigatorTabKey CurrentTab { get; set; }
    public bool ShowNavigator { get; set; } = true;
}

class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid()
            {
                new MainCarouselView()
                    .OnSelected(OnSelectWonder)
                    .InitialType(State.CurrentWonderType)
                    .Show(State.ShowNavigator),

                !State.ShowNavigator ?
                RenderBody() : null,

                new WonderNavigator()
                    .OnTabSelected(_=>SetState(s => s.CurrentTab = _))
                    .Show(!State.ShowNavigator)
                    .OnBackToWonderSelect(()=>SetState(s => s.ShowNavigator = true))
            }
        };
    }

    private VisualNode RenderBody()
    {
        //if (State.CurrentTab == NavigatorTabKey.Editorial)
            return new WonderWiki()
                .Type(State.CurrentWonderType);
    }

    private void OnSelectWonder(WonderType wonderType)
    {
        SetState(s =>
        {
            s.CurrentWonderType = wonderType;
            s.ShowNavigator = false;
        });
    }
}