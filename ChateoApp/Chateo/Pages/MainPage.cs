using Chateo.Resources.Styles;
using Chateo.Services;
using Chateo.Shared;
using MauiReactor;
using MauiReactor.Parameters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages;

class MainPageState
{
    public bool Loading { get; set; }

}

class MainState
{
    public UserViewModel? CurrentUser { get; set; }
}

class MainPage : Component<MainPageState>
{
    private readonly IParameter<MainState> _mainState;

    public MainPage()
    {
        _mainState = CreateParameter<MainState>();
    }

    protected override async void OnMounted()
    {
        if (MauiControls.Application.Current != null)
        {
            MauiControls.Application.Current.RequestedThemeChanged += (sender, args) => Invalidate();
        }
        
        var chatServer = Services.GetRequiredService<IChatServer>();

        State.Loading = true;

        await chatServer.StartListener();

        _mainState.Set(s => s.CurrentUser = Preferences.Default.GetFromJson<UserViewModel?>("current_user", null));

        if (_mainState.Value.CurrentUser != null)
        {
            if (!chatServer.Users.Any(_=>_.Id == _mainState.Value.CurrentUser.Id))
            {
                _mainState.Set(s => s.CurrentUser = null);
            }
        }

        SetState(s => s.Loading = false);
        
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        if (State.Loading)
        {
            return ContentPage(
                ActivityIndicator()
                    .IsVisible(true)
                    .IsRunning(true)
                    .HCenter()
                    .VCenter()
            )
            .BackgroundColor(Theme.Current.Background);
        }

        if (_mainState.Value.CurrentUser == null)
        {
            return RenderLogin();
        }

        return RenderHome();
    }

    private LandingPage RenderLogin()
    {
        return new LandingPage()
            .OnLogged(() => _mainState.Set(s => s.CurrentUser = Preferences.Default.GetFromJson<UserViewModel?>("current_user", null)));
    }

    private HomePage RenderHome()
    {
        return new();
    }
}