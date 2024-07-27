using MauiReactor;
using TaskApp.Framework;
using TaskApp.Models;
using TaskApp.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Pages.Components;

class OptionsPaneState
{
    public Dictionary<TodoList, int> GroupedItemsByList { get; set; } = [];
}

partial class OptionsPane : Component<OptionsPaneState>
{
    [Prop]
    Action? _onOpenStatisticsPage;

    [Prop]
    Action? _onOpenSettingsPage;

    protected override void OnMounted()
    {
        State.GroupedItemsByList = TodoItem.All
            .GroupBy(_ => _.List)
            .ToDictionary(_ => _.Key, _ => _.Count());
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return VStack(

            RenderOptionsMenuItem("statistics.svg", "Statistics", _onOpenStatisticsPage),

            RenderOptionsMenuItem("gear.svg", "Settings", _onOpenSettingsPage),

            Label("Lists")
                .ThemeKey(ApplicationTheme.Selector.Body)
                .Margin(0, 8),

            CollectionView()
                .ItemsSource(
                    Enum.GetValues<TodoList>()
                        .Where(_ => State.GroupedItemsByList.ContainsKey(_)),
                    RenderTodoListItem)
                .MaximumHeightRequest(400)

        )
        .Padding(16)
        .BackgroundColor(ApplicationTheme.White);
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
        );
    }

    Grid RenderOptionsMenuItem(string icon, string text, Action? openAction)
    {
        return Grid("*", "24,*,Auto",
            Button()
                .GridColumnSpan(3)
                .BackgroundColor(Colors.Transparent)
                .BorderWidth(0)
                .OnClicked(async () =>
                {
                    await BottomSheetManager.DismissAsync();

                    openAction?.Invoke();
                }),

            Border()
                .HeightRequest(1)
                .VEnd()
                .GridColumnSpan(3)
                .BackgroundColor(ApplicationTheme.LightGray),

            Image(icon)
                .HeightRequest(24)
                .WidthRequest(24)
                .Margin(0, 12),

            Label(text)
                .GridColumn(1)
                .VerticalTextAlignment(TextAlignment.Center)
                .Margin(12, 12)
        );
    }

}
