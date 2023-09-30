using MauiReactor;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionViewTestApp.Pages;

enum LayoutType
{
    LinearVertical,

    LinearHorizontal,

    VerticalGrid,

    HorizontalGrid
}

class MainPageLayoutState
{
    public Person[] Persons { get; set; }

    public LayoutType CurrentLayout { get; set; }
}

class MainPageLayout : Component<MainPageLayoutState>
{
    protected override void OnMounted()
    {
        var person1 = new Person("John", "Doe", new DateTime(1980, 5, 10));
        var person2 = new Person("Jane", "Smith", new DateTime(1990, 6, 20));
        var person3 = new Person("Alice", "Johnson", new DateTime(1985, 7, 30));
        var person4 = new Person("Bob", "Williams", new DateTime(2000, 8, 15));
        var person5 = new Person("Charlie", "Brown", new DateTime(1995, 9, 25));
        var person6 = new Person("Daniel", "Robinson", new DateTime(1982, 10, 2));
        var person7 = new Person("Ella", "Martin", new DateTime(1992, 11, 13));
        var person8 = new Person("Frank", "Garcia", new DateTime(1987, 3, 19));
        var person9 = new Person("Grace", "Rodriguez", new DateTime(1979, 4, 23));
        var person10 = new Person("Harry", "White", new DateTime(1999, 2, 28));

        State.Persons = new [] { person1, person2, person3, person4, person5, person6, person7, person8, person9, person10 };
        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid("Auto, *", "*")
            {
                new Picker()
                    .ItemsSource(Enum.GetValues<LayoutType>().Select(_=>_.ToString()).ToList())
                    .SelectedIndex((int)State.CurrentLayout)
                    .OnSelectedIndexChanged(index => SetState(s => s.CurrentLayout = (LayoutType)index)),

                new CollectionView()
                    .ItemsSource(State.Persons, RenderPerson)
                    .ItemsLayout(GetCollectionViewLayout())
                    .GridRow(1)
            }
        };
    }

    private IItemsLayout GetCollectionViewLayout()
    {
        return State.CurrentLayout switch
        {
            LayoutType.LinearVertical => new VerticalLinearItemsLayout(),
            LayoutType.LinearHorizontal => new HorizontalLinearItemsLayout(),
            LayoutType.VerticalGrid => new VerticalGridItemsLayout(span: 3),
            LayoutType.HorizontalGrid => new HorizontalGridItemsLayout(span: 2),
            _ => throw new NotImplementedException(),
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
