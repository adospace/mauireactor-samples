using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

class MainPageScrollingAnimationState
{
    public ObservableCollection<IndexedPersonWithAddress> Persons { get; set; }
    
    public double ScrollY { get; set; }
}

class MainPageScrollingAnimation : Component<MainPageScrollingAnimationState>
{
    private const double _itemSize = 128;

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
            new CollectionView()
                .ItemsSource(State.Persons, RenderItem)
                .OnScrolled((s,e) => SetState(s => s.ScrollY = e.VerticalOffset, false))

            //new ScrollView
            //{
            //    new VStack
            //    {
            //        State.Persons.Select(RenderItem)
            //    }
            //}
            //.OnScrolled((s,e) => SetState(s => s.ScrollY = e.ScrollY, false))
        };
    }

    private VisualNode RenderItem(IndexedPersonWithAddress item)
    {
        return new Border
        {
            new Label($"Item {item.Index}")
                .TextColor(Colors.White)
                .Center()
        }
        .StrokeThickness(0)
        .StrokeCornerRadius(34, 34, 0, 0)
        .HeightRequest(_itemSize)
        .BackgroundColor(Colors.BlueViolet)
        .ScaleX(() => 0.5 + GetPercOffset(item) * 0.5)
        .Opacity(() => 0.2 + GetPercOffset(item) * 0.8)
        ;
    }

    private double GetPercOffset(IndexedPersonWithAddress item)
    {
        var itemScrollY = item.Index * _itemSize;

        if (itemScrollY < State.ScrollY - _itemSize)
        {
            return 0.0;
        }
        else if (itemScrollY > State.ScrollY + _itemSize) 
        {
            return 1.0;
        }

        return (itemScrollY - (State.ScrollY - _itemSize)) / (_itemSize * 2);
    }
}
