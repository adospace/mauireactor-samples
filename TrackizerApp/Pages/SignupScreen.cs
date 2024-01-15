using MauiReactor;
using MauiReactor.Parameters;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages;


partial class SignupScreen : Component
{
    [Param]
    IParameter<User> _loggedUser;

    public override VisualNode Render()
        => new BaseScreenLayout
        {
            Grid(
                Image("back.png")
                    .HeightRequest(24)
                    .WidthRequest(24)
                    .Margin(0,32)
                    .HStart()
                    .VStart()
                    .OnTapped(OnBack)
                    ,

                Image("full_logo.png")
                    .WidthRequest(178)
                    .VStart()
                    .Margin(0,60),

                VStack(spacing: 16,
                    new RoundedEntry()
                        .LabelText("E-mail address"),
                    new RoundedPasswordEntry()
                        .LabelText("Password"),

                    Theme.PrimaryButton("Get started, it’s free!", OnSignup)
                    )
                    .VCenter(),

                VStack(spacing: 20,
                    Theme.BodyMedium("Do you have already an account?")
                        .TextColor(Theme.White)
                        .HorizontalTextAlignment(TextAlignment.Center),

                    Theme.Button("Sign In", OnSignin)
                    )
                    .Margin(0,30)
                    .VEnd()
                )
            .Margin(24,0)
        };

    async void OnSignin()
    {
        await Navigation!.PushModalAsync<SigninScreen>();
    }

    async void OnSignup()
    {
        _loggedUser.Set(_ =>
        {
            _.IsLoggedIn = true;
            _.Email = "j.doe@gmail.com";
            _.Name = "John Doe";
        });
        await Navigation!.PopModalAsync();
    }

    async void OnBack()
    {
        await Navigation!.PopModalAsync();
    }
}
