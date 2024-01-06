using CommunityToolkit.Maui.Core;
using MauiReactor;

namespace TrackizerApp.Pages.Components;

partial class BaseScreenLayout : Component
{
    [Prop]
    Action? _onAppearing;

    public override VisualNode Render()
         => ContentPage([..
                Children(),
                new StatusBarBehavior()
                    .StatusBarColor(Theme.Grey80)
                    .StatusBarStyle(StatusBarStyle.LightContent)])
        .BackgroundColor(Theme.Grey80)
        .HasNavigationBar(false)
        .OnAppearing(_onAppearing)
        ;
}
