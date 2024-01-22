using MauiReactor;
using MauiReactor.Parameters;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;
using TrackizerApp.Pages.Dialogs;
using TrackizerApp.Pages.Views;

namespace TrackizerApp.Pages;



public enum HomeScreenView
{
    [Display(Name = "")]
    Home,

    [Display(Name = "Spending & Budgets")]
    Budgets,

    [Display(Name = "Calendar")]
    Calendar,

    [Display(Name = "Credit Cards")]
    CreditCards
}

class HomeScreenState
{
    //public HomeScreenView View { get; set; }

}

partial class HomeScreen : Component<HomeScreenState>
{
    [Param]
    IParameter<User> _loggedUser;

    public override VisualNode Render()
        => Shell(
            TabBar(
                Tab(
                    ShellContent(RenderTabPage(HomeScreenView.Home)),
                    ShellContent().RenderContent(() => RenderTabPage(HomeScreenView.Budgets)),
                    ShellContent().RenderContent(() => RenderTabPage(HomeScreenView.Calendar)),
                    ShellContent().RenderContent(() => RenderTabPage(HomeScreenView.CreditCards))
                    )
                )
            );

    VisualNode RenderTabPage(HomeScreenView view)
        => new BaseScreenLayout
        {
            Grid(
                RenderPageBody(view),

                Theme.H3(view.GetDisplayName())
                    .VStart()
                    .HCenter()
                    .TextColor(Theme.Grey30)
                    .Margin(23,32),

                Image("settings_dark.png")
                    .HeightRequest(24)
                    .VStart()
                    .HEnd()
                    .Margin(23,32)
                    .OnTapped(ShowSettings)
                )
        }
        .OnAppearing(OnAppearing)
        .StatusBarColor(view == HomeScreenView.Home || view == HomeScreenView.Calendar ? Theme.Grey70 : Theme.Grey80)
        ;

    void ShowSettings()
    {
        
    }

    async void OnAppearing()
    {
        if (!_loggedUser.Value.IsLoggedIn && 
            Navigation?.ModalStack.Count == 0)
        {
            await Navigation.PushModalAsync<WelcomeScreen>();
        }
    }

    Grid RenderPageBody(HomeScreenView view)
        => Grid(

            RenderView(view),

            new NavigationBar()
                .View(view)
                //.OnViewChanged(view => SetState(s => s.View = view))
                .OnNewSubscription(OnNewSubscription)
            );

    void OnNewSubscription()
    {
        Navigation?.PushModalAsync<NewSubscription>();
    }

    VisualNode RenderView(HomeScreenView view) 
        => view switch
        {
            HomeScreenView.Home => new HomeView()/*.OnShowBudgetView(()=>SetState(s => s.View = HomeScreenView.Budgets))*/,
            HomeScreenView.Budgets => new BudgetsView(),
            HomeScreenView.Calendar => new CalendarView(),
            HomeScreenView.CreditCards => new CreditCardsView(),
            _ => throw new NotSupportedException(),
        };
}
