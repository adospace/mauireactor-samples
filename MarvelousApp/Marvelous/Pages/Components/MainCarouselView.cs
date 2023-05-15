using Marvelous.Models;
using Marvelous.Services;
using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.Pages.Components;

class MainCarouselViewState
{
    public WonderType CurrentType { get; set; }
    public DateTime? StartDrag { get; set; }
    public double PanX { get; set; }
    public double PanY { get; set; }
    public Size ContainerSize { get; set; }
    public ScrollOrientation DraggingOrientation { get; set; } = ScrollOrientation.Neither;
}

class MainCarouselView : Component<MainCarouselViewState>
{
    private Action<WonderType>? _selectedWonderAction;
    private WonderType _initialType;
    private bool _show;

    public MainCarouselView OnSelected(Action<WonderType> action)
    {
        _selectedWonderAction = action;
        return this;
    }

    public MainCarouselView InitialType(WonderType type)
    {
        _initialType = type;
        return this;
    }

    public MainCarouselView Show(bool show)
    {
        _show = show;
        return this;
    }

    public override VisualNode Render()
    {
        return new Grid()
        {
            Enum.GetValues<WonderType>().Select(RenderViewItem),


            new MainCarouselViewIndicator()
                .CurrentType(State.CurrentType)
        }
        .OnSizeChanged(size => SetState(s => s.ContainerSize = size))
        .OnPanUpdated(OnPan)
        .Background(Illustration.Config[State.CurrentType].BackgroundBrush)
        .Opacity(_show ? 1 : 0)
        .WithAnimation()
        ;
    }

    MainCarouselViewItem? RenderViewItem(WonderType wonderType)
    {
        return new MainCarouselViewItem()
            .Type(wonderType)
            .RelativePanX(State.PanX)
            .RelativePanY(State.PanY)
            .CurrentType(State.CurrentType)
            .ContainerSize(State.ContainerSize);
    }

    void OnPan(object? sender, MauiControls.PanUpdatedEventArgs args)
    {
        var container = (MauiControls.Grid?)sender;
        if (container == null)
        {
            return;
        }

        if (args.StatusType == GestureStatus.Started)
        {
            State.StartDrag = DateTime.Now;
        }

        if (args.StatusType == GestureStatus.Started)
        {
            SetState(s =>
            {
                s.PanX = Math.Abs(args.TotalX) > Math.Abs(args.TotalY) ? args.TotalX : 0.0;
                s.PanY = Math.Abs(args.TotalY) > Math.Abs(args.TotalX) ? args.TotalY : 0.0;
                s.ContainerSize = container.Bounds.Size;
            });
        }
        else if (args.StatusType == GestureStatus.Running)
        {
            SetState(s =>
            {
                s.PanX = s.DraggingOrientation == ScrollOrientation.Horizontal ? args.TotalX : 0.0;
                s.PanY = s.DraggingOrientation == ScrollOrientation.Vertical ? args.TotalY : 0.0;
                s.ContainerSize = container.Bounds.Size;
                s.DraggingOrientation = s.DraggingOrientation == ScrollOrientation.Neither ? Math.Abs(args.TotalX) > Math.Abs(args.TotalY) ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical : s.DraggingOrientation;
            });
        }
        else if (args.StatusType == GestureStatus.Canceled)
        {
            SetState(s =>
            {
                s.PanX = 0;
                s.PanY = 0;
                s.ContainerSize = container.Bounds.Size;
                s.DraggingOrientation = ScrollOrientation.Neither;
            });
        }
        else //Completed
        {
            var now = DateTime.Now;

            if (State.StartDrag.HasValue && 
                (((now - State.StartDrag.Value < TimeSpan.FromMilliseconds(200)) && Math.Abs(State.PanY) < 10) || Math.Abs(State.PanX) > State.ContainerSize.Width / 3.0))
            {
                SetState(s =>
                {
                    s.ContainerSize = container.Bounds.Size;
                    s.CurrentType = s.PanX < 0 ?
                        State.CurrentType.Next() :
                        State.CurrentType.Previous();
                    s.PanX = 0;
                    s.PanY = 0;
                    s.DraggingOrientation = ScrollOrientation.Neither;
                });
            }
            else
            {
                if (State.DraggingOrientation == ScrollOrientation.Vertical &&
                    State.ContainerSize.Height > 0 &&
                    Math.Abs(State.PanY) / State.ContainerSize.Height > 0.2)
                {
                    _selectedWonderAction?.Invoke(State.CurrentType);
                }
                SetState(s =>
                {
                    s.PanX = 0;
                    s.PanY = 0;
                    s.ContainerSize = container.Bounds.Size;
                    s.DraggingOrientation = ScrollOrientation.Neither;
                });

            }
        }
    }
}

class MainCarouselViewItemState
{
    public double TranslationX { get; set; }
}

class MainCarouselViewItem : Component<MainCarouselViewItemState>
{
    private WonderType _type;
    private WonderType _currentType;
    private double _relativePanX;
    private double _relativePanY;
    private Size _containerSize;
    private bool _isDragging;

    private bool IsCurrent => _currentType == _type;

    public MainCarouselViewItem Type(WonderType type)
    {
        _type = type;
        return this;
    }

    public MainCarouselViewItem CurrentType(WonderType currentType)
    {
        _currentType = currentType;
        return this;
    }

    public MainCarouselViewItem RelativePanX(double relativePan)
    {
        _relativePanX = relativePan;
        return this;
    }

