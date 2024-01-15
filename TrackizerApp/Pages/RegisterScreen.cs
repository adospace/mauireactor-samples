using MauiReactor;
using MauiReactor.Parameters;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages;

partial class RegisterScreen : Component
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
                    .Margin(24,32)
                    .HStart()
                    .VStart()
                    .OnTapped(OnBack)
                    ,

                Image("full_logo.png")
                    .WidthRequest(178)
                    .VStart()
                    .Margin(0,60),

                VStack(spacing: 16,

                    Theme.PrimaryImageButton("Sign up with Apple", null, Colors.Black, Colors.White, "apple.png"),

                    Theme.PrimaryImageButton("Sign up with Google", null, Colors.White, Colors.Black, "google.png"),

                    Theme.PrimaryImageButton("Sign up with Facebook", null, Color.FromRgba("#1771E6"), Colors.White, "facebook.png"),

                    Theme.H2("or")
                        .TextColor(Theme.White)
                        .Margin(0,40)
                        .HCenter()
                        ,

                    Theme.Button("Sign up with E-mail", OnSignupWithEmail),

                    Theme.BodySmall("By registering, you agree to our Terms of Use. Learn how we collect, use and share your data.")
                        .TextColor(Theme.Grey50)
                        .HorizontalTextAlignment(TextAlignment.Center)
                        .Margin(0,24)
                    )
                .VEnd()
                .Margin(25,30)
                )
        }
        .OnAppearing(OnAppearing)
        ;

    async void OnAppearing()
    {
        if (_loggedUser.Value.IsLoggedIn)
        {
            await Navigation!.PopModalAsync();
        }
    }

    async void OnBack()
    {
        await Navigation!.PopModalAsync();
    }

    async void OnSignupWithEmail()
    {
        await Navigation!.PushModalAsync<SignupScreen>();
    }
}
