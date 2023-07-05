using Chateo.Pages.Components;
using Chateo.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages;

class ChatsPageState
{
    public bool IsLoading { get; set; }

    public ObservableCollection<UserMessageViewModel> UserMessages { get; set; } = new();
}

class ChatsPage : Component<ChatsPageState>
{
    public override VisualNode Render()
    {
        return new Grid
        {
            State.IsLoading ?
            new ActivityIndicator()
                    .IsVisible(true)
                    .IsRunning(true)
                    .HCenter()
                    .VCenter()
                    :
            new Grid("56,108,52,*, 83", "*")
            {
                new Grid("24", "*, 32, 24")
                {
                    Theme.Current.Label("Chats")
                        .FontSize(18),

                    Theme.Current.Image(Icon.Chat)
                        .GridColumn(1)
                        .Margin(0,0,8,0),

                    Theme.Current.Image(Icon.Check)
                        .GridColumn(2)
                }
                .VEnd()
                .Margin(0,13),

                new ScrollView
                {
                    new HStack(spacing: 16)
                    {
                        RenderStoryItem("Your Story", Theme.Current.BorderedImage(Icon.StoryPlus)),

                        RenderStoryItem("Story 1", Theme.Current.BorderedImage("/images/avatar1.png")),

                        RenderStoryItem("Story 2", Theme.Current.BorderedImage("/images/avatar2.png"))
                    }
                }
                .Margin(0,16)
                .Orientation(ScrollOrientation.Horizontal)
                .GridRow(1),

                new Rectangle()
                    .HeightRequest(2)
                    .Margin(-24,0)
                    .Fill(Theme.Current.MediumBackground)
                    .VEnd()
                    .GridRow(1),

                new Border
                {
                    new Grid
                    {
                        Theme.Current.Image(Icon.Search)
                            .HeightRequest(24)
                            .HStart()
                            .Margin(8),

                        Theme.Current.Entry()
                            .Placeholder("Search")
                            .Margin(32,0,4,0)
                    }
                }
                .BackgroundColor(Theme.Current.MediumBackground)
                .StrokeShape(new RoundRectangle().CornerRadius(4))
                .HeightRequest(36)
                .Margin(0, 16, 0, 0)
                .GridRow(2),

                new CollectionView()
                    
                    .GridRow (3),

            }
        }
        .Margin(24, 16);
        ;
    }

    VisualNode RenderStoryItem(string label, Border image)
        => new Grid("56, 20", "56")
        {
            image,

            Theme.Current.Label(label)
                .FontSize(10)
                .Margin(0,4,0,0)
                .HorizontalTextAlignment(TextAlignment.Center)
                .GridRow(1)
        };

}
