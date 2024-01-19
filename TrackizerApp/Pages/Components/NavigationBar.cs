using MauiReactor;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackizerApp.Pages.Components;

class NavigationBarState
{
    public double Width { get; set; } = 329;
}

partial class NavigationBar : Component<NavigationBarState>
{
    [Prop]
    HomeScreenView _view;

    [Prop]
    Action<HomeScreenView>? _onViewChanged;

    [Prop]
    Action? _onNewSubscription;

    public override VisualNode Render()
        => Grid("*", "* *",
            RenderBar()
                .GridColumnSpan(2),

            ImageButton("home_plus.png")
                .HCenter()
                .HeightRequest(100)
                .VEnd()
                .Margin(0)
                .GridColumnSpan(2)
                .OnClicked(_onNewSubscription),

            Grid("*", "* *",
                ViewButton("home.png", HomeScreenView.Home),
                ViewButton("budgets.png", HomeScreenView.Budgets).GridColumn(1)
                )
            .Margin(32, 0),

            Grid("*", "* *",
                ViewButton("calendar.png", HomeScreenView.Calendar),
                ViewButton("credit_cards.png", HomeScreenView.CreditCards).GridColumn(1)
                )
                .Margin(36, 0)
                .GridColumn(1)

            )
            .BackgroundColor(Colors.Transparent)
            .HeightRequest(82)
            .VEnd()
            .OnSizeChanged(size => base.SetState(s => s.Width = size.Width));

    private Grid ViewButton(string imageSource, HomeScreenView view) 
        => Grid(
            Image(imageSource)
                .HeightRequest(18)
                .WidthRequest(18)
                .Center()
                .Opacity(_view == view ? 1.0 : 0.4),
            Button()
                .BackgroundColor(Colors.Transparent)
                .OnClicked(() => _onViewChanged?.Invoke(view)));

    Path RenderBar()
    {
        double barWidth = State.Width - (23 * 2) - 1;
        double controlPoint1 = barWidth / 2 - 33;
        double controlPoint2 = barWidth - 27.5;
        var pathData = string.Create(System.Globalization.CultureInfo.InvariantCulture,
            $"M{controlPoint1 - 9} 0 C{controlPoint1 - 3} 0 {controlPoint1 + 1} 5.5479 {controlPoint1 + 1} 11 V12 C{controlPoint1 + 1} 29.6731 {controlPoint1 + 15} 44 {controlPoint1 + 15 + 18} 44 C{controlPoint1 + 15 + 18 + 17} 44 {controlPoint1 + 15 + 18 + 17 + 15} 29.6731 {controlPoint1 + 15 + 18 + 17 + 15} 12 V11 C{controlPoint1 + 15 + 18 + 17 + 15} 5.5479 {controlPoint1 + 15 + 18 + 17 + 15 + 3} 0 {controlPoint1 + 15 + 18 + 17 + 15 + 3 + 6} 0 H{controlPoint2} C{controlPoint2 + 11} 0 {controlPoint2 + 11 + 5} 0 {controlPoint2 + 11 + 5 + 4} 2.48234 C{controlPoint2 + 11 + 5 + 4 + 2} 3.74805 {controlPoint2 + 11 + 5 + 4 + 4} 5.44127 {controlPoint2 + 11 + 5 + 4 + 4 + 1} 7.44004 C{controlPoint2 + 11 + 5 + 4 + 4 + 1 + 2.5} 11.3601 {controlPoint2 + 11 + 5 + 4 + 4 + 1 + 2.5} 16.7401 {controlPoint2 + 11 + 5 + 4 + 4 + 1 + 2.5} 27.5 C{controlPoint2 + 11 + 5 + 4 + 4 + 1 + 2.5} 38.2599 {controlPoint2 + 11 + 5 + 4 + 4 + 1 + 2.5} 43.6399 {controlPoint2 + 11 + 5 + 4 + 4 + 1} 47.56 C{controlPoint2 + 11 + 5 + 4 + 4} 49.5587 {controlPoint2 + 22} 51.2519 {controlPoint2 + 20} 52.5177 C{controlPoint2 + 16} 55 {controlPoint2 + 11} 55 {controlPoint2} 55H27.5 C16.7401 55 11.3601 55 7.44004 52.5177 C5.44127 51.2519 3.74805 49.5587 2.48234 47.56C0 43.6399 0 38.2599 0 27.5C0 16.7401 0 11.3601 2.48234 7.44004 C3.74805 5.44127 5.44127 3.74805 7.44004 2.48234 C11.3601 0 16.7401 0 27.5 0 H{controlPoint1 - 9}Z");

        return Path()
                .Fill(new MauiControls.SolidColorBrush(Color.FromArgb("#4E4E61").WithAlpha(0.75f)))
                .Stroke(new LinearGradient(90, Color.FromRgba("#CFCFFC"), Colors.Transparent))
                .StrokeThickness(1)
                .Data(pathData)
                .Margin(23, 11)
                .BackgroundColor(Colors.Transparent);
    }
}
