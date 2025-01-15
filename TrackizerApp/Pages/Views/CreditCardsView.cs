using MauiReactor;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages.Views;

class CreditCardsViewState
{
    public ObservableCollection<CreditCard> Cards { get; set; } = [
        new CreditCard("John Doe", "1232", "08/25", "Virtual Card"),
        new CreditCard("Aldo Rossi", "4534", "01/26", "Virtual Card"),
        new CreditCard("Oliver James", "7867", "10/23", "Standard Card")
        ];
}

partial class CreditCardsView : Component<CreditCardsViewState>
{
    public override VisualNode Render()
        => Grid(
            new CardSelector()
                .Cards(State.Cards),

            VStack(spacing: 16,
                ApplicationTheme.H3("Subscriptions").TextColor(ApplicationTheme.White).HCenter(),
                HStack(spacing: 8,
                    [.. Enum.GetValues<SubscriptionType>().Select(type => Image($"{type.ToString().ToLower()}.png").HeightRequest(40))]
                    )
                )
                .Center()
                .VEnd()
                .Margin(0, 240),

            Border(
                Border(
                    HStack(spacing: 10,
                        ApplicationTheme.H2("Add new card").TextColor(ApplicationTheme.Grey30).VCenter(),
                        Image("add.png").HeightRequest(16)                            
                        )
                        .Center()
                    )
                    .StrokeCornerRadius(16)
                    .StrokeDashArray([5, 5])
                    .Stroke(ApplicationTheme.Grey60)
                    .VStart()
                    .Margin(24)
                    .HeightRequest(52)
                )
                .HeightRequest(185)
                .VEnd()
                .StrokeThickness(0)
                .BackgroundColor(ApplicationTheme.Grey70)
                .StrokeCornerRadius(24, 24, 0, 0)
            );
}
