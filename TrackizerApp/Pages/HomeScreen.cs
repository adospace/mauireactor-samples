using MauiReactor;
using MauiReactor.Parameters;
using System.Threading.Tasks;
using TrackizerApp.Pages.Components;
using TrackizerApp.Pages.Views;

namespace TrackizerApp.Pages;

class UserModel
{
    public bool IsLoggedIn { get; set; } = true;

    public string? Email { get; set; }

    public string? Name { get; set; }
}

public enum HomeScreenView
{
    Home,

    Budgets,

    Calendar,

    CreditCards
}

class HomeScreenState
{
    public HomeScreenView View { get; set; }
}

class HomeScreen : Component<HomeScreenState>
{
    IParameter<UserModel> _loggedUser;

    public HomeScreen()
    {
        _loggedUser = GetOrCreateParameter<UserModel>();
    }

    public override VisualNode Render()
        => new BaseScreenLayout
        {
            !_loggedUser.Value.IsLoggedIn ?
            Grid()
            :
            RenderPageBody()
        }
        .OnAppearing(OnAppearing);

    async void OnAppearing()
    {
        if (!_loggedUser.Value.IsLoggedIn)
        {
            await Navigation!.PushModalAsync<WelcomeScreen>();
        }
    }

    Grid RenderPageBody()
        => Grid(

            RenderView(),

            new NavigationBar()
                .View(State.View)
                .OnViewChanged(view => SetState(s => s.View = view))

            );

    VisualNode RenderView() 
        => State.View switch
        {
            HomeScreenView.Home => new HomeView(),
            HomeScreenView.Budgets => new BudgetsView(),
            HomeScreenView.Calendar => new CalendarView(),
            HomeScreenView.CreditCards => new CreditCardsView(),
            _ => throw new NotSupportedException(),
        };
}

class BudgetsView : Component
{
    public override VisualNode Render()
        => Grid(Label("BudgetsView"));
}
class CalendarView : Component
{
    public override VisualNode Render()
        => Grid(Label("CalendarView"));
}
class CreditCardsView : Component
{
    public override VisualNode Render()
        => Grid(Label("CreditCardsView"));
}