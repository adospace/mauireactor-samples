using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using MauiReactor.Compatibility;
using MauiReactor.Shapes;
using OrderingApp.Models;
using OrderingApp.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingApp.Pages;

class MainPageState
{
    public ProductType SelectedType { get; set; }

    public int? SelectedItemIndex { get; set; }
}

class MainPage : Component<MainPageState>
{
    private MauiControls.CollectionView? _listView;

    public override VisualNode Render()
    {
        return ContentPage(
            Grid("260, *", "*",
                new Header()
                    .SelectedType(State.SelectedType)
                    .OnTypeSelected(productType => OnSelectedType(productType, true)),

                CollectionView(listView => _listView = listView)
                    .ItemsSource(ProductItem.Items, RenderProductItem)
                    .OnScrolled(OnListViewScrolled)
                    .GridRow(1)
                    .Margin(24,20),

                new Cart()
                    .Item(State.SelectedItemIndex == null ? null : ProductItem.Items[State.SelectedItemIndex.Value])
                    .OnClose(()=>SetState(s => s.SelectedItemIndex = null))
                    .GridRowSpan(2)
            )
        );
    }

    private VisualNode RenderProductItem(ProductItem item)
    {
        return Border(
            Grid("*", "115,*",
                Image($"{item.Image}.png")
                    .HeightRequest(140)
                    .WidthRequest(140)
                    .TranslationX(-20)
                    ,

                Grid("20,*,24", "*",
                    Label(item.Title)
                        .FontFamily("MulishSemiBold")
                        .FontSize(18)
                        .VStart()
                        .TextColor(Colors.Black),
                    
                    Label(item.Description)
                        .FontFamily("MulishRegular")
                        .FontSize(12)
                        .VCenter()
                        .TextColor(Color.FromArgb("#121212"))
                        .GridRow(1),


                    Label($"${item.Cost}")
                        .FontFamily("MulishSemiBold")
                        .FontSize(16)
                        .VStart()
                        .TextColor(Colors.Black)
                        .GridRow(2)
                )
                .GridColumn(1)
                .Margin(12,15),

                Border(
                    Label("+ ADD")
                        .FontFamily("MulishBold")
                        .TextColor(Colors.White)
                        .VCenter()
                        .HCenter()
                )
                .OnTapped(()=>SetState(s => s.SelectedItemIndex = ProductItem.Items.IndexOf(item)))
                .WidthRequest(78)
                .HeightRequest(34)
                .BackgroundColor(Theme.PrimaryColor)
                .StrokeCornerRadius(13,0,0,13)
                .GridColumn(1)
                .HEnd()
                .VEnd()
            )
        )
        .StrokeCornerRadius(13)
        .Background(new MauiControls.LinearGradientBrush(new MauiControls.GradientStopCollection
            {
                new MauiControls.GradientStop(Theme.PrimaryLightColor, 0.0537f),
                new MauiControls.GradientStop(Colors.White, 0.9738f)
            },
            new Point(0, 0.5),
            new Point(1, 0.5)))
        .Margin(0,0,0,12)
        .HeightRequest(132);
    }

    void OnSelectedType(ProductType productType, bool scrollListView)
    {
        if (State.SelectedType != productType)
        {
            SetState(s => s.SelectedType = productType);

            if (scrollListView && _listView != null)
            {
                switch (productType)
                {
                    case ProductType.Pizza:
                        _listView.ScrollTo(0, position: MauiControls.ScrollToPosition.Start);
                        break;
                    case ProductType.Salad:
                        _listView.ScrollTo(4, position: MauiControls.ScrollToPosition.Start);
                        break;
                    case ProductType.Dessert:
                        _listView.ScrollTo(8, position: MauiControls.ScrollToPosition.Start);
                        break;
                    case ProductType.Sides:
                        _listView.ScrollTo(10, position: MauiControls.ScrollToPosition.Start);
                        break;
                    case ProductType.Drinks:
                        _listView.ScrollTo(12, position: MauiControls.ScrollToPosition.Start);
                        break;
                }

            }
        }
    }

    void OnListViewScrolled(object? sender, MauiControls.ItemsViewScrolledEventArgs args)
    {
        if (args.VerticalOffset < 132 * 4)
        {
            OnSelectedType(ProductType.Pizza, false);
        }
        else if (args.VerticalOffset < 132 * 8)
        {
            OnSelectedType(ProductType.Salad, false);
        }
        else if (args.VerticalOffset < 132 * 10)
        {
            OnSelectedType(ProductType.Dessert, false);
        }
        else if (args.VerticalOffset < 132 * 12)
        {
            OnSelectedType(ProductType.Sides, false);
        }
        else
        {
            OnSelectedType(ProductType.Drinks, false);
        }
    }
}

