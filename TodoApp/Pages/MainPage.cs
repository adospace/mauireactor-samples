using MauiReactor;
using ReactorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Pages;

class MainPageState
{
    public IQuery<Todo> TodoItems { get; set; } = default!;
}

partial class MainPage : Component<MainPageState>
{
    [Inject]
    IModelContext _modelContext;

    protected override void OnMounted()
    {
        State.TodoItems = _modelContext.Query<Todo>(query => query.OrderBy(_ => _.Task));

        base.OnMounted();
    }

    public override VisualNode Render()
        => ContentPage(
            Grid("Auto, *, Auto", "*",
                TodoEditor(OnCreatedNewTask),

                CollectionView()
                    .ItemsSource(State.TodoItems, RenderItem)
                    .GridRow(1),

                Button("Clear List")
                    .OnClicked(OnClearList)
                    .GridRow(2)

            ));

    

    VisualNode RenderItem(Todo item)
        => Grid("54", "Auto, *",
            CheckBox()
                .IsChecked(item.Done)
                .OnCheckedChanged((s, args) => OnItemDoneChanged(item, args.Value)),
            Label(item.Task)
                .TextDecorations(item.Done ? TextDecorations.Strikethrough : TextDecorations.None)
                .VCenter()
                .GridColumn(1));

    static VisualNode TodoEditor(Action<Todo> created)
        => Render<string>(state =>
            Grid("*", "*,Auto",
                Entry()
                    .Text(state.Value ?? string.Empty)
                    .OnTextChanged(text => state.Set(s => text, false)),
                Button("Create")
                    .GridColumn(1)
                    .OnClicked(() =>
                    {
                        created(new Todo { Task = state.Value ?? "New Task" });
                        state.Set(s => string.Empty);
                    })
                )
            );

    void OnItemDoneChanged(Todo item, bool done)
    {
        item.Done = done;

        _modelContext.Update(item);
        _modelContext.Save();
    }

    void OnCreatedNewTask(Todo todo)
    {
        _modelContext.Add(todo);
        _modelContext.Save();
    }

    void OnClearList()
    {
        _modelContext.DeleteRange(State.TodoItems);
        _modelContext.Save();
    }
}
