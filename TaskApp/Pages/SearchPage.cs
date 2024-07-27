using MauiReactor;
using TaskApp.Framework;
using TaskApp.Models;
using TaskApp.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Pages;

class SearchPageState
{
    public string SearchString { get; set; } = string.Empty;

    public List<TodoItem> FilteredItems { get; set; } = [];

    public Dictionary<TodoList, int> GroupedItemsByList { get; set; } = [];


}

partial class SearchPage : Component<SearchPageState>
{
    protected override void OnMounted()
    {
        State.FilteredItems = TodoItem.All;
        State.GroupedItemsByList = TodoItem.All
            .GroupBy(_ => _.List)
            .ToDictionary(_ => _.Key, _ => _.Count());


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

            Grid("64,*", "*",
                RenderHeader(),

                RenderBody()
                    .GridRow(1)
            ));
    }

    Grid RenderHeader()
        => Grid("*", "Auto,*,Auto",
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

            Entry()
                .Placeholder("Search task or list")
                .OnTextChanged(text => SetState(s =>
                {
                    s.SearchString = text;
                    s.FilteredItems = string.IsNullOrWhiteSpace(State.SearchString) ? TodoItem.All : TodoItem.All.Where(_ => _.Task.Contains(State.SearchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    s.GroupedItemsByList = s.FilteredItems.GroupBy(_ => _.List).ToDictionary(_ => _.Key, _ => _.Count());
                }))
                .GridColumn(1)
                .Margin(12, 0)
                .VCenter(),

            Border()
                .HeightRequest(1)
                .VEnd()
                .GridColumnSpan(3)
                .BackgroundColor(ApplicationTheme.LightGray)
        );

    VStack RenderBody()
    {
        return VStack(spacing: 4,
            Label("Tasks")
                .ThemeKey(ApplicationTheme.Selector.Body)
                .Margin(20, 8),

            CollectionView()
                .ItemsSource(
                    State.FilteredItems,
                    RenderTodoItem)
                .MaximumHeightRequest(400),

            Label("Lists")
                .ThemeKey(ApplicationTheme.Selector.Body)
                .Margin(20, 8),

            CollectionView()
                .ItemsSource(
                    Enum.GetValues<TodoList>()
                        .Where(_ => State.GroupedItemsByList.ContainsKey(_)),
                    RenderTodoListItem)
                .MaximumHeightRequest(400)
            );
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

                Label(item.List.ToString())
                    .ThemeKey(ApplicationTheme.Selector.Body)
                )
                .VCenter()
                .GridColumn(2)
                .Margin(0, 12)

            )
            .Margin(20, 0);
    }

    Grid RenderTodoListItem(TodoList item)
    {
        State.GroupedItemsByList.TryGetValue(item, out var itemsCount);

        return Grid("*", "24,*,Auto",
            Border()
                .HeightRequest(1)
                .VStart()
                .GridColumnSpan(3)
                .BackgroundColor(ApplicationTheme.LightGray),

            Border()
                .HeightRequest(24)
                .WidthRequest(24)
                .BackgroundColor(ApplicationTheme.MediumGray)
                .StrokeCornerRadius(6)
                .Margin(0, 12),

            Border()
                .HeightRequest(16)
                .WidthRequest(16)
                .StrokeCornerRadius(4)
                .Stroke(new MauiControls.SolidColorBrush(Utils.GetColorForList(item))),

            Label(item.ToString())
                .GridColumn(1)
                .VerticalTextAlignment(TextAlignment.Center)
                .Margin(12, 12),

            Label($"{itemsCount} items")
                .ThemeKey(ApplicationTheme.Selector.Body)
                .VCenter()
                .GridColumn(2)
        )
        .Margin(20, 0);
    }
}