class HeaderState
{
    public double SelectedTypeLeftMargin { get; set; }
}

class Header : Component<HeaderState>
{
    private ProductType _selectedType;
    private Action<ProductType>? _selectAction;
    private MauiControls.ScrollView? _scrollView;

    public Header SelectedType(ProductType selectedType)
    {
        _selectedType = selectedType;
        return this;
    }

    public Header OnTypeSelected(Action<ProductType> action)
    {
        _selectAction = action;
        return this;
    }

    protected override void OnMounted()
    {
        State.SelectedTypeLeftMargin = 90 * (int)_selectedType;
        base.OnMounted();
    }

    protected override void OnPropsChanged()
    {
        State.SelectedTypeLeftMargin = 90 * (int)_selectedType;

        MauiControls.Application.Current?.Dispatcher.Dispatch(async () =>
        {
            var targetX = 90 * (int)_selectedType;
            if (_scrollView != null && targetX != _scrollView.ScaleX)
            {
                await _scrollView.ScrollToAsync(targetX, 0, true);
            }
        });

        base.OnPropsChanged();
    }

    public override VisualNode Render()
    {
        return Grid("62,49,130.0", "*",
            Grid("30", "39,30,*,110",
                Image("menu_icon.png")
                    .Margin(0,3,12,3)
                    .Aspect(Aspect.AspectFit),

                Image("dodo.png")
                    .GridColumn(1),

                Image("title.png")
                    .GridColumn(2)
                    .Margin(12,0),

                HStack(spacing: 2,
                    Label("DELIVERY")
                        .VCenter()
                        .TextColor(Theme.PrimaryColor),

                    Image("chevron_down.png")
                        .VCenter()
                )
                .HEnd()
                .GridColumn(3)
            )
            .Margin(24,32,24,0),

            Border(
                Grid("32", "*",
                    Label("29 Hola street, California, USA")
                        .Margin(8,0)
                        .FontSize(14)
                        .FontFamily("MulishSemiBold")
                        .TextColor(Theme.PrimaryColor)
                        .VerticalTextAlignment(TextAlignment.Center),

                    Image("pin.png")
                        .HeightRequest(16)
                        .Margin(8,0)
                        .HEnd()
                        .VCenter()
                )
            )
            .BackgroundColor(Theme.PrimaryColor.WithAlpha(0.1f))
            .StrokeThickness(0)
            .StrokeShape(new RoundRectangle().CornerRadius(4))
            .GridRow(1)
            .Margin(24, 16.5, 24, 0),
        
            ScrollView(scrollView => _scrollView = scrollView,
                Grid(
                    HStack(spacing:10,
                        [.. Enum.GetValues<ProductType>().Select(RenderProductType)]
                    ),

                    RenderSelectedBorder()
                )
            )
            .Margin(0,20.5,0,0)
            .Orientation(ScrollOrientation.Horizontal)
            .Padding(24,0,24,4)
            .GridRow(2)
        )
        .BackgroundColor(Theme.PrimaryLightColor);
    }

    Border RenderProductType(ProductType type)
    {
        return Border(
            Grid("*, 20", "*",
                Image($"{type.ToString().ToLowerInvariant()}.png")
                    .HeightRequest(64)
                    .WidthRequest(64)
                    .HCenter()
                    .VCenter(),

                Label(type.ToString())
                    .FontFamily("MulishSemiBold")
                    .TextColor(Colors.Black)
                    .FontSize(14)
                    .GridRow(1)
                    .HorizontalTextAlignment(TextAlignment.Center)
                    .VerticalTextAlignment(TextAlignment.Center)
            )
            .Padding(8)
            .BackgroundColor(Theme.PrimaryLightColor)
        )
        .WidthRequest(80)
        .OnTapped(()=>_selectAction?.Invoke(type))
        .BackgroundColor(Theme.PrimaryLightColor)
        .StrokeShape(new RoundRectangle().CornerRadius(12))
        ;
    }

    VisualNode RenderSelectedBorder()
    {
        return Border()
            .BackgroundColor(Colors.Transparent)
            .WidthRequest(80)
            .StrokeThickness(2)
            .Stroke(Theme.PrimaryColor)
            .StrokeShape(new RoundRectangle().CornerRadius(12))
            .HStart()
            .TranslationX(State.SelectedTypeLeftMargin)
            .WithAnimation(duration: 300)
            ;
    }
}

