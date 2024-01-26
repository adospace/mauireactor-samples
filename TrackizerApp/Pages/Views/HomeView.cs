using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Shapes;
using MauiReactor.Canvas;
using System.Collections.ObjectModel;
using TrackizerApp.Models;

namespace TrackizerApp.Pages.Views;


enum HomeViewListType
{
    Subscriptions,

    UpcomingBills
}

class HomeViewState
{
    public HomeViewListType ListType { get; set; }

    public ObservableCollection<Subscription> Subscriptions { get; set; } = [
        new Subscription(SubscriptionType.Spotify, 5.99, DateOnly.FromDateTime(DateTime.Now)),
        new Subscription(SubscriptionType.YouTube, 18.99, DateOnly.FromDateTime(DateTime.Now.AddDays(-2))),
        new Subscription(SubscriptionType.OneDrive, 29.99, DateOnly.FromDateTime(DateTime.Now.AddDays(-5))),
        new Subscription(SubscriptionType.Netflix, 9.99, DateOnly.FromDateTime(DateTime.Now.AddDays(-7)))
        ];

    public double TargetMonthBills { get; set; } = 1235;

    public bool IsVisible { get; set; }
}


partial class HomeView : Component<HomeViewState>
{
    [Prop]
    Action? _onShowBudgetView;

    [Prop]
    bool _isVisible;

    protected override void OnPropsChanged()
    {
        //simulate calculations, remote call etc
        if (State.TargetMonthBills != 0 &&
            State.IsVisible != _isVisible &&
            _isVisible)
        {
            State.TargetMonthBills = 0;
            SetState(s => s.TargetMonthBills = 1235, delayMilliseconds: 500);
        }

        State.IsVisible = _isVisible;

        base.OnPropsChanged();
    }

    public override VisualNode Render()
        => Grid("Auto,*", "*",
            VStack(
                BudgetIndicator(2230, State.TargetMonthBills),

                ListTypeTab()
                ),

            Grid(
                Subscriptions()
                    .IsVisible(() => State.ListType == HomeViewListType.Subscriptions)
                    .Margin(24, 8, 24, 72),

                UpcomingBills()
                    .IsVisible(() => State.ListType == HomeViewListType.UpcomingBills)
                    .Margin(24, 8, 24, 72)
            )
            .GridRow(1),

            Border()
                .GridRow(1)
                .VEnd()
                .HeightRequest(90)
                .Background(new LinearGradient(0, (Theme.Grey80, 0.7f), (Colors.Transparent, 1.0f)))
        );

    VisualNode BudgetIndicator(double monthBudget, double monthBills)
        => Render<(double Width, double AnimatingMonthBills)>(state 
            => Grid(
                Border()
                    .HeightRequest(429)
                    .StrokeCornerRadius(0, 0, 24, 24)
                    .VStart()
                    .StrokeThickness(0)
                    .BackgroundColor(Theme.Grey70),

                    new CanvasView()
                    {
                        new Group
                        {
                            new Arc()
                                .StrokeColor(Theme.Grey60)
                                .StrokeSize(2)
                                .StrokeDashPattern([0.5f, 10.0f])
                                .StrokeLineCap(LineCap.Round)
                                .StartAngle(-180 + 40)
                                .EndAngle(-40)
                                .Clockwise(true)
                                .Margin(10),

                            new Arc()
                                .StrokeColor(Theme.Grey30)
                                .StrokeSize(12)
                                .StrokeLineCap(LineCap.Round)
                                .StartAngle(-180 + 40)
                                .EndAngle(-40)
                                .Clockwise(true)
                                .Margin(30),

                            new Arc()
                                .StrokeColor(Theme.Grey60)
                                .StrokeSize(2)
                                .StrokeDashPattern([0.5f, 10.0f])
                                .StrokeLineCap(LineCap.Round)
                                .StartAngle(-180 + 40)
                                .EndAngle(-40)
                                .Clockwise(true)
                                .Margin(50),

                            new DropShadow
                            {
                                new Arc()
                                    .StrokeColor(Theme.Accentp100)
                                    .StrokeSize(12)
                                    .StrokeLineCap(LineCap.Round)
                                    .StartAngle(-180 + 40)
                                    .EndAngle(() => -180 + 40 - (float)Math.Min(180 + 80, (state.Value.AnimatingMonthBills / monthBudget) * (180 + 80)))
                                    .Clockwise(true)
                                    .Margin(30)
                            }
                            .Color(Theme.White.WithLuminosity(0.7f))
                            .Blur(10),
                        }
                        .Margin(10)
                        .TranslationY(-20)
                    }
                    .WidthRequest(state.Value.Width)
                    .HeightRequest(state.Value.Width)
                    .BackgroundColor(Colors.Transparent),

                Image("full_logo.png")
                    .WidthRequest(107)
                    .VStart()
                    .HCenter()
                    .Margin(0,110),

                Theme.H7()
                    .Text(()=> $"${state.Value.AnimatingMonthBills:0}")
                    .VStart()
                    .HCenter()
                    .TextColor(Theme.White)
                    .Margin(0,140)
                    .OnTapped(() => state.Set(s => (s.Width, 0))),

                Theme.H1("This month bills")
                    .VCenter()
                    .HCenter()
                    .TextColor(Theme.Grey40)
                    .Margin(0, 200),

                Theme.Button("See your budget", _onShowBudgetView)
                    .TextColor(Theme.White)
                    .HCenter()
                    .VEnd()
                    .Margin(110)
                    .HeightRequest(38),

                Grid("68", "* * *",
                    BudgetItem(Theme.Accentp100, "Active subs", "12"),
                    BudgetItem(Theme.Primary100, "Highest subs", "$19.99").GridColumn(1),
                    BudgetItem(Theme.Accents50, "Lowest subs", "$5.99").GridColumn(2)
                )
                .VEnd()
                .Margin(24)
                .ColumnSpacing(8),

                new AnimationController
                {
                    new ParallelAnimation
                    {
                        new DoubleAnimation()
                            .StartValue(0)
                            .TargetValue(monthBills)
                            .Duration(1000)
                            .Easing(Easing.CubicOut)
                            .OnTick(v => state.Set(s => (s.Width, v), false))
                    }
                }
                .IsEnabled(state.Value.AnimatingMonthBills != monthBills)
            )
            .OnSizeChanged(size => state.Set(_ => (size.Width, _.AnimatingMonthBills)))
        );

