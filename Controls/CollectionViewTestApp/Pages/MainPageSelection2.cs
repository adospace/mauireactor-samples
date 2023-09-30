using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

class MainPageSelection2State
{
    public List<Person> Persons { get; set; }

    public Person SelectedPerson { get; set; }
}

class MainPageSelection2 : Component<MainPageSelection2State>
{
    protected override void OnMounted()
    {
        var person1 = new Person("John", "Doe", new DateTime(1980, 5, 10));
        var person2 = new Person("Jane", "Smith", new DateTime(1990, 6, 20));
        var person3 = new Person("Alice", "Johnson", new DateTime(1985, 7, 30));
        var person4 = new Person("Bob", "Williams", new DateTime(2000, 8, 15));
        var person5 = new Person("Charlie", "Brown", new DateTime(1995, 9, 25));

        State.Persons = new List<Person>(new[] { person1, person2, person3, person4, person5 });
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid("Auto,*", "*")
            {
                new Grid("*", "*,*,*")
                {
                    new Button("Up")
                        .OnClicked(MoveUp),
                    new Button("Down")
                        .GridColumn(1)
                        .OnClicked(MoveDown),
                    new Button("Reset")
                        .GridColumn (2)
                        .OnClicked(()=>SetState(s => s.SelectedPerson = null)),
                },

                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
                    .SelectedItem(State.SelectedPerson)
                    .SelectionMode(MauiControls.SelectionMode.Single)
                    .OnSelected<CollectionView, Person>(OnSelectedSinglePerson)
                    .GridRow(1),
            }
        };
    }

    private void MoveUp()
    {
        if (State.SelectedPerson == null)
        {
            return;
        }

        var indexOfPerson = State.Persons.IndexOf(State.SelectedPerson);
        if (indexOfPerson > 0)
        {
            indexOfPerson--;
            SetState(s => s.SelectedPerson = State.Persons[indexOfPerson]);
        }
    }

    private void MoveDown()
    {
        if (State.SelectedPerson == null)
        {
            return;
        }

        var indexOfPerson = State.Persons.IndexOf(State.SelectedPerson);
        if (indexOfPerson < State.Persons.Count - 1)
        {
            indexOfPerson++;
            SetState(s => s.SelectedPerson = State.Persons[indexOfPerson]);
        }
    }

    private void OnSelectedSinglePerson(Person person)
    {
        SetState(s => s.SelectedPerson = person);
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
        .BackgroundColor(State.SelectedPerson == person ? Colors.Green : Colors.White)
        .Padding(5,10);
    }
}
