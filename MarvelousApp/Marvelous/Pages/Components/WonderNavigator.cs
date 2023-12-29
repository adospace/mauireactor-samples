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

    public bool Show { get; set; }
}

partial class WonderNavigator : Component<WonderNavigatorState>
{

    static readonly NavigatorTabKey[] _navigatorTabKeys = Enum.GetValues<NavigatorTabKey>();

    [Prop]
    WonderType _type;

    [Prop]
    Action? _onBackToWonderSelect;

    [Prop]
    Action<NavigatorTabKey>? _onTabSelected;

    [Prop]
    bool _show;

    protected override void OnMounted()
    {
        MauiControls.Application.Current?.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(10), 
            () => SetState(s => s.Show = _show));
        base.OnMounted();
    }

    public override VisualNode Render() 
        => new CanvasView
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
                .OnTap(_onBackToWonderSelect),

                _navigatorTabKeys.Select(tabKey=>
                    new WonderNavigatorTab()
                        .TabKey(tabKey)
                        .IsActive(State.CurrentTab == tabKey)
                        .OnSelected(OnTabSelected))
                .ToArray()
            }
        }
        .TranslationY(State.Show ? 0 : 80)
        .WithAnimation(duration: 200)
        .HeightRequest(80)
        .VEnd()
        .BackgroundColor(Colors.Transparent);


    void OnTabSelected(NavigatorTabKey tabKey)
    {
        SetState(s => s.CurrentTab = tabKey);
        _onTabSelected?.Invoke(tabKey);
    }
}

partial class WonderNavigatorTab : Component
{
    [Prop]
    NavigatorTabKey _tabKey;

    [Prop]
    bool _isActive;

    [Prop]
    Action<NavigatorTabKey>? _onSelected;

    public override VisualNode Render()
        => new PointInteractionHandler
        {
            new Align
            {
                new Group
                {
                    new Picture($"Marvelous.Resources.Images.common_tab_{_tabKey.ToString().ToLower()}{(_isActive ? "_active" : string.Empty)}.png"),

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
        .OnTap(() => _onSelected?.Invoke(_tabKey));
}