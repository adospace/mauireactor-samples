using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackizerApp.Models;

namespace TrackizerApp.Pages.Views;

class BudgetsViewState
{
    public ObservableCollection<BudgetByCategory> BudgetByCategories { get; set; } = [
        new BudgetByCategory(Category.AutoTransport, 125.99, 400),
        new BudgetByCategory(Category.Entertainment, 350.99, 600),
        new BudgetByCategory(Category.Security, 475.99, 600)
    ];
}

partial class BudgetsView : Component<BudgetsViewState>
{
    public override VisualNode Render()
        => Grid("Auto,*", "*",

            VStack(
                BudgetIndicator(),

                Border(
                    HStack(spacing: 8,
                        Theme.H2("Your budgets are on track")
                            .TextColor(Theme.White),
                        Image("images/like.png")
                    )
                    .Center()
                )
                .StrokeCornerRadius(16)
                .HeightRequest(60)
                .Stroke(Theme.Grey60)
            ),

            BudgetByCategory()
                .GridRow(1)
        )
        .Margin(24);

    VisualNode BudgetIndicator()
        => Render<double[]>(state =>
                Grid(
                    new CanvasView()
                    {
                        new Group
                        {
                            new Arc()
                                .StrokeColor(Theme.Grey60)
                                .StrokeSize(8)
                                .StartAngle(-180)
                                .StrokeLineCap(LineCap.Round)
                                .EndAngle(0)
                                .Clockwise(true)
                                ,

                            new DropShadow
                            {
                                new Arc()
                                    .StrokeColor(Theme.Accents100)
                                    .StrokeSize(12)
                                    .StrokeLineCap(LineCap.Round)
                                    .StartAngle(180)
                                    .EndAngle(() => 180 - (float)Math.Min(180, state.Value![0] / State.BudgetByCategories.Sum(_=>_.MonthBudget) * 180) + 4)
                                    .Clockwise(true)
                            }
                            .Color(Theme.White.WithLuminosity(0.7f))
                            .Blur(10),

                            new DropShadow
                            {
                                new Arc()
                                    .StrokeColor(Theme.Accentp100)
                                    .StrokeSize(12)
                                    .StrokeLineCap(LineCap.Round)
                                    .StartAngle(() => 180 - (float)Math.Min(180, state.Value![0] / State.BudgetByCategories.Sum(_=>_.MonthBudget) * 180) - 4)
                                    .EndAngle(() => 180 - (float)Math.Min(180, (state.Value![0] + state.Value![1]) / State.BudgetByCategories.Sum(_=>_.MonthBudget) * 180) + 4)
                                    .Clockwise(true)
                            }
                            .Color(Theme.White.WithLuminosity(0.7f))
                            .Blur(10),

                            new DropShadow
                            {
                                new Arc()
                                    .StrokeColor(Theme.Primary100)
                                    .StrokeSize(12)
                                    .StrokeLineCap(LineCap.Round)
                                    .StartAngle(() => 180 - (float)Math.Min(180, (state.Value![0] + state.Value![1]) / State.BudgetByCategories.Sum(_=>_.MonthBudget) * 180) - 4)
                                    .EndAngle(() => 180 - (float)Math.Min(180, state.Value!.Sum() / State.BudgetByCategories.Sum(_=>_.MonthBudget) * 180))
                                    .Clockwise(true)
                            }
                            .Color(Theme.White.WithLuminosity(0.7f))
                            .Blur(10),

                        }
                        .TranslationY(75)
                    }
                    .WidthRequest(210)
                    .HeightRequest(210)
                    .BackgroundColor(Colors.Red),

                    Theme.H5()
                        .Text(()=> $"${state.Value?.Sum():0}")
                        .VStart()
                        .HCenter()
                        .TextColor(Theme.White)
                        .Margin(0, 120, 0, 0)
                        .OnTapped(() => state.Set(s => [0, 0, 0])),

                    Theme.H1("of $2,000 budget")
                        .VCenter()
                        .HCenter()
                        .TextColor(Theme.Grey40)
                        .Margin(0, 120, 0, 0),

                    new AnimationController
                    {
                        new ParallelAnimation
                        {
                            new DoubleAnimation()
                                .StartValue(0)
                                .TargetValue(State.BudgetByCategories[0].MonthBills)
                                .Duration(1000)
                                .Easing(Easing.CubicOut)
                                .OnTick(v => state.Set(s => [v, s![1], s[2]])),

                            new DoubleAnimation()
                                .StartValue(0)
                                .TargetValue(State.BudgetByCategories[1].MonthBills)
                                .Duration(1000)
                                .Easing(Easing.CubicOut)
                                .OnTick(v => state.Set(s => [s![0], v, s[2]])),

                            new DoubleAnimation()
                                .StartValue(0)
                                .TargetValue(State.BudgetByCategories[2].MonthBills)
                                .Duration(1000)
                                .Easing(Easing.CubicOut)
                                .OnTick(v => state.Set(s => [s![0], s[1], v]))
                        }
                    }
                    .IsEnabled(state.Value![0] != State.BudgetByCategories[0].MonthBills ||
                        state.Value[1] != State.BudgetByCategories[1].MonthBills ||
                        state.Value[2] != State.BudgetByCategories[2].MonthBills)
                    
                )
            , [0,0,0]);

