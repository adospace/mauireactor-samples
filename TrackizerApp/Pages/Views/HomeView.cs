using MauiReactor;

namespace TrackizerApp.Pages.Views;


enum HomeViewListType
{
    Subscriptions,

    UpcomingBills
}

class HomeViewState
{
    public HomeViewListType ListType { get; set; }
}


class HomeView : Component<HomeViewState>
{
    public override VisualNode Render()
        => Grid(
            VStack(
                Border()
                    .HeightRequest(429)
                    .StrokeCornerRadius(0, 0, 24, 24)
                    .VStart()
                    .BackgroundColor(Theme.Grey70),

                ListTypeTab()


                )



        );


    VisualNode ListTypeTab()
        => Render<double>(state =>
            Grid("50", "*,*",
                Border()
                    .GridColumnSpan(2)
                    .BackgroundColor(Theme.Grey100)
                    .StrokeCornerRadius(16),

                Border()
                    .Margin(9, 7)
                    .BackgroundColor(Color.FromArgb("#4E4E61").WithAlpha(0.2f))
                    .StrokeCornerRadius(12)
                    .TranslationX(State.ListType == HomeViewListType.Subscriptions ? 0 : (state.Value / 2))
                    .WithAnimation(easing: Easing.CubicIn, duration: 200),

                Theme.H1("Your subscriptions")
                    .TextColor(State.ListType == HomeViewListType.Subscriptions ? Theme.White : Theme.Grey30)
                    .VerticalTextAlignment(TextAlignment.Center)
                    .HorizontalTextAlignment(TextAlignment.Center)
                    .BackgroundColor(Colors.Transparent)
                    .OnTapped(()=>SetState(s => s.ListType = HomeViewListType.Subscriptions)),

                Theme.H1("Upcoming bills")
                    .TextColor(State.ListType == HomeViewListType.UpcomingBills ? Theme.White : Theme.Grey30)
                    .VerticalTextAlignment(TextAlignment.Center)
                    .HorizontalTextAlignment(TextAlignment.Center)
                    .BackgroundColor(Colors.Transparent)
                    .OnTapped(() => SetState(s => s.ListType = HomeViewListType.UpcomingBills))
                    .GridColumn(1)
                    
                )
                .OnSizeChanged(size => state.Set(_ => _ = size.Width))
                .Margin(24, 21, 24, 0)
        ); 
}
