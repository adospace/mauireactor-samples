using MauiReactor;
using MauiReactor.Canvas;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackizerApp.Models;

namespace TrackizerApp.Pages.Views;

record CalendarItem(int Day, bool Current, bool HasSubscriptions);

class CalendarViewState
{
    public int SelectedMonth { get; set; }

    public List<CalendarItem> Items { get; set; } = [];

    public ObservableCollection<Subscription> Subscriptions { get; set; } = [
        new Subscription(SubscriptionType.Spotify, 5.99, DateOnly.FromDateTime(DateTime.Now)),
        new Subscription(SubscriptionType.YouTube, 18.99, DateOnly.FromDateTime(DateTime.Now.AddDays(-2))),
        new Subscription(SubscriptionType.OneDrive, 29.99, DateOnly.FromDateTime(DateTime.Now.AddDays(-5))),
        new Subscription(SubscriptionType.Netflix, 9.99, DateOnly.FromDateTime(DateTime.Now.AddDays(-7)))
        ];
}


class CalendarView : Component<CalendarViewState>
{
    static List<string> _allMonths = [.. Enumerable.Range(0, 11).Select(_ => new DateTime().AddMonths(_).ToString("MMMM"))];

    protected override void OnMountedOrPropsChanged()
    {
        State.SelectedMonth = DateTime.Now.Month;
        State.Items = Enumerable.Range(0, 28).Select(day => new CalendarItem(day + 1, day == DateTime.Now.Day, day == 8)).ToList();

        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
        => Grid("Auto,*","*",
            TopCalendar(),

            Grid("Auto,Auto","*",
                Theme.H4(new DateTime(DateTime.Now.Year, State.SelectedMonth, 1).ToString("MMMM"))
                    .TextColor(Theme.White),

                Theme.H4("$24.99")
                    .TextColor(Theme.White)
                    .FontAttributes(MauiControls.FontAttributes.Bold)
                    .HEnd(),

                Theme.H1(new DateTime(DateTime.Now.Year, State.SelectedMonth, 1).ToString("dd.MM.yyyy"))
                    .GridRow(1)
                    .TextColor(Theme.Grey30),

                Theme.H1("in upcoming bills")
                    .GridRow(1)
                    .HEnd()
                    .TextColor(Theme.Grey30)
            )
            .Margin(24, 410,24,0),


            CollectionView()
                .ItemsLayout(new VerticalGridItemsLayout().Span(2).HorizontalItemSpacing(8).VerticalItemSpacing(8))
                .ItemsSource(State.Subscriptions, subscription =>
                    Border(
                        Grid(
                            Image($"{subscription.Type}.png")
                                .WidthRequest(40)
                                .HStart()
                                .VStart(),
                            VStack(spacing: 5,
                                Theme.H2(subscription.Type.GetDisplayName())
                                    .TextColor(Theme.White),
                                Theme.H4($"${subscription.MonthBill}")
                                    .FontAttributes(MauiControls.FontAttributes.Bold)
                                    .TextColor(Theme.White)
                                )
                                .VEnd()
                                .HStart()
                            )
                            .Padding(16)
                        )
                        .HeightRequest(168)
                        .StrokeCornerRadius(16)
                        .StrokeThickness(0.5)
                        .BackgroundColor(Theme.Grey60.WithAlpha(0.2f))
                        .Stroke(new MauiControls.LinearGradientBrush(
                            [
                                new MauiControls.GradientStop(Color.FromArgb("#CFCFFC"), 0.0f),
                                new MauiControls.GradientStop(Colors.Transparent, 1.0f)
                            ], new Point(0.0, 0.5), new Point(1.0, 1.0))))
                .Margin(24, 24, 24, 80)
                .GridRow(1),

            Border()
                .GridRow(1)
                .VEnd()
                .HeightRequest(90)
                .Background(new LinearGradient(0, (Theme.Grey80, 0.7f), (Colors.Transparent, 1.0f)))
        );

    Border TopCalendar()
        => Border(
                Grid(
                    Theme.H7("Subs Schedule")
                        .TextColor(Theme.White)
                        .Margin(24, 98)
                        .WidthRequest(184)
                        .HStart()
                        .FontAttributes(MauiControls.FontAttributes.Bold),

                    Grid(
                        Theme.H2("3 subscriptions for today")
                            .TextColor(Theme.White)
                            .VCenter(),

                        Border(
                            HStack(spacing: 6,
                                Picker()
                                    .ItemsSource(_allMonths)
                                    .SelectedIndex(State.SelectedMonth - 1)
                                    .TextColor(Theme.White)
                                    .OnSelectedIndexChanged(index => SetState(s => s.SelectedMonth = index + 1)),
                                Image("arrow_down.png")
                                    .WidthRequest(12)
                            )
                        )
                        .StrokeThickness(0)
                        .HEnd()
                    )
                    .VStart()
                    .Margin(24, 200),


                    CollectionView()
                        .ItemsLayout(new HorizontalLinearItemsLayout())
                        .ItemsSource(State.Items, item =>
                            Border(
                                Grid(
                                    Theme.H4(item.Day.ToString("00"))
                                        .TextColor(Theme.White)
                                        .FontAttributes(MauiControls.FontAttributes.Bold)
                                        .HCenter()
                                        .Margin(10, 8),
                                    Theme.H1(new DateTime(DateTime.Now.Year, State.SelectedMonth, item.Day).ToString("ddd")[..2])
                                        .TextColor(Theme.Grey30)
                                        .HCenter()
                                        .Margin(10, 38),

                                    Ellipse()
                                        .HeightRequest(6)
                                        .WidthRequest(6)
                                        .Fill(Theme.Accentp100)
                                        .VEnd()
                                        .Margin(10, 16)
                                        .IsVisible(item.HasSubscriptions)
                                    )
                                )
                                .StrokeCornerRadius(16)
                                .StrokeThickness(0.5)
                                .Stroke(new MauiControls.LinearGradientBrush(
                                    [
                                        new MauiControls.GradientStop(Color.FromArgb("#CFCFFC"), 0.0f),
                                        new MauiControls.GradientStop(Colors.Transparent, 1.0f)
                                    ], new Point(0.0, 0.5), new Point(1.0, 1.0)))
                                .BackgroundColor(item.Current ? Theme.Grey60 : Theme.Grey60.WithAlpha(0.2f))
                                .WidthRequest(48)
                                .Margin(0, 0, 8, 0)
                            )
                        .VEnd()
                        .HeightRequest(105)
                        .Margin(24, 32)
                    )
                )
                .HeightRequest(386)
                .StrokeCornerRadius(0, 0, 24, 24)
                .VStart()
                .StrokeThickness(0)
                .BackgroundColor(Theme.Grey70);
}
