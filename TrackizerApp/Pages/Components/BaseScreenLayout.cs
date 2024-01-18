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
                #if !IOS
                new StatusBarBehavior()
                    .StatusBarColor(_statusBarColor)
                    .StatusBarStyle(StatusBarStyle.LightContent)
                #endif
                    ])
        .Padding(-1)
        .BackgroundColor(Theme.Grey80)
        .HasNavigationBar(false)
        .OnAppearing(_onAppearing)
        ;
}
