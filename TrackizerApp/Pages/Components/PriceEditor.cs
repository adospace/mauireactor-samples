using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackizerApp.Pages.Components;

partial class PriceEditor : Component
{
    public override VisualNode Render()
        => Grid("*", "Auto,*,Auto",
            
            OperationButton("plus.png"),

            VStack(
                Theme.H1("Monthly price")
                    .TextColor(Theme.Grey40)
                    .HorizontalTextAlignment(TextAlignment.Center),

                Theme.H4("$5.99")
                    .FontAttributes(MauiControls.FontAttributes.Bold)
                    .HorizontalTextAlignment(TextAlignment.Center)
                    .TextColor(Theme.White),

                Border()
                    .HeightRequest(1)
                    .BackgroundColor(Theme.Grey70)
                    .WidthRequest(170)
                )
                .Center()
                .GridColumn(1),


            OperationButton("minus.png")
                .GridColumn(2)
            
            );


    Border OperationButton(string imageSource)
        => Border(
                Image(imageSource)
                .WidthRequest(48)
            )
            .Center()
            .StrokeCornerRadius(16)
            .StrokeThickness(0.5)
            .Stroke(new MauiControls.LinearGradientBrush(
                [
                    new MauiControls.GradientStop(Color.FromArgb("#CFCFFC"), 0.0f),
                    new MauiControls.GradientStop(Colors.Transparent, 1.0f)
                ], new Point(), new Point(0.5, 1.0)))
            .BackgroundColor(Theme.Grey60);
}