    public MainCarouselViewItem RelativePanY(double relativePan)
    {
        _relativePanY = relativePan;
        return this;
    }

    public MainCarouselViewItem ContainerSize(Size size)
    {
        _containerSize = size;
        return this;
    }

    public MainCarouselViewItem IsDragging(bool isDragging)
    {
        _isDragging = isDragging;
        return this;
    }

    public override VisualNode Render()
    {
        var config = Illustration.Config[_type];
        var translationX = 0.0;
        var opacity = 0.0;

        var percOpacity = _containerSize.Width > 0 ? Easing.CubicIn.Ease(Math.Abs(_relativePanX / _containerSize.Width)) : 0.0;
        var percVerticalPan = _containerSize.Height > 0 ? (-_relativePanY / _containerSize.Height) : 0.0;

        if (IsCurrent)
        {
            translationX = _relativePanX;
            opacity = 1.0 - percOpacity;
        }
        else if (_relativePanX < 0 && _type.IsNextOf(_currentType))
        {
            translationX = _containerSize.Width + _relativePanX;
            opacity = percOpacity;
        }
        else if (_relativePanX > 0 && _type.IsPreviousOf(_currentType))
        {
            translationX = _relativePanX - _containerSize.Width;
            opacity = percOpacity;
        }
        else if (_type.IsNextOf(_currentType))
        {
            translationX = _containerSize.Width;
        }
        else if (_type.IsPreviousOf(_currentType))
        {
            translationX = - _containerSize.Width;
        }

        return new Grid
        {
            new AbsoluteLayout
            {
                config.BackgroundImages?.Select(RenderIllustrationImage),
            },

            new AbsoluteLayout
            {
                config.MainObjectImage != null ?
                RenderIllustrationImage(config.MainObjectImage)
                    .TranslationX(translationX)
                    .When(!_isDragging, _ => _.WithAnimation(duration: 400))
                    .Opacity(opacity)
                    :null
            },

            new AbsoluteLayout
            {
                config.ForegroundImages?.Select(_=> 
                    RenderIllustrationImage(_)
                        .Scale(1.0 + (float)percVerticalPan * 0.3f)
                        .WithAnimation(duration: 300)
                        ),
            },

            IsCurrent ?
            new Rectangle()
                .Background(Illustration.Config[_currentType].ForegroundBrush)
                : null,

            new Label(Illustration.Config[_currentType].Title)
                .TextColor(Colors.White)
                .FontFamily("YesevaOne")
                .FontSize(58)
                .HCenter()
                .VEnd()
                .WidthRequest(320)
                .HorizontalTextAlignment(TextAlignment.Center)
                .LineHeight(0.8)
                .Margin(0, 120)
                .Opacity(opacity)
                ,

            new Border()
                .Background(new MauiControls.LinearGradientBrush(new MauiControls.GradientStopCollection
                {
                    new MauiControls.GradientStop(Colors.White.WithAlpha(0.0f), 1.0f - (float)percVerticalPan),
                    new MauiControls.GradientStop(Colors.White, 1),
                }, new Point(0, 0), new Point(0, 1)))
                .StrokeShape(new Rectangle().RadiusX(30).RadiusY(30))
                .VCenter()
                .VEnd()
                .Margin(0,25)
                .WidthRequest(50)
                .HeightRequest(800)
                .StrokeThickness(0)
                .Opacity(opacity)
                ,

            new Image("common_arrow_indicator.png")
                .TranslationY(IsCurrent ? 30 : 0)
                .WithAnimation(duration: 200)
                .HCenter()
                .VEnd()
                .Margin(0,70)
                .Opacity(opacity)

        };
    }

    private Image RenderIllustrationImage(IllustrationImage image)
    {
        return new Image(image.Source)
            .Opacity(IsCurrent ? image.Opacity : 0.0)
            .AbsoluteLayoutBounds(IsCurrent ? 
                image.GetFinalBounds(_containerSize) : 
                image.GetInitialBounds(_containerSize))
            .WithAnimation(duration: 400)
            ;
    }
}

class MainCarouselViewIndicator : Component
{
    private WonderType _wonderType;

    public MainCarouselViewIndicator CurrentType(WonderType wonderType)
    {
        _wonderType = wonderType;
        return this;
    }

    public override VisualNode Render()
    {
        return new HStack(spacing: 5)
        {
            Enum.GetValues<WonderType>().Select(RenderIndicatorItem)
        }
        .HeightRequest(10)
        .Margin(0, 80)
        .HCenter()
        .VEnd();
    }

    private VisualNode RenderIndicatorItem(WonderType type, int index)
    {
        return new Border()
            .StrokeShape(new Rectangle().RadiusX(10).RadiusY(10))
            .BackgroundColor(Colors.White)
            .WidthRequest(type == _wonderType ? 20 : 10)
            .WithAnimation(duration: 200);
    }
}

static class WonderTypeExtensions
{
    public static WonderType Next(this WonderType wonderType)
        => wonderType == WonderType.TajMahal ? WonderType.ChichenItza : (WonderType)((int)wonderType + 1);

    public static WonderType Previous(this WonderType wonderType)
        => wonderType == WonderType.ChichenItza ? WonderType.TajMahal : (WonderType)((int)wonderType - 1);

    public static bool IsPreviousOf(this WonderType wonderType, WonderType nextWonderType)
        => nextWonderType.Previous() == wonderType;

    public static bool IsNextOf(this WonderType wonderType, WonderType prevWonderType)
        => prevWonderType.Next() == wonderType;
}