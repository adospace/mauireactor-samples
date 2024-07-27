using MauiReactor;
using MauiReactor.Shapes;

using TaskApp.Framework;
using TaskApp.Models;
using TaskApp.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Pages;

enum StatisticsTimeMode
{
    Weekly,
    Monthly,
    Yearly,
    All
}

class StatisticsPageState
{
    public StatisticsTimeMode Mode { get; set; }

    public List<TodoItem> FilteredItems { get; set; } = [];
}

partial class StatisticsPage : Component<StatisticsPageState>
{
    protected override void OnMounted()
    {
        UpdateFilteredItems(StatisticsTimeMode.Weekly);

        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return ContentPage(
#if ANDROID
         new StatusBarBehavior()
            .StatusBarColor(ApplicationTheme.White)
            .StatusBarStyle(Theme.IsLightTheme ? CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent : CommunityToolkit.Maui.Core.StatusBarStyle.LightContent)
            ,
#endif
            Grid("64,64,80,88", "*",
                
                RenderHeader(),

                RenderTimeNavigationBar(State.Mode, newMode =>
                {
                    UpdateFilteredItems(newMode);
                    SetState(s => s.Mode = newMode);
                }),


                RenderBody()
                    .GridRow(2)
            ));
    }

    static VisualNode RenderTimeNavigationBar(StatisticsTimeMode mode, Action<StatisticsTimeMode> onModeChanged)
        => Render<(double Width, double Height)>(state =>
        {
            Button RenderButtonMode(StatisticsTimeMode buttonMode)
                => Button(buttonMode.ToString())
                    .BackgroundColor(Colors.Transparent)
                    .When(mode == buttonMode, _ => _.TextColor(ApplicationTheme.Black))
                    .OnClicked(() => onModeChanged(buttonMode));

            return Grid("*", "*,*,*,*",
                Border()
                    .StrokeCornerRadius(10)
                    .BackgroundColor(ApplicationTheme.LightGray)
                    .GridColumnSpan(4),

                Border()
                    .BackgroundColor(ApplicationTheme.White)
                    .StrokeCornerRadius(8)
                    .Margin(4)
                    .TranslationX(state.Value.Width / 4.0 * (int)mode)
                    .WithAnimation(duration: 200),

                RenderButtonMode(StatisticsTimeMode.Weekly),
                RenderButtonMode(StatisticsTimeMode.Monthly).GridColumn(1),
                RenderButtonMode(StatisticsTimeMode.Yearly).GridColumn(2),
                RenderButtonMode(StatisticsTimeMode.All).GridColumn(3)
            )
            .OnSizeChanged(size => state.Set(s => (size.Width, size.Height)))
            .GridRow(1)
            .Margin(20, 12);
        });


    Grid RenderHeader()
        => Grid("*", "56,*,Auto",
            ImageButton("back.png")
                .BackgroundColor(Colors.Transparent)
                .HeightRequest(48)
                .WidthRequest(48)
                .OnClicked(async () =>
                {
                    if (Navigation != null && Navigation.NavigationStack.Count > 0)
                    {
                        await Navigation.PopAsync();
                    }
                })
                .Margin(8, 0, 4, 0),

            Label("Statistics")
                .GridColumn(1)
                .VCenter()
                .ThemeKey(ApplicationTheme.Selector.Header),


            Border()
                .HeightRequest(1)
                .VEnd()
                .GridColumnSpan(3)
                .BackgroundColor(ApplicationTheme.LightGray)
        );


    Grid RenderBody()
        => Grid("56, 20, Auto, Auto", "*,*",
            Label()
                .ThemeKey(ApplicationTheme.Selector.Header)
                .FormattedText(new MauiControls.FormattedString
                {
                    Spans =
                    {
                        new MauiControls.Span { Text = $"{State.FilteredItems.Count(_=>_.IsDone)}", TextColor = ApplicationTheme.Black, FontSize=36 },
                        new MauiControls.Span { Text = $"/{State.FilteredItems.Count}", TextColor = ApplicationTheme.DarkGray, FontSize=20 },
                    }
                }),

            Label("Tasks completed")
                .TextColor(ApplicationTheme.DarkGray)
                .GridRow(1),


            Label()
                .ThemeKey(ApplicationTheme.Selector.Header)
                .FormattedText(new MauiControls.FormattedString
                {
                    Spans =
                    {
                        new MauiControls.Span { Text = $"{State.FilteredItems.Count(_=>_.IsDone)/(double)State.FilteredItems.Count*100:0}", TextColor = ApplicationTheme.Black, FontSize=36 },
                        new MauiControls.Span { Text = $"%", TextColor = ApplicationTheme.DarkGray, FontSize=20 },
                    }
                })
                .GridColumn(1),

            Label("Completion rate")
                .TextColor(ApplicationTheme.DarkGray)
                .GridRow(1)
                .GridColumn(1),


            RenderChart(State.FilteredItems),


            RenderStatistics()
                .GridRow(3)
                .GridColumnSpan(2)

            )
        .Margin(20, 4);

