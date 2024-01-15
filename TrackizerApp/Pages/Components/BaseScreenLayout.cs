using CommunityToolkit.Maui.Core;
using MauiReactor;

namespace TrackizerApp.Pages.Components;

partial class BaseScreenLayout : Component
{
    [Prop]
    Color _statusBarColor = Theme.Grey80;

    [Prop]
    Action? _onAppearing;

    public override VisualNode Render()
         => ContentPage([..
                Children(),
                new StatusBarBehavior()
                    .StatusBarColor(_statusBarColor)
                    .StatusBarStyle(StatusBarStyle.LightContent)])
        .BackgroundColor(Theme.Grey80)
        .HasNavigationBar(false)
        .OnAppearing(_onAppearing)
        ;
}
