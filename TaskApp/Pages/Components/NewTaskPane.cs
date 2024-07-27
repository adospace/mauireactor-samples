using CommunityToolkit.Maui.Core.Platform;
using MauiReactor;
using TaskApp.Framework;
using TaskApp.Models;
using TaskApp.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskApp.Pages.Components;
class NewTaskPaneState
{
    public string? Task { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Today;

    public TodoList TodoList { get; set; }

    public bool ShowCalendar { get; set; }
}

partial class NewTaskPane : Component<NewTaskPaneState>
{
    MauiControls.Entry? _entry;

    [Prop]
    Action<TodoItem>? _onNewTask;

    public override VisualNode Render()
    {
        return VStack(
            Entry(entry => _entry = entry)
                .Placeholder("Write your task")
                .OnTextChanged(text => SetState(s => s.Task = text)),

            Grid("40", "Auto,*,40",

                RenderCalendar(),

                RenderTodoList()
                    .GridColumn(1),

                ImageButton("send.png")
                    .IsEnabled(!string.IsNullOrWhiteSpace(State.Task))
                    .ThemeKey(ApplicationTheme.Selector.Primary)
                    .GridColumn(2)
                    .OnClicked(CreateNewTask)
            )
            .ColumnSpacing(8)
        )
        .Padding(16)
        .BackgroundColor(ApplicationTheme.White);

    }

    VisualNode RenderCalendar()
    {
        if (!State.ShowCalendar)
        {
            return Grid(
                Button("Today", () => SetState(s => s.ShowCalendar = true))
                    .Padding(40, 10, 12, 10)
                    .FontSize(12),

                Image("calendar.png")
                    .Margin(12, 10)
                    .HStart()
                );
        }

        return Border(
            DatePicker()
                .OnDateSelected(date => SetState(s => s.TimeStamp = date))
                .TextColor(ApplicationTheme.DarkGray)
            )
            .Padding(12, 0)
            .StrokeCornerRadius(10)
            .BackgroundColor(ApplicationTheme.MediumGray);
    }

    ScrollView RenderTodoList()
    {
        return HScrollView(
            HStack(spacing: 8,
                Enum.GetValues<TodoList>()
                .Select(item => Border(
                    HStack(spacing: 8,
                        Border()
                            .HeightRequest(16)
                            .WidthRequest(16)
                            .StrokeCornerRadius(2)
                            .Stroke(new MauiControls.SolidColorBrush(Utils.GetColorForList(item))
                        ),

                        Label(item)
                            .ThemeKey(ApplicationTheme.Selector.Normal)
                            .TextColor(State.TodoList == item ? ApplicationTheme.MediumGray : ApplicationTheme.DarkGray)
                        )
                        .GridColumn(2)
                        .Margin(12, 10)
                    )
                    .StrokeCornerRadius(10)
                    .BackgroundColor(State.TodoList == item ? ApplicationTheme.DarkGray : ApplicationTheme.MediumGray)
                    .OnTapped(() => SetState(s => s.TodoList = item))
                )
                .ToArray()
                )
            );
    }

    async void CreateNewTask()
    {
        if (_entry != null)
        {
            await _entry.HideKeyboardAsync(CancellationToken.None);
        }

        await BottomSheetManager.DismissAsync();

        TodoItem newItem = new(State.Task!, State.TimeStamp, State.TodoList);

        _onNewTask?.Invoke(newItem);
    }

}
