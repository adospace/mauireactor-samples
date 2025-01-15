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
    public BudgetByCategory[] BudgetByCategories { get; set; } = [
        new BudgetByCategory(Category.AutoTransport, 125.99, 400),
        new BudgetByCategory(Category.Entertainment, 350.99, 600),
        new BudgetByCategory(Category.Security, 475.99, 600)
    ];

    public bool IsVisible { get; set; }
}

partial class BudgetsView : Component<BudgetsViewState>
{
    [Prop]
    bool _isVisible;

    protected override void OnPropsChanged()
    {
        //simulate calculations, remote call etc
        if (State.IsVisible != _isVisible &&
            _isVisible)
        {
            State.BudgetByCategories = [
                new BudgetByCategory(Category.AutoTransport, 0, 400),
                new BudgetByCategory(Category.Entertainment, 0, 600),
                new BudgetByCategory(Category.Security, 0, 600)
            ];

            SetState(s => s.BudgetByCategories = [
                new BudgetByCategory(Category.AutoTransport, 125.99, 400),
                new BudgetByCategory(Category.Entertainment, 350.99, 600),
                new BudgetByCategory(Category.Security, 475.99, 600)
            ], delayMilliseconds: 1500);
        }

        State.IsVisible = _isVisible;

        base.OnPropsChanged();
    }

    public override VisualNode Render()
        => Grid("Auto,*", "*",

            VStack(
                BudgetIndicator([.. State.BudgetByCategories.Select(_=>_.MonthBills)], State.BudgetByCategories.Sum(_ => _.MonthBudget)),

                Border(
                    HStack(spacing: 8,
                        ApplicationTheme.H2("Your budgets are on track")
                            .TextColor(ApplicationTheme.White),
                        Image("images/like.png")
                    )
                    .Center()
                )
                .StrokeCornerRadius(16)
                .HeightRequest(60)
                .Stroke(ApplicationTheme.Grey60)
            ),

            BudgetByCategory()
                .GridRow(1)
        )
        .Margin(24);

    VisualNode BudgetIndicator(double[] monthBills, double total)
        => Render<double[]>(state =>
                Grid(
                    new CanvasView()
                    {
                        new Group
                        {
                            new Arc()
                                .StrokeColor(ApplicationTheme.Grey60)
                                .StrokeSize(8)
                                .StartAngle(-180)
                                .StrokeLineCap(LineCap.Round)
                                .EndAngle(0)
                                .Clockwise(true)
                                ,

                            new DropShadow
                            {
                                state.Value![0] < 0.1 ? null :
                                new Arc()
                                    .StrokeColor(ApplicationTheme.Accents100)
                                    .StrokeSize(12)
                                    .StrokeLineCap(LineCap.Round)
                                    .StartAngle(180)
                                    .EndAngle(() => 180 - (float)Math.Min(180, state.Value![0] / total * 180) + 4)
                                    .Clockwise(true)
                            }
                            .Color(ApplicationTheme.White.WithLuminosity(0.7f))
                            .Blur(10),

                            new DropShadow
                            {
                                (state.Value![0] + state.Value![1]) < 0.1 ? null :
                                new Arc()
                                    .StrokeColor(ApplicationTheme.Accentp100)
                                    .StrokeSize(12)
                                    .StrokeLineCap(LineCap.Round)
                                    .StartAngle(() => 180 - (float)Math.Min(180, state.Value![0] /total * 180) - 4)
                                    .EndAngle(() => 180 - (float)Math.Min(180, (state.Value![0] + state.Value![1]) /total * 180) + 4)
                                    .Clockwise(true)
                            }
                            .Color(ApplicationTheme.White.WithLuminosity(0.7f))
                            .Blur(10),

                            new DropShadow
                            {
                                state.Value!.Sum() < 0.1 ? null :
                                new Arc()
                                    .StrokeColor(ApplicationTheme.Primary100)
                                    .StrokeSize(12)
                                    .StrokeLineCap(LineCap.Round)
                                    .StartAngle(() => 180 - (float)Math.Min(180, (state.Value![0] + state.Value![1]) /total * 180) - 4)
                                    .EndAngle(() => 180 - (float)Math.Min(180, state.Value!.Sum() /total * 180))
                                    .Clockwise(true)
                            }
                            .Color(ApplicationTheme.White.WithLuminosity(0.7f))
                            .Blur(10),

                        }
                        .TranslationY(60)
                        .Margin(15)
                    }
                    .WidthRequest(225)
                    .HeightRequest(225),

                    ApplicationTheme.H5()
                        .Text(()=> $"${state.Value?.Sum():0}")
                        .VStart()
                        .HCenter()
                        .TextColor(ApplicationTheme.White)
                        .Margin(0, 120, 0, 0)
                        .OnTapped(() => state.Set(s => [0, 0, 0])),

                    ApplicationTheme.H1("of $2,000 budget")
                        .VCenter()
                        .HCenter()
                        .TextColor(ApplicationTheme.Grey40)
                        .Margin(0, 120, 0, 0),

                    new AnimationController
                    {
                        new ParallelAnimation
                        {
                            new DoubleAnimation()
                                .StartValue(0)
                                .TargetValue(monthBills[0])
                                .Duration(1000)
                                .Easing(Easing.CubicOut)
                                .OnTick(v => state.Set(s => [v, s![1], s[2]])),

                            new DoubleAnimation()
                                .StartValue(0)
                                .TargetValue(monthBills[1])
                                .Duration(1000)
                                .Easing(Easing.CubicOut)
                                .OnTick(v => state.Set(s => [s![0], v, s[2]])),

                            new DoubleAnimation()
                                .StartValue(0)
                                .TargetValue(monthBills[2])
                                .Duration(1000)
                                .Easing(Easing.CubicOut)
                                .OnTick(v => state.Set(s => [s![0], s[1], v]))
                        }
                        .InitialDelay(0)
                    }
                    .IsEnabled(!state.Value!.SequenceEqual(monthBills))
                    
                )
            , [0,0,0]);

