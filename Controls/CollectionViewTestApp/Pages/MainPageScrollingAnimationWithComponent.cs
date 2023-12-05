using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

class MainPageScrollingAnimationWithComponentState
{
    public ObservableCollection<IndexedPersonWithAddress> Persons { get; set; }
}

class MainPageScrollingAnimationWithComponent : Component<MainPageScrollingAnimationWithComponentState>
{
    event EventHandler<double> Scrolled;

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
                .OnScrolled((s,e) =>
                {
                    Scrolled?.Invoke(s, e.VerticalOffset);
                    System.Diagnostics.Debug.WriteLine($"Scroll {e.VerticalOffset}");
                })
        };
    }

    private VisualNode RenderItem(IndexedPersonWithAddress item)
         => new PersonComponent()
                .WhenScroll(handler => Scrolled += handler)
                .Item(item);
}

class PersonComponentState
{
    public double ScrollY { get; set; }
}

class PersonComponent : Component<PersonComponentState>
{
    IndexedPersonWithAddress _item;
    private Action<EventHandler<double>> _subscribeToScrollEvent;

    readonly double _itemSize = 128;

    public PersonComponent Item(IndexedPersonWithAddress item)
    {
        _item = item;
        return this;
    }

    public PersonComponent WhenScroll(Action<EventHandler<double>> subscribeToScrollEvent)
    {
        _subscribeToScrollEvent = subscribeToScrollEvent;
        return this;        
    }

    protected override void OnMounted()
    {
        //System.Diagnostics.Debug.WriteLine($"OnMounted (Item {_item.Index})");
        _subscribeToScrollEvent?.Invoke(OnParentScrolled);
        base.OnMounted();
    }

    void OnParentScrolled(object sender, double verticalOffset)
    {
        SetState(s => s.ScrollY = verticalOffset);
    }

    public override VisualNode Render()
    {
        //System.Diagnostics.Debug.WriteLine($"Render (Item {_item.Index})");
        double GetPercOffset()
        {
            var itemScrollY = _item.Index * _itemSize;

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

        return new Border
        {
            new VStack()
            {
                new Border()
                    .BackgroundColor(Colors.Blue)
                    .WidthRequest(50)
                    .HeightRequest(50)
                    .VCenter()
                    .HCenter()
                    .ScaleX(() => 1.0 + GetPercOffset()),

                new Label($"Item {_item.Index}")
                    .TextColor(Colors.White)
                    .Center()
            }
        }
        .StrokeThickness(0)
        .StrokeCornerRadius(34, 34, 0, 0)
        .HeightRequest(_itemSize)
        .BackgroundColor(Colors.BlueViolet)
        //.ScaleX(() => 0.5 + GetPercOffset() * 0.5)
        .Opacity(() => 0.2 + GetPercOffset() * 0.8)
        ;
    }

}