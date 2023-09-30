using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

class MainPageSelectionState
{
    public Person[] Persons { get; set; }

    public ObservableCollection<string> EventMessages = new();

    public MauiControls.SelectionMode SelectionMode { get; set; }
}

class MainPageSelection : Component<MainPageSelectionState>
{
    protected override void OnMounted()
    {
        var person1 = new Person("John", "Doe", new DateTime(1980, 5, 10));
        var person2 = new Person("Jane", "Smith", new DateTime(1990, 6, 20));
        var person3 = new Person("Alice", "Johnson", new DateTime(1985, 7, 30));
        var person4 = new Person("Bob", "Williams", new DateTime(2000, 8, 15));
        var person5 = new Person("Charlie", "Brown", new DateTime(1995, 9, 25));

        State.Persons = new [] { person1, person2, person3, person4, person5 };
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid("Auto,*,*", "*")
            {
                new Picker()
                    .ItemsSource(Enum.GetValues<MauiControls.SelectionMode>().Select(_=>_.ToString()).ToList())
                    .SelectedIndex((int)State.SelectionMode)
                    .OnSelectedIndexChanged(index => SetState(s => s.SelectionMode = (MauiControls.SelectionMode)index)),

                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
                    .SelectionMode(State.SelectionMode)
                    .OnSelectionChanged(OnSelectionChanged)
                    //.OnSelected<CollectionView, Person>(OnSelectedSinglePerson)
                    .GridRow(1),

                new CollectionView()
                    .ItemsSource(State.EventMessages, message => new Label(message).Margin(4,8))
                    .GridRow(2),
            }
        };
    }

    private void OnSelectedSinglePerson(Person person)
    {
        State.EventMessages.Add($"Selected: {person}");
    }

    private void OnSelectionChanged(object sender, MauiControls.SelectionChangedEventArgs args)
    {
        State.EventMessages.Add($"Previous selection: {string.Join(",", args.PreviousSelection)}");
        State.EventMessages.Add($"Current selection: {string.Join(",", args.CurrentSelection)}");
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