    CollectionView BudgetByCategory()
        => CollectionView()
            .ItemsSource(State.BudgetByCategories, 
                (budget) =>
                    Render<double>(state =>
                        Border(
                            Grid("*,*,Auto", "Auto, *, Auto",

                                Image($"icons/{budget.Category.ToString().ToLower()}.png")
                                    .GridRowSpan(2)
                                    .Margin(16),

                                Theme.H2(budget.Category.GetDisplayName())
                                    .TextColor(Theme.White)
                                    .VEnd()
                                    .GridColumn(1),

                                Theme.H1("$375 left to spend")
                                    .TextColor(Theme.Grey30)
                                    .GridColumn(1)
                                    .GridRow(1),

                                Theme.H2($"${budget.MonthBills}")
                                    .TextColor(Theme.White)
                                    .Margin(16,0)
                                    .VEnd()
                                    .HEnd()
                                    .GridColumn(2),

                                Theme.H1($"of ${budget.MonthBudget}")
                                    .TextColor(Theme.Grey30)
                                    .Margin(16, 0)
                                    .HEnd()
                                    .GridColumn(2)
                                    .GridRow(1),

                                new CanvasView
                                {
                                    new Group
                                    {
                                        new Box()
                                            .BackgroundColor(Theme.Grey30)
                                            .CornerRadius(9),

                                        new DropShadow
                                        {
                                            new Box()
                                                .CornerRadius(9)
                                                .ScaleX(() => (float)(state.Value / budget.MonthBudget))
                                                .BackgroundColor(budget.Category == Category.AutoTransport ? Theme.Accents100 : (budget.Category == Category.Entertainment ? Theme.Accentp100 : Theme.Primary100))
                                        }
                                        .Color(Theme.White.WithLuminosity(0.7f))
                                        .Blur(5)
                                    }
                                }
                                .BackgroundColor(Colors.Transparent)
                                .HeightRequest(4)
                                .GridRow(2)
                                .GridColumnSpan(3)
                                .Margin(16, 0, 16, 11),

                                //Rectangle()
                                //    .BackgroundColor(Theme.Grey30)
                                //    .HeightRequest(4)
                                //    .RadiusX(9)
                                //    .RadiusY(9)
                                //    .GridRow(2)
                                //    .GridColumnSpan(3)
                                //    .Margin(16, 0, 16, 11),

                                //Rectangle()
                                //    .BackgroundColor(Theme.Primary100)
                                //    .HeightRequest(4)
                                //    .RadiusX(9)
                                //    .RadiusY(9)
                                //    .ScaleX(() =>
                                //    {
                                //        System.Diagnostics.Debug.WriteLine($"scale: {state.Value / budget.MonthBudget}");
                                //        return state.Value / budget.MonthBudget;
                                //    })
                                //    .GridRow(2)
                                //    .GridColumnSpan(3)
                                //    .Margin(16, 0, 16, 11)
                                //    .Shadow(Shadow().Brush(
                                //        new MauiControls.SolidColorBrush(
                                //            Theme.Primary100.WithLuminosity(0.5f).WithAlpha(0.5f))
                                //        )),

                                new AnimationController
                                {
                                    new ParallelAnimation
                                    {
                                        new DoubleAnimation()
                                            .StartValue(0)
                                            .TargetValue(budget.MonthBills)
                                            .Duration(1000)
                                            .Easing(Easing.CubicOut)
                                            .OnTick(v => state.Set(s => v, false))
                                    }
                                }
                                .IsEnabled(state.Value != budget.MonthBills)
                            )
                         )
                        .StrokeCornerRadius(16)
                        .StrokeThickness(0.5)
                        .Stroke(new MauiControls.LinearGradientBrush(
                            [
                                new MauiControls.GradientStop(Color.FromArgb("#CFCFFC"), 0.0f),
                                new MauiControls.GradientStop(Colors.Transparent, 1.0f)
                            ], new Point(), new Point(0.5, 1.0)))
                        .BackgroundColor(Theme.Grey60.WithAlpha(0.2f))
                        .HeightRequest(84)
                        .Margin(0,0,0,8)   
                    )
                )
                .Margin(0,16,0,8);
}
