using Chateo.Resources.Styles;
using MauiReactor;
using System;

namespace Chateo.Pages;

partial class LandingPage : Component
{
    [Prop]
    private Action? _onLogged;

    protected override void OnMountedOrPropsChanged()
    {
        Routing.RegisterRoute<RegisterPage>("register");
        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        return Shell(
            ContentPage(
                Grid("*,*,24,70", "*",
                    Image("landing.png")
                        .HeightRequest(262)
                        .WidthRequest(262),

                    Label("Connect easily with your family and friends over countries")
                        .HorizontalTextAlignment(TextAlignment.Center)
                        .FontFamily("MulishSemiBold")
                        .FontSize(24)
                        .HCenter()
                        .VStart()
                        .Margin(24, 42, 24, 0)
                        .GridRow(1)
                        .TextColor(Theme.Current.Foreground),

                    Label("Terms & Privacy Policy")
                        .HorizontalTextAlignment(TextAlignment.Center)
                        .FontFamily("MulishSemiBold")
                        .HCenter()
                        .HCenter()
                        .GridRow(2)
                        .TextColor(Theme.Current.Foreground),

                    Theme.Current.PrimaryButton("Start Messaging")
                        .OnClicked(OnOpenLoginPage)
                        .GridRow(3)
                        .Margin(0,18,0,0)
                )
                .Margin(24, 90, 24, 54)
            )
            .BackgroundColor(Theme.Current.Background)
            .Set(MauiControls.Shell.NavBarIsVisibleProperty, false)
        );
    }

    private async void OnOpenLoginPage()
    {
        await MauiControls.Shell.Current.GoToAsync<LoginPageProps>("register", props =>
        {
            props.OnLogged = _onLogged;
        });
    }
}
