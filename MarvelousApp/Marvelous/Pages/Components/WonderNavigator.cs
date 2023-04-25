using Marvelous.Models;
using Marvelous.Resources.Styles;
using MauiReactor;
using MauiReactor.Canvas;
using System;
using System.Linq;

namespace Marvelous.Pages.Components;

class WonderNavigatorState
{
    public NavigatorTabKey CurrentTab { get; set; }
}

class WonderNavigator : Component<WonderNavigatorState>
{
    private WonderType _type;
    private Action? _backToWonderSelectionAction;
    private Action<NavigatorTabKey>? _onTabSelected;
    private bool _show;

    public WonderNavigator Type(WonderType type)
    {
        _type = type;
        return this;
    }

    public WonderNavigator OnBackToWonderSelect(Action action)
    {
        _backToWonderSelectionAction = action;
        return this;
    }

    public WonderNavigator OnTabSelected(Action<NavigatorTabKey> action)
    {
        _onTabSelected = action;
        return this;
    }

    public WonderNavigator Show(bool show)
    {
        _show = show;
        return this;
    }

    public override VisualNode Render()
    {
        return new CanvasView
        {
            new Box()
                .Margin(0,20,0,0)
                .BackgroundColor(Theme.TertiaryColor),

            new Row()
            {
                new PointInteractionHandler
                {
                    new Align
                    {
                        new Group
                        {
                            new Group
                            {
                                new Ellipse()
                                    .FillColor(Theme.TertiaryColor),

                                new Ellipse()
                                    .Margin(5)
                                    .FillColor(Theme.DarkTertiaryColor),

                            },

                            new Align
                            {
                                new ClipRectangle
                                {
                                    new Picture($"Marvelous.Resources.Images.{Illustration.Config[_type].WonderButton}")
                                }
                                .CornerRadius(80)
                            }
                            .Margin(5)
                        }
                    }
                    .VCenter()
                    .HCenter()
                    .Width(70)
                    .Height(70)
                }
                .OnTap(_backToWonderSelectionAction),

                Enum.GetValues<NavigatorTabKey>().Select(tabKey=>
                    new WonderNavigatorTab()
                        .TabKey(tabKey)
                        .IsActive(State.CurrentTab == tabKey)
                        .OnSelected(OnTabSelected)
                        )
                .ToArray()
            }
        }
        .TranslationY(_show ? 0 : 80)
        .WithAnimation(duration: 400)
        .HeightRequest(80)
        .VEnd()
        .BackgroundColor(Colors.Transparent);
    }

    void OnTabSelected(NavigatorTabKey tabKey)
    {
        SetState(s => s.CurrentTab = tabKey);
        _onTabSelected?.Invoke(tabKey);
    }
}

class WonderNavigatorTab : Component
{
    private NavigatorTabKey _key;
    private bool _isActive;
    private Action<NavigatorTabKey>? _onSelectedAction;

    public WonderNavigatorTab TabKey(NavigatorTabKey key)
    {
        _key = key;
        return this;
    }

    public WonderNavigatorTab IsActive(bool isActive)
    {
        _isActive = isActive;
        return this;
    }

    public WonderNavigatorTab OnSelected(Action<NavigatorTabKey> action)
    {
        _onSelectedAction = action;
        return this;
    }

    public override VisualNode Render()
    {
        return new PointInteractionHandler
        {
            new Align
            {
                new Group
                {
                    new Picture($"Marvelous.Resources.Images.common_tab_{_key.ToString().ToLower()}{(_isActive ? "_active" : string.Empty)}.png"),

                    new Align
                    {
                        new Box()
                            .BackgroundColor(Theme.PrimaryColor)
                    }
                    .Margin(_isActive ? new ThicknessF(0,12) : new ThicknessF(12,12))
                    .WithAnimation(duration: 200)
                    .Height(4)
                    .VEnd()
                }
            }
            .HCenter()
            .VEnd()
            .Height(72)
            .Width(24)
        }
        .OnTap(()=>_onSelectedAction?.Invoke(_key))
        ;
    }
}