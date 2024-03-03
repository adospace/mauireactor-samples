using MauiReactor;
using System;
using System.Collections.ObjectModel;

namespace CollectionViewTestApp.Pages;

class MainPageSwipeState
{
    public ObservableCollection<Person> Persons { get; set; }
}

class MainPageSwipe : Component<MainPageSwipeState>
{
    protected override void OnMounted()
    {
        var person1 = new Person("John", "Doe", new DateTime(1980, 5, 10));
        var person2 = new Person("Jane", "Smith", new DateTime(1990, 6, 20));
        var person3 = new Person("Alice", "Johnson", new DateTime(1985, 7, 30));
        var person4 = new Person("Bob", "Williams", new DateTime(2000, 8, 15));
        var person5 = new Person("Charlie", "Brown", new DateTime(1995, 9, 25));

        State.Persons = new ObservableCollection<Person>(new[] { person1, person2, person3, person4, person5 });
        base.OnMounted();
    }

    public override VisualNode Render()
        => ContentPage(
            CollectionView()
                .ItemsSource(State.Persons, RenderPerson)
                );

    private VisualNode RenderPerson(Person person) 
        => SwipeView(
            VStack(spacing: 5,
                Label($"{person.FirstName} {person.LastName}"),
                Label(person.DateOfBirth.ToShortDateString())
                    .FontSize(12)
            )
            .VCenter()
        )
        .LeftItems(
        [
            SwipeItem()
                .IconImageSource("archive.png")
                .Text("Archive")
                .BackgroundColor(Colors.Green),
            SwipeItem()
                .IconImageSource("delete.png")
                .Text("Delete")
                .BackgroundColor(Colors.Red)
                .OnInvoked(()=>State.Persons.Remove(person))
        ])
        .HeightRequest(60);
}
