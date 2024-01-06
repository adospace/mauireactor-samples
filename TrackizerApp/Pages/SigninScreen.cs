using MauiReactor;
using MauiReactor.Parameters;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages;

class SigninScreen : Component
{
    IParameter<UserModel> _loggedUser;

    public SigninScreen()
    {
        _loggedUser = GetOrCreateParameter<UserModel>();
    }

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
