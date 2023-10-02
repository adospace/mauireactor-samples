using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndicatorViewTestApp.Pages;

public class TestModel
{
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string Description { get; set; }
}

class MainPageCustomState
{
    public List<TestModel> Models { get; set; } = new();

    public TestModel SelectedModel { get; set; }

}

class MainPageCustom : Component<MainPageCustomState>
{
    private MauiControls.CarouselView _carouselView;

    protected override void OnMounted()
    {
        State.Models = new List<TestModel>()
        {
            new TestModel
            {
                Title = "Lorem Ipsum Dolor Sit Amet",
                SubTitle = "Consectetur Adipiscing Elit",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed in libero non quam accumsan sodales vel vel nunc."
            },
            new TestModel
            {
                Title = "Praesent Euismod Tristique",
                SubTitle = "Vestibulum Fringilla Egestas",
                Description = "Praesent euismod tristique vestibulum. Vivamus vestibulum justo in massa aliquam, id facilisis odio feugiat. Duis euismod massa id elit imperdiet."
            },
            new TestModel
            {
                Title = "Aliquam Erat Volutpat",
                SubTitle = "Aenean Feugiat In Mollis",
                Description = "Aliquam erat volutpat. Aenean feugiat in mollis ac. Nullam eget justo ut orci dictum auctor."
            },
            new TestModel
            {
                Title = "Suspendisse Tincidunt",
                SubTitle = "Faucibus Ligula Quis",
                Description = "Suspendisse tincidunt, arcu eget auctor efficitur, nulla justo tristique neque, et fermentum orci ante eget nunc."
            },
        };

        State.SelectedModel = State.Models.First();

        base.OnMounted();
    }

    public override VisualNode Render()
    {
        return new ContentPage
        {
            new VStack
            {
                new Grid("* 40", "*")
                {
                    new CarouselView(r => _carouselView = r)
                        .HeightRequest(450)
                        .HorizontalScrollBarVisibility(ScrollBarVisibility.Never)
                        .ItemsSource(State.Models, _=> new Grid("*", "*")
                        {
                            new VStack
                            {
                                  new Label(_.Title)
                                    .Margin(0,5),
                                  new Label(_.SubTitle)
                                    .Margin(0,5),
                                 new Label(_.Description)
                                    .Margin(0,8)
                            }
                        })
                        .CurrentItem(() => State.SelectedModel!)
                        .OnCurrentItemChanged((s, args) => SetState(s => s.SelectedModel = (TestModel)args.CurrentItem))
                        ,

                    new HStack(spacing: 5)
                    {
                        State.Models.Select(item =>
                            new Image(State.SelectedModel == item ? "sun.png" : "circle.png")
                                .WidthRequest(20)
                                .HeightRequest(20)
                                .OnTapped(()=>SetState(s=>s.SelectedModel = item, false))
                            )
                    }
                    .HCenter()
                    .VCenter()
                    .GridRow(1)
                }
            }
            .Padding(8,0)
            .VCenter()
        };
    }
}