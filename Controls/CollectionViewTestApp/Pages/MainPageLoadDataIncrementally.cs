using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

public record PersonWithAddress(string FirstName, string LastName, int Age, string Address);

class MainPageLoadDataIncrementallyState
{
    public ObservableCollection<PersonWithAddress> Persons { get; set; }
    
    public bool IsLoading {  get; set; }
}

class MainPageLoadDataIncrementally : Component<MainPageLoadDataIncrementallyState>
{
    private static IEnumerable<PersonWithAddress> GenerateSamplePersons(int count)
    {
        var random = new Random();
        var firstNames = new[] { "John", "Jim", "Joe", "Jack", "Jane", "Jill", "Jerry", "Jude", "Julia", "Jenny" };
        var lastNames = new[] { "Brown", "Green", "Black", "White", "Blue", "Red", "Gray", "Smith", "Doe", "Jones" };
        var cities = new[] { "New York", "London", "Sidney", "Tokyo", "Paris", "Berlin", "Mumbai", "Beijing", "Cairo", "Rio" };

        var persons = new List<PersonWithAddress>();

        for (int i = 0; i < count; i++)
        {
            var firstName = firstNames[random.Next(0, firstNames.Length)];
            var lastName = lastNames[random.Next(0, lastNames.Length)];
            var age = random.Next(20, 60);
            var city = cities[random.Next(0, cities.Length)];
            var address = $"{city} No. {random.Next(1, 11)} Lake Park";

            persons.Add(new PersonWithAddress(firstName, lastName, age, address));
        }

        return persons;
    }

    protected override void OnMounted()
    {
        State.Persons = new ObservableCollection<PersonWithAddress>(GenerateSamplePersons(100));
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid("*", "*")
            {
                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
                    .RemainingItemsThreshold(50)
                    .OnRemainingItemsThresholdReached(LoadMorePersons),

                new ActivityIndicator()
                    .IsRunning(State.IsLoading)
                    .VCenter()
                    .HCenter()
            }
        };
    }

    private void LoadMorePersons()
    {
        if (State.IsLoading == true)
        {
            return;
        }

        SetState(s => s.IsLoading = true);

        Task.Run(async () =>
        {
            await Task.Delay(3000);

            foreach (var newPerson in GenerateSamplePersons(50))
            {
                //is not required here to call set state because we don't want to refresh the entire component
                //the collection view is bound to State.Persons
                State.Persons.Add(newPerson);
            }

            SetState(s => s.IsLoading = false);
        });
    }

    private VisualNode RenderPerson(PersonWithAddress person)
    {
        return new VStack(spacing: 5)
        {
            new Label($"{person.FirstName} {person.LastName} ({person.Age})"),
            new Label(person.Address)
                .FontSize(12)
        }
        .HeightRequest(56)
        .VCenter();
    }
}