    CollectionView BudgetByCategory()
        => CollectionView()
            .ItemsSource(State.BudgetByCategories, 
                (budget) =>
                    Render<double>(state =>
                        Border(
                            Grid("*,*,Auto", "Auto, *, Auto",

                                Image($"{budget.Category.ToString().ToLower()}.png")
                                    .GridRowSpan(2)
                                    .Margin(16),

                                ApplicationTheme.H2(budget.Category.GetDisplayName())
                                    .TextColor(ApplicationTheme.White)
                                    .VEnd()
                                    .GridColumn(1),

                                ApplicationTheme.H1("$375 left to spend")
                                    .TextColor(ApplicationTheme.Grey30)
                                    .GridColumn(1)
                                    .GridRow(1),

                                ApplicationTheme.H2($"${budget.MonthBills}")
                                    .TextColor(ApplicationTheme.White)
                                    .Margin(16,0)
                                    .VEnd()
                                    .HEnd()
                                    .GridColumn(2),

                                ApplicationTheme.H1($"of ${budget.MonthBudget}")
                                    .TextColor(ApplicationTheme.Grey30)
                                    .Margin(16, 0)
                                    .HEnd()
                                    .GridColumn(2)
                                    .GridRow(1),

                                new CanvasView
                                {
                                    new Group
                                    {
                                        new Box()
                                            .BackgroundColor(ApplicationTheme.Grey30)
                                            .CornerRadius(9),

                                        new DropShadow
                                        {
                                            new Box()
                                                .CornerRadius(9)
                                                .ScaleX(() => (float)(state.Value / budget.MonthBudget))
                                                .BackgroundColor(budget.Category == Category.AutoTransport ? ApplicationTheme.Accents100 : (budget.Category == Category.Entertainment ? ApplicationTheme.Accentp100 : ApplicationTheme.Primary100))
                                        }
                                        .Color(ApplicationTheme.White.WithLuminosity(0.7f))
                                        .Blur(5)
                                    }
                                }
                                .BackgroundColor(Colors.Transparent)
                                .HeightRequest(4)
                                .GridRow(2)
                                .GridColumnSpan(3)
                                .Margin(16, 0, 16, 11),

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
                        .BackgroundColor(ApplicationTheme.Grey60.WithAlpha(0.2f))
                        .HeightRequest(84)
                        .Margin(0,0,0,8)   
                    )
                )
                .Margin(0,16,0,8);
}
