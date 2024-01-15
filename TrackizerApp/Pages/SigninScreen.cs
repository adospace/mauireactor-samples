using MauiReactor;
using MauiReactor.Parameters;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages;

partial class SigninScreen : Component
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
                    .Margin(0,60)
                )

            .Margin(24,0)
        };

    async void OnBack()
    {
        await Navigation!.PopModalAsync();
    }
}
