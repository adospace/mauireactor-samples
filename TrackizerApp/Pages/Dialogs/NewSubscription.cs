using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiReactor;
using TrackizerApp.Models;
using TrackizerApp.Pages.Components;

namespace TrackizerApp.Pages.Dialogs;

class NewSubscriptionState
{
    public SubscriptionType SubscriptionType { get; set; }
    public string? Description { get; set; }
}

class NewSubscription : Component<NewSubscriptionState>
{
    static SubscriptionType[] _subscriptionTypes = Enum.GetValues<SubscriptionType>();

    public override VisualNode Render()
        => new BaseScreenLayout
        {
            Grid(
                Border(
                    Grid("Auto, *", "*",
                        ApplicationTheme.H7("Add new subscription")
                            .TextColor(ApplicationTheme.White)
                            .FontAttributes(MauiControls.FontAttributes.Bold)
                            .HorizontalTextAlignment(TextAlignment.Center)
                            .Margin(0,96,0,0),

                        new SubscriptionSelector()
                            .OnSelectedType(type => SetState(s => s.SubscriptionType = type, false))
                            .GridRow(1)
                        )                        
                    )
                    .BackgroundColor(ApplicationTheme.Grey70)
                    .StrokeCornerRadius(0, 0, 24, 24)
                    .HeightRequest(476)
                    .VStart(),

                Image("back.png")
                    .HeightRequest(24)
                    .VStart()
                    .HStart()
                    .Margin(24,32)
                    .OnTapped(OnClose),

                ApplicationTheme.H3("New")
                    .VStart()
                    .HCenter()
                    .TextColor(ApplicationTheme.Grey30)
                    .Margin(23,32),

                Grid("Auto,*,Auto", "*",
                    new RoundedEntry()
                        .OnTextChanged(text => SetState(s => s.Description = text))
                        .LabelText("Description"),

                    new PriceEditor()
                        .GridRow(1),

                    ApplicationTheme.PrimaryButton("Add this platform", OnAdd)
                        .GridRow(2)

                )
                .RowSpacing(24)
                .Margin(24,24,24,32)
                .VEnd()
            )
        }
        .StatusBarColor(ApplicationTheme.Grey70);

    async void OnAdd()
    {
        if (Navigation?.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
    }

    async void OnClose()
    {
        if (Navigation?.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
    }
}
