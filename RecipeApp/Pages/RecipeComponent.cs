using System;
using System.Collections.Generic;
using System.Linq;
using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using MauiReactor.Compatibility;
using MauiReactor.Shapes;

namespace RecipeApp.Pages;

class RecipeComponentState
{
    public double HeaderScaleY { get; set; } = 1.0;

    public float HeaderMargin { get; set; }

    public double HeaderTranslationY { get; set; }

    public double BodyTranslationY { get; set; }

    public double HeaderOpacity { get; set; }

    public double HeaderHeight { get; set; } = 300;
}

partial class RecipeComponent : Component<RecipeComponentState>
{
    [Prop]
    Recipe _recipe;
    
    [Prop]
    Rect _originalViewPort;

    [Prop]
    Action _onCancelSelection;

    protected override void OnMountedOrPropsChanged()
    {
        InitializeState();
        base.OnMountedOrPropsChanged();
    }

    void InitializeState()
    {
        State.HeaderScaleY = _originalViewPort.Height / 300;
        State.HeaderTranslationY = _originalViewPort.Top;
        State.HeaderMargin = 10;
        State.BodyTranslationY = _originalViewPort.Top - 300;
        State.HeaderHeight = 300;
        MauiControls.Application.Current.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(10), () =>
            SetState(s =>
            {
                s.HeaderScaleY = 1.0;
                s.HeaderMargin = 0;
                s.HeaderTranslationY = 0;
                s.BodyTranslationY = 0;
            })
        );
    }

    public override VisualNode Render()
    {
        return Grid(
            [.. RenderPageContent(),

            new CanvasView()
            {
                new Align
                {
                    new PointInteractionHandler
                    {
                        new Group
                        {
                            new MauiReactor.Canvas.Ellipse()
                                .FillColor(Colors.Black),

                            new MauiReactor.Canvas.Path()
                                .Data(new PathF(10,21).LineTo(32,21))
                                .StrokeColor(Colors.White)
                                .StrokeSize(2),

                            new MauiReactor.Canvas.Path()
                                .Data(new PathF(10,21).LineTo(22,11))
                                .StrokeColor(Colors.White)
                                .StrokeSize(2),

                            new MauiReactor.Canvas.Path()
                                .Data(new PathF(10,21).LineTo(22,31))
                                .StrokeColor(Colors.White)
                                .StrokeSize(2)
                        }
                    }
                    .OnTap(_onCancelSelection)
                }
                .Margin(10)
                .Height(42)
                .Width(42)
                .HStart()
                .VStart()
            }
            .HeightRequest(52)
            .VStart()
            .BackgroundColor(Colors.Transparent)
            .OnTapped(_onCancelSelection)
            ]
        )
        .IsVisible(_recipe != null)
        .BackgroundColor(Colors.White)
        ;
    }

    IEnumerable<VisualNode> RenderPageContent()
    {
        if (_recipe == null)
        {
            yield break;
        }

        yield return RenderBody();

        yield return RenderHeader();
    }

    CanvasView RenderHeader()
        => new CanvasView
        {
            new DropShadow
            {
                new Box()
                {
                    new Group
                    {
                        _recipe.bgImage != "" ?
                        new Picture(_recipe.bgImage)
                            .Aspect(Aspect.AspectFit)
                            : null,

                        new Picture(_recipe.imageSource)
                            .Aspect(Aspect.AspectFit)
                            .Rotation(()=>300 - (float)State.HeaderHeight)
                            .AnchorX(0.5f)
                            .AnchorY(0.5f)
                            .Margin(20)
                    }
                }
                .BackgroundColor(_recipe.bgColor)
                .Margin(State.HeaderMargin, 0, State.HeaderMargin, 15)
                .WithAnimation(duration: 200)
                .CornerRadius(0, 0, 20, 20)
            }
            .Color(AppColors.blackLight.WithAlpha(0.5f))
            .Blur(30),
        }
        .HeightRequest(() => State.HeaderHeight)
        .VStart()
        .BackgroundColor(Colors.Transparent)
        .ScaleY(State.HeaderScaleY)
        .WithAnimation(duration: 200)
        .TranslationY(State.HeaderTranslationY)
        .WithAnimation(easing: ExtendedEasing.InOutCubic, duration: 400);

    ScrollView RenderBody()
        => ScrollView(
            new CanvasView()
            {
                new Column($"48, 128, 32, {_recipe.ingredients.Length * 42}, 32, {_recipe.instructions.Length * 128}")
                {
                    new Text(_recipe.title)
                        .VerticalAlignment(VerticalAlignment.Top)
                        .FontSize(32)
                        .FontColor(Colors.Black)
                        .Margin(15),

                    new Text(_recipe.description)
                        .FontName("RubikRegular")
                        .FontSize(16)
                        .FontColor(Colors.Black)
                        .Margin(15),

                    new Text("INGREDIENTS")
                        .FontName("RubikBold")
                        .FontColor(Colors.Black)
                        .FontWeight(1200)
                        .Margin(15),

                    new Column()
                    {
                        _recipe.ingredients.Select(RenderIngredientItem).ToArray()
                    },                    

                    new Text("STEPS")
                        .FontName("RubikBold")
                        .FontColor(Colors.Black)
                        .Margin(15),

                    new Column()
                    {
                        _recipe.instructions.Select(RenderStepItem).ToArray()
                    },
                }
            }
            .BackgroundColor(Colors.Transparent)
            .HeightRequest(48 + 128 + 32 + _recipe.ingredients.Length * 42 + 32 + _recipe.instructions.Length * 128)
            .TranslationY(State.BodyTranslationY)
            .WithAnimation(easing: ExtendedEasing.InOutCubic, duration: 400)
        )
        .Padding(() => new Thickness(0, State.HeaderHeight, 0, 0))
        .OnScrolled(OnBodyScrolled);

    VisualNode RenderIngredientItem(string ingredient) 
        => new Row("42, *")
        {
            new Group
            {
                new Box()
                    .BackgroundColor(_recipe.bgColor)
                    .CornerRadius(5),

                new Align
                {
                    new Picture("RecipeApp.Resources.Images.chef.png")
                }
                .Height(24)
                .VCenter()
            },

            new Text(ingredient)
                .FontName("RubikRegular")
                .FontColor(Colors.Black)
                .Margin(10,0)
                .VerticalAlignment(VerticalAlignment.Center)
        }
        .Margin(15, 5)
        ;

    VisualNode RenderStepItem(IndexItem step) 
        => new Row("32,*")
        {
            new Align
            {
                new Group
                {
                    new Box()
                        .BackgroundColor(_recipe.bgColor)
                        .CornerRadius(5),

                    new Text(step.Index.ToString())
                        .FontName("RubikRegular")
                        .FontColor(Colors.Black)
                        .FontWeight(1200)
                        .FontSize(22)
                        .VerticalAlignment(VerticalAlignment.Center)
                        .AnchorX(0.5f)
                        .AnchorY(0.5f)
                        .Rotation(45)
                }
            }
            .VStart()
            .Height(32),

            new Text(step.item)
                .FontName("RubikRegular")
                .FontColor(Colors.Black)
                .Margin(10,0)
                .VerticalAlignment(VerticalAlignment.Top)
        }
        .Margin(15, 5);

    void OnBodyScrolled(object sender, MauiControls.ScrolledEventArgs scrolledArgs)
    {
        var newHeaderHeight = Math.Max(150, 300 - scrolledArgs.ScrollY);
        if (newHeaderHeight != State.HeaderHeight)
        {
            SetState(s =>
            {
                s.HeaderHeight = newHeaderHeight;
            }, invalidateComponent: false);
        }
    }
}
