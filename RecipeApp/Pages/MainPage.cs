using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using MauiReactor.Compatibility;
using MauiReactor.Shapes;

namespace RecipeApp.Pages;

class MainPageState
{
    public int CenterItemId { get; set; }

    public double VerticalOffset { get; set; }

    public double ListWidth { get; set; }

    public int? SelectedRecipeId { get; set; }

    public Rect SelectedRecipeViewPort { get; set; }
}

class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
    {
        return new ContentPage
        {
            new Grid("*", "*")
            {
                new CollectionView()
                    .ItemsSource(RecipesData.dessertMenu, RenderRecipe)
                    .OnScrolled(OnListScrolled)
                    .OnSizeChanged(size => State.ListWidth = size.Width),

                new RecipeComponent()
                    .Recipe(State.SelectedRecipeId != null ? RecipesData.dessertMenu[State.SelectedRecipeId.Value - 1] : null)
                    .OriginalViewPort(State.SelectedRecipeViewPort)
                    .OnCancelSelection(()=>SetState(s => s.SelectedRecipeId = null))
            }
        };
    }

    private void OnListScrolled(object sender, MauiControls.ItemsViewScrolledEventArgs args)
    {
        SetState(s =>
        {
            s.CenterItemId = RecipesData.dessertMenu[args.CenterItemIndex].id;
            s.VerticalOffset = args.VerticalOffset;
        }, invalidateComponent: false);
    }

    VisualNode RenderRecipe(Recipe recipe)
        => new RecipeItemComponent()
            .Recipe(recipe)
            .DistanceFromCenterItem(State.CenterItemId - recipe.id)
            .VerticalOffset(State.VerticalOffset)
            .OnSelected(() => OnRecipeItemSelected(recipe));

    void OnRecipeItemSelected(Recipe recipe)
    {
        double itemYOffset = (recipe.id - 1) * 250 - State.VerticalOffset;

        SetState(s =>
        {
            s.SelectedRecipeId = recipe.id;
            s.SelectedRecipeViewPort = new Rect(0, itemYOffset, State.ListWidth, 250);
        });
    }
}

class RecipeItemComponentState
{
    public double RotationX { get; set; }
}

class RecipeItemComponent : Component<RecipeItemComponentState>
{
    private Recipe _recipe;
    private int _distance;
    private double _verticalOffset;
    private Action _onSelectedAction;

    public RecipeItemComponent Recipe(Recipe recipe)
    {
        _recipe = recipe;
        return this;
    }

    public RecipeItemComponent DistanceFromCenterItem(int distance)
    {
        _distance = distance;
        return this;
    }

    public RecipeItemComponent VerticalOffset(double verticalOffset)
    {
        _verticalOffset = verticalOffset;
        return this;
    }

    public RecipeItemComponent OnSelected(Action action)
    {
        _onSelectedAction = action;
        return this;
    }

    protected override void OnMounted()
    {
        State.RotationX = _distance * 10;
        MauiControls.Application.Current.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(60), () => SetState(s => s.RotationX = 0.0));
        base.OnMounted();
    }

    protected override void OnPropsChanged()
    {
        State.RotationX = _distance * 10;
        MauiControls.Application.Current.Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(60), () => SetState(s => s.RotationX = 0.0));
        base.OnPropsChanged();
    }

    public override VisualNode Render()
    {
        return new CanvasView()
        {
            new DropShadow
            {
                new Box()
                {
                    new Column()
                    {
                        new Text(_recipe.title)
                            .FontSize(32)
                            .VerticalAlignment(VerticalAlignment.Center),

                        new Text(_recipe.description)
                            .FontSize(12)
                            .VerticalAlignment(VerticalAlignment.Top)
                    }
                    .Margin(10,10,140,10)
                }
                .BackgroundColor(_recipe.bgColor)
                .Margin(10, 15)
                .CornerRadius(20)
            }
            .Color(AppColors.blackLight.WithAlpha(0.5f))
            .Blur(30),

            new Align
            {
                new Picture(_recipe.imageSource)
                    .Aspect(Aspect.Fill)
            }
            .VEnd()
            .HEnd()
            .Width(140)
            .Height(140)
            .TranslationX((Math.Abs((float)State.RotationX) / 30.0f) * 140)
            .WithAnimation(easing: ExtendedEasing.InOutBounce, duration: 600)
        }
        .BackgroundColor(Colors.Transparent)
        .HeightRequest(250)
        .Scale(1.0 - (Math.Abs(State.RotationX) / 120.0))
        .RotationX(State.RotationX)
        .WithAnimation(easing: ExtendedEasing.OutCubic, duration: 800)
        .OnTapped(_onSelectedAction);
    }
}



