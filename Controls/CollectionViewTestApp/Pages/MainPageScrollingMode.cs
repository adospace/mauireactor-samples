using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CollectionViewTestApp.Pages;


class MainPageScrollingModeState
{
    public ObservableCollection<IndexedPersonWithAddress> Persons { get; set; }

    public MauiControls.ItemsUpdatingScrollMode CurrentScrollMode { get; set; }
}

class MainPageScrollingMode : Component<MainPageScrollingModeState>
{
    private MauiControls.CollectionView _collectionViewRef;

    private static List<IndexedPersonWithAddress> GenerateSamplePersons(int count)
    {
        var random = new Random();
        var firstNames = new[] { "John", "Jim", "Joe", "Jack", "Jane", "Jill", "Jerry", "Jude", "Julia", "Jenny" };
        var lastNames = new[] { "Brown", "Green", "Black", "White", "Blue", "Red", "Gray", "Smith", "Doe", "Jones" };
        var cities = new[] { "New York", "London", "Sidney", "Tokyo", "Paris", "Berlin", "Mumbai", "Beijing", "Cairo", "Rio" };

        var persons = new List<IndexedPersonWithAddress>();

        for (int i = 0; i < count; i++)
        {
            var firstName = firstNames[random.Next(0, firstNames.Length)];
            var lastName = lastNames[random.Next(0, lastNames.Length)];
            var age = random.Next(20, 60);
            var city = cities[random.Next(0, cities.Length)];
            var address = $"{city} No. {random.Next(1, 11)} Lake Park";

            persons.Add(new IndexedPersonWithAddress(i, firstName, lastName, age, address));
        }

        return persons;
    }

    protected override void OnMounted()
    {
        State.Persons = new ObservableCollection<IndexedPersonWithAddress>(GenerateSamplePersons(100));
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid("Auto, Auto, *", "*")
            {
                new Picker()
                    .ItemsSource(Enum.GetValues<MauiControls.ItemsUpdatingScrollMode>().Select(_=>_.ToString()).ToArray())
                    .OnSelectedIndexChanged(index => SetState(s => s.CurrentScrollMode = (MauiControls.ItemsUpdatingScrollMode)index)),                    

                new Button("Add item", ()=> State.Persons.Add(GenerateSamplePersons(1).First() with { Index = State.Persons.Count }))
                    .GridRow(1),

                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
                    .ItemsUpdatingScrollMode(State.CurrentScrollMode)
                    .GridRow(2)
            }
        };
    }

    private VisualNode RenderPerson(IndexedPersonWithAddress person)
    {
        return new VStack(spacing: 5)
        {
            new Label($"{person.Index}. {person.FirstName} {person.LastName} ({person.Age})"),
            new Label(person.Address)
                .FontSize(12)
        }
        .HeightRequest(56)
        .VCenter();
    }
}