class CartState
{
    public double TranslateY { get; set; } = 660;

    public double SizeCost { get; set; } = 12;

    public double CrustCost { get; set; } = 1.5;

    public double AddOn1Cost { get; set; } = 0;

    public double AddOn2Cost { get; set; } = 0;
}

partial class Cart : Component<CartState>
{
    [Prop]
    private ProductItem? _item;

    [Prop]
    private Action? _onClose;

    protected override void OnMountedOrPropsChanged()
    {
        State.TranslateY = _item != null ? 0 : 660;
        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        return Grid(
            Grid("*,78", "*",
                RenderBody(),

                RenderBottom()
            )
            .TranslationY(State.TranslateY)
            .WithAnimation(duration: 300, easing: Easing.CubicOut)
            .HeightRequest(600)
            .VEnd()
            .BackgroundColor(Colors.White)
        )
        .BackgroundColor(_item != null ? Colors.Black.WithAlpha(0.8f) : Colors.Transparent)
        .IsVisible(_item != null)
        ;
    }

    private Grid? RenderBody()
    {
        if (_item == null)
        {
            return null;
        }

        return Grid("66,*", "*",
            Grid("*", "*,24",
                new Label(_item.Title)
                    .FontFamily("MulishBold")
                    .FontSize(18)
                    .TextColor(Colors.Black),

                new Image("close.png")                    
                    .GridColumn(1)
                    .OnTapped(_onClose)
            )
            .Margin(24,20),

            VStack(spacing: 20,
                new CartItemGroup
                {
                    RenderCartItem("Small - 6''", 8, State.SizeCost == 8, ()=>SetState(s => s.SizeCost = 8)),
                    RenderCartItem("Medium - 10''", 12, State.SizeCost == 12, ()=>SetState(s => s.SizeCost = 12)),
                    RenderCartItem("Large - 14''", 16, State.SizeCost == 16, ()=>SetState(s => s.SizeCost = 16)),
                }
                .Title("Choose Size"),

                new CartItemGroup
                {
                    RenderCartItem("Classic Hand tossed", 0, State.CrustCost == -1, ()=>SetState(s => s.CrustCost = -1)),
                    RenderCartItem("Thin Crust", 0, State.CrustCost == -2, ()=>SetState(s => s.CrustCost = -2)),
                    RenderCartItem("Cheese Brust", 1.5, State.CrustCost == 1.5, ()=>SetState(s => s.CrustCost = 1.5)),
                }
                .Title("Select Crust"),

                new CartItemGroup
                {
                    RenderCartItem("Add Extra Cheese", 2.5, State.AddOn1Cost == 2.5, ()=>SetState(s => s.AddOn1Cost = s.AddOn1Cost == 0 ? 2.5 : 0), checkBox: true),
                    RenderCartItem("Add Mushroom", 2.5, State.AddOn2Cost == 2.5, ()=>SetState(s => s.AddOn2Cost = s.AddOn2Cost == 0 ? 2.5 : 0), checkBox: true),
                }
                .Title("Add ons")
                .Optional(true)

            )
            .Margin(24,0)
            .GridRow(1)
        );
    }

    CartItem RenderCartItem(
        string label, 
        double cost, 
        bool selected,
        Action selectedAction,
        bool checkBox = false)
        => new CartItem()
            .Label(label)
            .Cost(cost)
            .Selected(selected)
            .OnSelected(selectedAction)
            .CheckBox(checkBox);

    private VisualNode RenderBottom()
    {
        return Grid("*", "*",
            Rectangle()
                .Fill(Color.FromArgb("#E6E6E6"))
                .HeightRequest(2)
                .GridColumnSpan(2)
                .VStart(),

            new Total()
                .Cost(new[]{ State.SizeCost, State.CrustCost, State.AddOn1Cost, State.AddOn2Cost }.Where(_=>_>0).Sum())
        )
        .GridRow(1);
    }
}

class CartItemGroup : Component
{
    private string? _title;
    private bool _optional;

    public CartItemGroup Title(string title)
    {
        _title = title;
        return this;
    }

    public CartItemGroup Optional(bool optional)
    {
        _optional = optional;
        return this;
    }

    public override VisualNode Render()
    {
        return new Grid("22,*", "*")
        {
            new Grid("22","*,76")
            {
                new Label(_title)
                    .FontFamily("MulishBold")
                    .FontSize(14)
                    .TextColor(Colors.Black)
                    .VStart()
                ,
                new Image(_optional ? "optional" : "required.png")
                    .Aspect(Aspect.AspectFit)
                    .GridColumn(1)
            },

            new VStack(spacing: 0)
            {
                RenderChildren(Children())
            }
            .GridRow(1)
        };
    }

