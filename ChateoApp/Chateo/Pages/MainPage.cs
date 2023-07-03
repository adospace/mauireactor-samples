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
    public UserViewModel? CurrentUser { get; set; }


}

class MainPage : Component<MainPageState>
{

    protected override void OnMounted()
    {
        State.CurrentUser = Preferences.Default.Get<UserViewModel?>("current_user", null);

        base.OnMounted();
    }


    public override VisualNode Render()
    {
        if (State.CurrentUser == null)
        {
            return RenderLogin();
        }

        return RenderShell();
    }

    private VisualNode RenderLogin()
    {
        return new LandingPage();
    }

    private VisualNode RenderShell()
    {
        return new HomePage();
    }
}