    Border BudgetItem(Color topColor, string topText, string bottomText)
        => Border(
            Grid("1,*,*", "*",
                    Border()
                        .BackgroundColor(topColor)
                        .HeightRequest(1)
                        .Margin(30, 0),
                    Theme.H1(topText)
                        .TextColor(Theme.Grey40)
                        .VEnd()
                        .HCenter()
                        .GridRow(1),
                    Theme.H2(bottomText)
                        .TextColor(Theme.White)
                        .VStart()
                        .HCenter()
                        .HorizontalTextAlignment(TextAlignment.Center)
                        .GridRow(2)
                )
            )
            .StrokeCornerRadius(16)
            .StrokeThickness(0.5)
            .Stroke(new MauiControls.LinearGradientBrush(
                [
                    new MauiControls.GradientStop(Color.FromArgb("#CFCFFC"), 0.0f),
                    new MauiControls.GradientStop(Colors.Transparent, 1.0f)
                ], new Point(), new Point(0.5, 1.0)))
            .BackgroundColor(Theme.Grey60);

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

    CollectionView Subscriptions()
        => CollectionView()
                .ItemsSource(State.Subscriptions, (subscription) => 
                    Border(
                        Grid("*", "64,*,Auto",
                            Image($"{subscription.Type.ToString().ToLower()}.png")
                                .HeightRequest(40)
                                .Center(),

                            Theme.H2($"{subscription.Type.GetDisplayName()}")
                                .GridColumn(1)
                                .TextColor(Theme.White)
                                .VCenter(),

                            Theme.H2($"${subscription.MonthBill}")
                                .GridColumn(2)
                                .VCenter()
                                .TextColor(Theme.White)
                                .Margin(17)
                        )
                    )
                    .Margin(0,8)
                    .Stroke(Theme.Grey60)
                    .StrokeThickness(1)
                    .StrokeCornerRadius(16)
                    .HeightRequest(64)
                )
            .Margin(24,8);

    CollectionView UpcomingBills()
        => CollectionView()
                .ItemsSource(State.Subscriptions, (subscription) =>
                    Border(
                        Grid("*", "64,*,Auto",
                            Border(
                                VStack(
                                    Theme.BodyExtraSmall($"{subscription.StartingDate:MMM}").TextColor(Theme.Grey30),
                                    Theme.BodyMedium($"{subscription.StartingDate:dd}").TextColor(Theme.Grey30).HCenter()
                                    )
                                    .Center()
                                )
                                .Margin(12)
                                .StrokeCornerRadius(12)
                                .BackgroundColor(Theme.Grey70)
                                    ,

                            Theme.H2($"{subscription.Type.GetDisplayName()}")
                                .GridColumn(1)
                                .TextColor(Theme.White)
                                .VCenter(),

                            Theme.H2($"${subscription.MonthBill}")
                                .GridColumn(2)
                                .VCenter()
                                .TextColor(Theme.White)
                                .Margin(17)
                        )
                    )
                    .Margin(0, 8)
                    .Stroke(Theme.Grey60)
                    .StrokeThickness(1)
                    .StrokeCornerRadius(16)
                    .HeightRequest(64)
                )
            .Margin(24, 8);
}
