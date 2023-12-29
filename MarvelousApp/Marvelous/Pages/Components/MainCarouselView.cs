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

partial class MainCarouselView : Component<MainCarouselViewState>
{
    [Prop]
    Action<WonderType>? _onSelected;

    [Prop]
    WonderType _initialType;

    [Prop]
    bool _show;

    public override VisualNode Render() 
        => Grid(
            [.. Enum.GetValues<WonderType>().Select(RenderViewItem),

            new MainCarouselViewIndicator()
                .WonderType(State.CurrentType)]
        )
        .OnSizeChanged(size => SetState(s => s.ContainerSize = size))
        .OnPanUpdated(OnPan)
        .Background(Illustration.Config[State.CurrentType].BackgroundBrush)
        .Opacity(_show ? 1 : 0)
        .WithAnimation()
        ;

    MainCarouselViewItem? RenderViewItem(WonderType wonderType) 
        => new MainCarouselViewItem()
            .Type(wonderType)
            .RelativePanX(State.PanX)
            .RelativePanY(State.PanY)
            .CurrentType(State.CurrentType)
            .ContainerSize(State.ContainerSize);

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
            if (State.StartDrag.HasValue && 
                (((DateTime.Now - State.StartDrag.Value < TimeSpan.FromMilliseconds(200)) && Math.Abs(State.PanY) < 10) || Math.Abs(State.PanX) > State.ContainerSize.Width / 3.0))
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
                    _onSelected?.Invoke(State.CurrentType);
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

partial class MainCarouselViewItem : Component
{
    [Prop]
    WonderType _type;

    [Prop]
    WonderType _currentType;

    [Prop]
    double _relativePanX;

    [Prop]
    double _relativePanY;

    [Prop]
    Size _containerSize;

    [Prop]
    bool _isDragging;

    bool IsCurrent => _currentType == _type;

    public override VisualNode Render()
    {
        var config = Illustration.Config[_type];
        var translationX = 0.0;
        var opacity = 0.0;

        var percOpacity = _containerSize.Width > 0 ? Easing.CubicIn.Ease(Math.Abs(_relativePanX / _containerSize.Width)) : 0.0;
        var percVerticalPan = _containerSize.Height > 0 ? (-_relativePanY / _containerSize.Height) : 0.0;
        var percOpacityInverse = _containerSize.Width > 0 ? Easing.CubicOut.Ease(Math.Abs(_relativePanX / _containerSize.Width)) : 0.0;

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

        return Grid(
            AbsoluteLayout(
                [.. config.BackgroundImages?.Select(RenderIllustrationImage)]
            ),

            AbsoluteLayout(
                config.MainObjectImage != null ?
                RenderIllustrationImage(config.MainObjectImage)
                    .TranslationX(translationX)
                    .When(!_isDragging, _ => _.WithAnimation(duration: 400))
                    .Opacity(opacity)
                    :null
            ),

            AbsoluteLayout([.. config.ForegroundImages?.Select(_=> 
                RenderIllustrationImage(_)
                    .Scale(1.0 + (float)percVerticalPan * 0.3f)
                    .WithAnimation(duration: 300)
                    )]
            ),

            IsCurrent ?
            Rectangle()
                .Background(Illustration.Config[_currentType].ForegroundBrush)
                : null,

            Label(Illustration.Config[_currentType].Title)
                .Opacity(opacity)
                .WithAnimation()
                .TextColor(Colors.White)
                .FontFamily("YesevaOne")
                .FontSize(58)
                .HCenter()
                .VEnd()
                .WidthRequest(320)
                .HorizontalTextAlignment(TextAlignment.Center)
                .LineHeight(0.8)
                .Margin(0, 120)
                ,

            Border()
                .Background(new LinearGradient(180.0,
                    (Colors.White.WithAlpha(0.0f), 1.0f - (float)percVerticalPan),
                    (Colors.White, 1)
                ))
                .StrokeCornerRadius(30)
                .VCenter()
                .VEnd()
                .Margin(0,25)
                .WidthRequest(50)
                .HeightRequest(800)
                .StrokeThickness(0)
                .Opacity(opacity)
                ,

            Image("common_arrow_indicator.png")
                .HCenter()
                .VEnd()
                .Margin(0,30)
                .Opacity(opacity)

        );
    }

    Image RenderIllustrationImage(IllustrationImage image) 
        => Image(image.Source)
            .Opacity(IsCurrent ? image.Opacity : 0.0)
            .AbsoluteLayoutBounds(IsCurrent ?
                image.GetFinalBounds(_containerSize) :
                image.GetInitialBounds(_containerSize))
            .WithAnimation(duration: 400)
            ;
}

partial class MainCarouselViewIndicator : Component
{
    [Prop]
    WonderType _wonderType;

    public override VisualNode Render() 
        => HStack(spacing: 5,
            [.. Enum.GetValues<WonderType>().Select(RenderIndicatorItem)]
        )
        .HeightRequest(10)
        .Margin(0, 80)
        .HCenter()
        .VEnd();

    VisualNode RenderIndicatorItem(WonderType type, int index)
        => Border()
            .StrokeShape(new Rectangle().RadiusX(10).RadiusY(10))
            .BackgroundColor(Colors.White)
            .WidthRequest(type == _wonderType ? 20 : 10)
            .WithAnimation(duration: 200);
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