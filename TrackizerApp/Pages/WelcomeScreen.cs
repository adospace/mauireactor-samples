using MauiReactor;
using MauiReactor.Parameters;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages;

partial class WelcomeScreen : Component
{
    [Param]
    IParameter<User> _loggedUser;

    public override VisualNode Render()
        => new BaseScreenLayout
        {
            Grid(
                Border()
                    .HeightRequest(800)
                    .WidthRequest(1000)
                    .RotationX(200)
                    .RotationY(200)
                    .Rotation(180)
                    .TranslationX(200)
                    .Background(new RadialGradient(ApplicationTheme.Accentp100.WithAlpha(0.2f), Colors.Transparent)),

                Image("full_logo.png")
                    .WidthRequest(178)
                    .VStart()
                    .Margin(0,60),

                Image("welcome_background.png")
                    .WidthRequest(289),

                new RotatingImage()
                    .Source("welcome_image.png")
                    .Width(288)
                    .TranslationX(-280)
                    .TranslationY(-100),

                new RotatingImage()
                    .Source("welcome_image.png")
                    .Width(288)
                    .TranslationX(280)
                    .TranslationY(100),

                new RotatingImage()
                    .Source("welcome_you_tube.png")
                    .Width(143)
                    .TranslationX(80)
                    .TranslationY(-100),

                new RotatingImage()
                    .Source("welcome_netflix.png")
                    .Width(143)
                    .TranslationX(-80)
                    .TranslationY(-10),

                new RotatingImage()
                    .Source("welcome_spotify.png")
                    .Width(243)
                    .TranslationX(50)
                    .TranslationY(100),

                VStack(spacing: 16,
                    ApplicationTheme.PrimaryButton("Get Started", OnGetStarted),

                    ApplicationTheme.Button("I have an account", OnHaveAnAccount)
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

    async void OnGetStarted() => await Navigation!.PushModalAsync<RegisterScreen>();

    async void OnHaveAnAccount() => await Navigation!.PushModalAsync<SigninScreen>();
}
