using Chateo.Pages.Components;
using Chateo.Resources.Styles;
using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages;

class ChatsPageState
{
    public bool IsLoading { get; set; }
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
            new Grid("56,68,*, 83", "*")
            {
                new Grid("24", "*, 32, 24")
                {
                    Theme.Current.Label("Chats")
                        .FontSize(18),

                    Theme.Current.Image(Icon.Comment)
                        .HeightRequest(24)
                        .WidthRequest(24)
                        .GridColumn(1)
                        .Margin(0,0,0,8),

                    Theme.Current.Image(Icon.Check)
                        .GridColumn(2)
                }
                .VEnd()
                .Margin(0,13),
            }
        }
        .Margin(24, 16);
        ;
    }
}