    private IEnumerable<VisualNode> RenderChildren(IReadOnlyList<VisualNode> childList)
    {
        for (int i = 0; i < childList.Count; i++)
        {
            yield return childList[i];

            if (i < childList.Count - 1)
            {
                yield return new Rectangle()
                    .Margin(32,0,0,0)
                    .Fill(Color.FromArgb("#E6E6E6"))
                    .HeightRequest(2);
            }
        }
    }
}

partial class CartItem : Component
{
    [Prop]
    private Action? _onSelected;

    [Prop]
    private string? _label;
    
    [Prop]
    private bool _checkBox;

    [Prop]
    private double _cost;

    [Prop]
    private bool _selected;

    public override VisualNode Render()
    {
        return Grid("40", "24,*,30",
            _checkBox ? RenderCheckBox() : RenderRadio(),

            Component.Label(_label)
                .FontFamily("MulishRegular")
                .FontSize(12)
                .TextColor(Colors.Black)
                .GridColumn(1)
                .VCenter()
                .Margin(9,0),

            Component.Label($"${_cost}")
                .FontFamily("MulishRegular")
                .FontSize(12)
                .TextColor(Colors.Black)
                .VCenter()
                .GridColumn(2)
                .IsVisible(_cost > 0)
        )
        .OnTapped(_onSelected);
    }

    CanvasView RenderRadio()
    {
        return new CanvasView
        {
            new Row("24,*,32")
            {
                new Group
                {
                    new MauiReactor.Canvas.Ellipse()
                        .Margin(4)
                        .StrokeColor(_selected ? Theme.PrimaryColor : Color.FromArgb("#CCCCCC")),

                    new MauiReactor.Canvas.Ellipse()
                        .Margin(7)
                        .FillColor(_selected ? Theme.PrimaryColor : Colors.Transparent),
                }
            }
        }
        .BackgroundColor(Colors.Transparent)
        .HeightRequest(24)
        .WidthRequest(24);
    }

    CanvasView RenderCheckBox()
    {
        return new CanvasView
        {
            new Row("24,*,32")
            {
                new Group
                {
                    new Box()
                        .Margin(4)
                        .BorderColor(_selected ? Theme.PrimaryColor : Color.FromArgb("#CCCCCC")),

                    new Box()
                        .Margin(7)
                        .BackgroundColor(_selected ? Theme.PrimaryColor : Colors.Transparent),
                }
            }
        }
        .OnTapped(_onSelected)
        .BackgroundColor(Colors.Transparent)
        .HeightRequest(24)
        .WidthRequest(24);
    }
}

class TotalState
{
    public double Total { get; set; }

    public bool IsConterAnimationEnabled { get; set; }

    public double TargetValue { get; set; }
}

class Total : Component<TotalState>
{
    private double _totalValue;

    public Total Cost(double totalValue)
    {
        _totalValue = totalValue;
        return this;
    }

    protected override void OnMounted()
    {
        State.TargetValue = _totalValue;
        State.IsConterAnimationEnabled = true;

        base.OnMounted();
    }

    protected override void OnPropsChanged()
    {
        State.TargetValue = _totalValue;
        State.IsConterAnimationEnabled = true;
        
        base.OnPropsChanged();
    }

    public override VisualNode Render()
    {
        return Grid("*", "*, 120",
            Button("+ ADD TO CART")
                .FontSize(14)
                .FontFamily("MulishSemiBold")
                .TextColor(Colors.White)
                .BackgroundColor(Theme.PrimaryColor)
                .CornerRadius(8)
                .HFill(),

            Label()
                .FontFamily("MulishSemiBold")
                .FontSize(16)
                .TextColor(Colors.Black)
                .FontAttributes(MauiControls.FontAttributes.Bold)
                .HCenter()
                .VCenter()
                .Text(()=>State.Total.ToString("$0.00"))
                .HEnd()
                .GridColumn(1),

            new AnimationController
            {
                new SequenceAnimation
                {
                    new DoubleAnimation()
                        .StartValue(State.Total)
                        .TargetValue(State.TargetValue)
                        .OnTick(v => SetState(s => s.Total = v)),
                }
                .IterationCount(1)
            }
            .IsEnabled(State.IsConterAnimationEnabled)
            .OnIsEnabledChanged(enabled => SetState(s => s.IsConterAnimationEnabled = enabled))
        )
        .Margin(24,20);
    }
}