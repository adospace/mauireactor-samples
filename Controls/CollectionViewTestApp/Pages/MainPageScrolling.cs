using MauiReactor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CollectionViewTestApp.Pages;
public record IndexedPersonWithAddress(int Index, string FirstName, string LastName, int Age, string Address);

class MainPageScrollingState
{
    public List<IndexedPersonWithAddress> Persons { get; set; }

    public MauiControls.ItemsViewScrolledEventArgs LatestEventArgs { get; set; }

    public int ScrollToIndex {  get; set; }

    public MauiControls.ScrollToPosition ScrollToPosition { get; set; }

    public bool IsAnimationDisabled { get; set; }
}

class MainPageScrolling : Component<MainPageScrollingState>
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
        State.Persons = GenerateSamplePersons(100);
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid("Auto, Auto,*", "*")
            {
                new VStack(spacing: 5)
                {
                    new Label($"CenterItemIndex: {State.LatestEventArgs?.CenterItemIndex}"),
                    new Label($"LastVisibleItemIndex: {State.LatestEventArgs?.LastVisibleItemIndex}"),
                    new Label($"FirstVisibleItemIndex: {State.LatestEventArgs?.FirstVisibleItemIndex}"),
                    new Label($"VerticalDelta: {State.LatestEventArgs?.VerticalDelta}"),
                    new Label($"VerticalOffset: {State.LatestEventArgs?.VerticalOffset}"),
                },

                new VStack(spacing: 5)
                {
                    new Entry()
                        .Keyboard(Keyboard.Numeric)
                        .OnTextChanged(_ =>
                        {
                            if (int.TryParse(_, out var scrollToIndex) &&
                                scrollToIndex >= 0 &&
                                scrollToIndex < State.Persons.Count)
                            {
                                SetState(s => s.ScrollToIndex = scrollToIndex);
                            }
                        }),

                    new Picker()
                        .ItemsSource(Enum.GetValues<MauiControls.ScrollToPosition>().Select(_=>_.ToString()).ToArray())
                        .OnSelectedIndexChanged(index => SetState(s => s.ScrollToPosition = (MauiControls.ScrollToPosition)index)),                        

                    new HStack(spacing: 5)
                    {
                        new CheckBox()
                            .OnCheckedChanged((sender, args) => SetState(s => s.IsAnimationDisabled = args.Value)),

                        new Label("Disable animation")
                            .HFill()
                            .VCenter(),
                    },

                    new Button("Scroll To")
                        .OnClicked(()=> _collectionViewRef?.ScrollTo(State.ScrollToIndex, position: State.ScrollToPosition, animate: !State.IsAnimationDisabled))
                }
                .GridRow(1),

                new CollectionView(collectionViewRef => _collectionViewRef = collectionViewRef)
                    .ItemsSource(State.Persons, RenderPerson)
                    .GridRow(2)
                    .OnScrolled(OnScrolled)
            }
        };
    }

    private void OnScrolled(object sender, MauiControls.ItemsViewScrolledEventArgs args)
    {
        SetState(s => s.LatestEventArgs = args);
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