    static VisualNode RenderChart(List<TodoItem> items)
        => Render<(double Width, double Height)>(state =>
        {
            var counts = items
                .GroupBy(_ => _.List)
                .Where(_ => _.Any())
                .ToDictionary(_ => _.Key, _ => new
                {
                    Count = _.Count(),
                    CountDone = _.Count(x => x.IsDone)
                });

            int tickDistance = counts.Max(_ => _.Value.Count) / 5;
            int maxCount = (counts.Max(_ => _.Value.Count) / tickDistance + 1) * tickDistance;

            Line RenderVerticalLine(int lineIndex)
                => Line()
                    .Stroke(ApplicationTheme.LightGray)
                    .StrokeDashOffset(0)
                    .StrokeDashArray(new MauiControls.DoubleCollection([6, 6]))
                    .X1(lineIndex * state.Value.Width / 5)
                    .Y1(1)
                    .X2(lineIndex * state.Value.Width / 5)
                    .Y2(state.Value.Height - 1);

            return VStack(spacing: 20,
                Grid(
                    VStack(spacing: 12,
                        [..
                        Enum.GetValues<TodoList>()
                            .Reverse()
                            .Where(_=>counts.ContainsKey(_))
                            .Select(mode => Grid(
                                Border()
                                    .StrokeCornerRadius(5)
                                    .BackgroundColor(Utils.GetColorForList(mode))
                                    .HStart()
                                    .WidthRequest(state.Value.Width *
                                        (counts[mode].Count / (double)maxCount) *
                                        (counts[mode].CountDone / (double)counts[mode].Count))
                                    .WithAnimation(),

                                Border()
                                    .StrokeCornerRadius(5)
                                    .BackgroundColor(Utils.GetColorForList(mode).WithAlpha(0.16f))
                                    .HStart()
                                    .WidthRequest(state.Value.Width *
                                        (counts[mode].Count / (double)maxCount))
                            )
                            .HeightRequest(20))
                        ]),

                    Grid(
                        [..
                        Enumerable.Range(0,6)
                            .Select(_=>RenderVerticalLine(_))
                        ]
                    ))
                )
            .OnSizeChanged(size => state.Set(s => (size.Width, size.Height)))
            .Margin(0, 20)
            .GridRow(2)
            .GridColumnSpan(2);
        }
        );

    VStack RenderStatistics()
    {
        var counts = State.FilteredItems
            .GroupBy(_ => _.List)
            .Where(_ => _.Any())
            .ToDictionary(_ => _.Key, _ => new
            {
                Count = _.Count(),
                CountDone = _.Count(x => x.IsDone)
            });

        return VStack(
            [..
            Enum.GetValues<TodoList>()
                .Reverse()
                .Where(_=>counts.ContainsKey(_))
                .Select(mode => Grid("48", "16,*,*,*",
                    Border()
                        .BackgroundColor(ApplicationTheme.LightGray)
                        .HeightRequest(1)
                        .VEnd()
                        .GridColumnSpan(4)
                        ,

                    Border()
                        .HeightRequest(16)
                        .Margin(0,16)
                        .StrokeCornerRadius(4)
                        .Stroke(Utils.GetColorForList(mode)),

                    Label(mode)
                        .VCenter()
                        .Margin(12,0)
                        .GridColumn(1),

                    Label()
                        .Center()
                        .FormattedText(new MauiControls.FormattedString
                        {
                            Spans =
                            {
                                new MauiControls.Span { Text = counts[mode].CountDone.ToString(), TextColor = ApplicationTheme.Black, FontSize=14 },
                                new MauiControls.Span { Text = $"/{counts[mode].CountDone}", TextColor = ApplicationTheme.DarkGray, FontSize=10 },
                            }
                        })
                        .Margin(12,0)
                        .GridColumn(2),

                    Label()
                        .VCenter()
                        .HEnd()
                        .FormattedText(new MauiControls.FormattedString
                        {
                            Spans =
                            {
                                new MauiControls.Span { Text = $"{counts[mode].CountDone / (double)counts[mode].Count * 100:0}", TextColor = ApplicationTheme.Black, FontSize=14 },
                                new MauiControls.Span { Text = $"%", TextColor = ApplicationTheme.DarkGray, FontSize=10 },
                            }
                        })
                        .Margin(12,0)
                        .GridColumn(4)
                ))
            ]);
    }



    void UpdateFilteredItems(StatisticsTimeMode mode)
    {
        var baseDate = DateTime.Today;

        switch (mode)
        {
            case StatisticsTimeMode.Weekly:
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                State.FilteredItems = TodoItem.All.Where(_ => _.TimeStamp >= thisWeekStart && _.TimeStamp <= thisWeekEnd).ToList();
                break;
            case StatisticsTimeMode.Monthly:
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
                State.FilteredItems = TodoItem.All.Where(_ => _.TimeStamp >= thisMonthStart && _.TimeStamp <= thisMonthEnd).ToList();
                break;
            case StatisticsTimeMode.Yearly:
                var thisYearStart = new DateTime(baseDate.Year, 1, 1);
                var thisYearEnd = new DateTime(baseDate.Year, 12, 31);
                State.FilteredItems = TodoItem.All.Where(_ => _.TimeStamp >= thisYearStart && _.TimeStamp <= thisYearEnd).ToList();
                break;
            case StatisticsTimeMode.All:
                State.FilteredItems = TodoItem.All;
                break;
        }
    }

}
