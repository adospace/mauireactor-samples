using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

class MainPageEmptyViewState
{
    public ObservableCollection<Person> Persons { get; set; }
}

class MainPageEmptyView : Component<MainPageEmptyViewState>
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
    {
        return new ContentPage
        {
            new Grid("Auto * *", "*")
            {
                new Button("Remove")
                    //here we use SetState even for the Persons ObservableCollection
                    //because we need to update the IsEnabled property of the button
                    .OnClicked(()=>SetState(s => s.Persons.RemoveAt(0)))
                    .IsEnabled(State.Persons.Count > 0),

                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
                    //1 use just a string
                    .EmptyView("No more persons!")
                    .GridRow(1),

                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
                    //pass in a full view
                    .EmptyView(new VStack(spacing: 5)
                    {
                        new Label("No more persons!"),

                        new Image("error.png")
                    }
                    .VCenter()
                    .HCenter())
                    .GridRow(2)
            }
        };
    }

    private VisualNode RenderPerson(Person person)
    {
        return new VStack(spacing: 5)
        {
            new Label($"{person.FirstName} {person.LastName}"),
            new Label(person.DateOfBirth.ToShortDateString())
                .FontSize(12)
                .TextColor(Colors.Gray)
        }
        .Margin(5,10);
    }
}
