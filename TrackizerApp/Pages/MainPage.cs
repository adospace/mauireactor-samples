using CommunityToolkit.Maui.Core;
using MauiReactor;
using MauiReactor.Animations;
using System.Threading.Tasks;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages;

class MainPage : Component
{
    public override VisualNode Render()
        => NavigationPage(new HomeScreen());
}
