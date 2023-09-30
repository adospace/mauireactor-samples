using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiReactor;

namespace FlyoutPageTestApp.Pages;

enum PageType
{
    Contacts,

    TodoList,

    Reminders
}

class MainPageState
{
    public PageType CurrentPageType { get; set; }
}

class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
    {
        return new FlyoutPage
        {
            DetailPage()
        }
        .Flyout(new FlyoutMenuPage()
            .OnPageSelected(pageType => SetState(s => s.CurrentPageType = pageType))
        )
        ;
    }

    VisualNode DetailPage()
    {
        return State.CurrentPageType switch
        {
            PageType.Contacts => new ContactsPage(),
            PageType.TodoList => new TodoListPage(),
            PageType.Reminders => new RemindersPage(),
            _ => throw new InvalidOperationException(),
        };
    }

}

class FlyoutMenuPage : Component
{
    private Action<PageType> _selectAction;

    public FlyoutMenuPage OnPageSelected(Action<PageType> action)
    {
        _selectAction = action;
        return this;
    }

    public override VisualNode Render()
    {
        return new ContentPage("Personal Organiser")
        {
            new CollectionView()
                .ItemsSource(Enum.GetValues<PageType>(), pageType =>
                    new Label(pageType.ToString())
                        .Margin(10,5)
                        .VCenter())
                .SelectionMode(Microsoft.Maui.Controls.SelectionMode.Single)
                .OnSelected<CollectionView, PageType>(pageType => _selectAction?.Invoke(pageType))
        };
    }
}

class ContactsPage : Component
{
    public override VisualNode Render()
        => new NavigationPage
        {
            new ContentPage("Contacts")
            {
                new Label("Contacts")
                    .VCenter()
                    .HCenter()
            }
        };
}

class TodoListPage : Component
{
    public override VisualNode Render()
        => new NavigationPage
        {
            new ContentPage("TodoList")
            {
                new Label("TodoList")
                    .VCenter()
                    .HCenter()
            }
        };
}

class RemindersPage : Component
{
    public override VisualNode Render() =>
        new NavigationPage
        {
            new ContentPage("Reminders")
            {
                new Label("Reminders")
                    .VCenter()
                    .HCenter()
            }
        };
}