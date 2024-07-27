using MauiReactor;
using MauiReactor.Shapes;
using TaskApp.Resources.Styles;
using System.Collections.Generic;
using System;
using TaskApp.Models;
using System.Linq;
using TaskApp.Framework;
using The49.Maui.BottomSheet;

namespace TaskApp.Pages;

internal class MainPageState
{
    public DateTime CurrentDate { get; set; } = DateTime.Today;
}

internal class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
     => ContentPage(
#if ANDROID
         new StatusBarBehavior()
            .StatusBarColor(ApplicationTheme.White)
            .StatusBarStyle(Theme.IsLightTheme ? CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent : CommunityToolkit.Maui.Core.StatusBarStyle.LightContent)
            ,
#endif

         Grid("80,68,*,88", "*",

             RenderHeader(),

             RenderCalendar(),

             RenderBody(),

             RenderFooter()

             )
        );


    VisualNode RenderHeader()
        => Grid("*", "Auto,*,Auto",
            Image("avatar.png"),

            Label("Good morning 👋")
                .GridColumn(1)
                .Padding(12, 0)
                .VerticalTextAlignment(TextAlignment.Center)
                .ThemeKey(ApplicationTheme.Selector.Header),

            Ellipse()
                .GridColumn(2)
                .HeightRequest(48)
                .WidthRequest(48)
                .Stroke(new MauiControls.SolidColorBrush(ApplicationTheme.LightGray))
                .StrokeThickness(1),

            Image("bell.png")
                .GridColumn(2)
                .Center()
            )
            .Padding(20, 24);

    ScrollView RenderCalendar()
    {
        var minDate = TodoItem.All.Min(_ => _.TimeStamp).Date.AddDays(-10);
        var maxDate = TodoItem.All.Max(_ => _.TimeStamp).Date.AddDays(+10);

        IEnumerable<DateTime> EnumerateDates()
        {
            var currentDate = minDate;
            while (currentDate <= maxDate)
            {
                yield return currentDate;
                currentDate = currentDate.AddDays(1);
            }
        }

        return HScrollView(
            HStack(
                EnumerateDates()
                    .Select(RenderCalendarItem)
                    .ToArray()
            )
        )
        .GridRow(1);
    }

    VStack RenderCalendarItem(DateTime day, int index)
    {
        var hasItems = TodoItemGroup.All.Any(_ => _.Date == day);
        return VStack(spacing: 2,
            Label(day.Day)
                .ThemeKey(ApplicationTheme.Selector.CalendarItem)
                .WidthRequest(45)
                .HorizontalTextAlignment(TextAlignment.Center)
                .HeightRequest(32)
                .TextColor(day == State.CurrentDate ? ApplicationTheme.Accent : (!hasItems ? ApplicationTheme.Gray : ApplicationTheme.Black)),

            Label(day.ToString("ddd"))
                .ThemeKey(ApplicationTheme.Selector.Body)
                .HorizontalTextAlignment(TextAlignment.Center)
                .VerticalTextAlignment(TextAlignment.End)
                .HeightRequest(16)
                .TextColor(day == State.CurrentDate ? ApplicationTheme.Accent : (!hasItems ? ApplicationTheme.Gray : ApplicationTheme.Black)),

            day == State.CurrentDate ?
                Border()
                    .HeightRequest(2)
                    .Margin(0, 10, 0, 0)
                    .BackgroundColor(ApplicationTheme.Accent) : null
            )
            .OnTapped(() => SetState(s => s.CurrentDate = day))
            ;
    }


    CollectionView RenderBody()
    {
        return CollectionView()
            .IsGrouped(true)
            .ItemsSource<CollectionView, TodoItemGroup, TodoItem>(
                TodoItemGroup.All,
                RenderTodoItem,
                RenderTodoItemGroupHeader,
                RenderTodoItemGroupFooter)
            .GridRow(2);
    }

    Grid RenderTodoItem(TodoItem item)
    {
        return Grid("*", "24,*,Auto",
            Border()
                .HeightRequest(1)
                .VStart()
                .GridColumnSpan(3)
                .BackgroundColor(ApplicationTheme.LightGray),

            Border()
                .HeightRequest(24)
                .WidthRequest(24)
                .BackgroundColor(item.IsDone ? ApplicationTheme.DarkGray : ApplicationTheme.MediumGray)
                .StrokeCornerRadius(6)
                .OnTapped(() => SetState(s => item.IsDone = !item.IsDone))
                .Margin(0, 12),

            Label(item.Task)
                .TextDecorations(item.IsDone ? TextDecorations.Strikethrough : TextDecorations.None)
                .GridColumn(1)
                .VerticalTextAlignment(TextAlignment.Center)
                .Margin(12, 12),

            HStack(spacing: 4,
                Border()
                    .HeightRequest(8)
                    .WidthRequest(8)
                    .StrokeCornerRadius(2)
                    .Stroke(new MauiControls.SolidColorBrush(Utils.GetColorForList(item.List))
                ),

                Label(item.List)
                    .ThemeKey(ApplicationTheme.Selector.Body)
                )
                .VCenter()
                .GridColumn(2)
                .Margin(0, 12)

            )
            .Margin(20, 0);
    }

    HStack RenderTodoItemGroupHeader(TodoItemGroup group)
    {
        return HStack(spacing: 8,
            Label(Utils.GetHumanFriendlyDate(group.Date))
                .ThemeKey(ApplicationTheme.Selector.Body),

            Ellipse()
                .HeightRequest(4)
                .WidthRequest(4)
                .BackgroundColor(ApplicationTheme.LightGray)
                .VCenter()
                ,

            Label(group.Date.ToString("dddd"))
                .ThemeKey(ApplicationTheme.Selector.Body)

            )
        .Margin(20, 8);
    }

    Border RenderTodoItemGroupFooter(TodoItemGroup group)
    {
        return Border()
            .HeightRequest(1)
            .VStart()
            .BackgroundColor(ApplicationTheme.LightGray);
    }

    Grid RenderFooter()
    {
        return Grid("*", "*",
            HStack(spacing: 2,
                ImageButton("settings.png")
                    .WidthRequest(48)
                    .BackgroundColor(Colors.Transparent)
                    .OnClicked(OpenOptionsPane)
                    ,

                ImageButton("search.png")
                    .WidthRequest(48)
                    .BackgroundColor(Colors.Transparent)
                    .OnClicked(async () =>
                    {
                        if (Navigation != null)
                        {
                            await Navigation.PushAsync<SearchPage>();
                        }
                    })
            ),

            ImageButton("plus.png")
                .WidthRequest(48)
                .CornerRadius(14)
                .ThemeKey(ApplicationTheme.Selector.Primary)
                .HEnd()
                .Margin(8, 0)
                .OnClicked(OpenNewTaskPane)
            )
            .Padding(8, 20)
            .GridRow(3);
    }

    async void OpenOptionsPane()
    {
        await BottomSheetManager.ShowAsync(
            () => new Components.OptionsPane()
                .OnOpenSettingsPage(async () =>
                {
                    if (Navigation != null)
                    {
                        await Navigation.PushAsync<SettingsPage>();
                    }
                })
                .OnOpenStatisticsPage(async () =>
                {
                    if (Navigation != null)
                    {
                        await Navigation.PushAsync<StatisticsPage>();
                    }
                }),
            sheet =>
            {
                sheet.HasBackdrop = true;
                sheet.HasHandle = true;
                sheet.CornerRadius = 16;
                sheet.Detents =
                [
                    new ContentDetent(),
                    new FullscreenDetent()
                ];
            }
        );
    }

    async void OpenNewTaskPane()
    {
        await BottomSheetManager.ShowAsync(
            () => new Components.NewTaskPane()
                .OnNewTask(newTask =>
                {
                    TodoItemGroup.AddItem(newTask);
                    Invalidate();
                }),
            sheet =>
            {
                sheet.HasBackdrop = true;
                sheet.HasHandle = true;
                sheet.CornerRadius = 16;
                sheet.Detents =
                [
                    new ContentDetent(),
                    new FullscreenDetent()
                ];
            }
        );
    }
}