using Chateo.Services;
using Chateo.Shared;
using MauiReactor;
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

    public UserViewModel? CurrentUser { get; set; }


}

class MainPage : Component<MainPageState>
{

    protected override void OnMounted()
    {

        base.OnMounted();
    }

    protected override async void OnMountedOrPropsChanged()
    {
        State.CurrentUser = Preferences.Default.GetFromJson<UserViewModel?>("current_user", null);

        if (State.CurrentUser != null)
        {
            State.Loading = true;

            var chatServer = Services.GetRequiredService<IChatServer>();

            var allUsers = await chatServer.GetAllUsers();

            if (!allUsers.Any(_=>_.Id == State.CurrentUser.Id))
            { 
                SetState(s => s.CurrentUser = null);
            }

            SetState(s => s.Loading = false);
        }

        base.OnMountedOrPropsChanged();
    }


    public override VisualNode Render()
    {
        if (State.Loading)
        {
            return new ContentPage
            {
                new ActivityIndicator()
                    .IsVisible(true)
                    .IsRunning(true)
                    .HCenter()
                    .VCenter()
            };
        }

        if (State.CurrentUser == null)
        {
            return RenderLogin();
        }

        return RenderHome();
    }

    private VisualNode RenderLogin()
    {
        return new LandingPage()
            .OnLogged(() => SetState(s => s.CurrentUser = Preferences.Default.GetFromJson<UserViewModel?>("current_user", null)));
    }

    private VisualNode RenderHome()
    {
        return new HomePage();
    }
}