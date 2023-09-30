using MauiReactor;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

class MainPagePullToRefreshState
{
    public ObservableCollection<Person> Persons { get; set; }
    
    public bool IsRefreshing {  get; set; }
}

class MainPagePullToRefresh : Component<MainPagePullToRefreshState>
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
            new RefreshView
            {
                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
            }
            .IsRefreshing(State.IsRefreshing)
            .OnRefreshing(OnRefresh)
        };
    }

    private void OnRefresh()
    {
        SetState(s => s.IsRefreshing = true);

        Task.Run(async () =>
        {
            await Task.Delay(2000);

            var person6 = new Person("Daniel", "Robinson", new DateTime(1982, 10, 2));
            var person7 = new Person("Ella", "Martin", new DateTime(1992, 11, 13));
            var person8 = new Person("Frank", "Garcia", new DateTime(1987, 3, 19));

            //is not required here to call set state because we don't want to refresh the entire component
            //the collection view is bound to State.Persons
            State.Persons.Insert(0, person6);
            State.Persons.Insert(0, person7);
            State.Persons.Insert(0, person8);

            //here we need to refresh the component so that the collection hide the 
            //busy indicator
            SetState(s => s.IsRefreshing = false);
        });
    }

    private VisualNode RenderPerson(Person person)
    {
        return new VStack(spacing: 5)
        {
            new Label($"{person.FirstName} {person.LastName}"),
            new Label(person.DateOfBirth.ToShortDateString())
                .FontSize(12)
        }
        .VCenter();
    }
}
