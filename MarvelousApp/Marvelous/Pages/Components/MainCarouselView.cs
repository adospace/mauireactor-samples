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

class DragStateInfo
{
    public DateTime? StartDrag { get; set; }
    public double PanX { get; set; }
    public double PanY { get; set; }
    public Size ContainerSize { get; set; }
    public ScrollOrientation DraggingOrientation { get; set; } = ScrollOrientation.Neither;
    public WonderType CurrentType { get; set; }

}

class MainCarouselViewState
{
    //public WonderType CurrentType { get; set; }
    public DragStateInfo DragInfo { get; set; } = new();
}

partial class MainCarouselView : Component<MainCarouselViewState>
{
    event EventHandler<DragStateInfo>? DragStateChanged;

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
                .WonderType(State.DragInfo.CurrentType)]
        )
        .OnSizeChanged(size =>
        {
            State.DragInfo.ContainerSize = size;
            DragStateChanged?.Invoke(this, State.DragInfo);
        })
        .OnPanUpdated(OnPan)
        .Background(Illustration.Config[State.DragInfo.CurrentType].BackgroundBrush)
        .Opacity(_show ? 1 : 0)
        .WithAnimation()
        ;

    MainCarouselViewItem? RenderViewItem(WonderType wonderType) 
        => new MainCarouselViewItem()
            .Type(wonderType)
            //.CurrentType(State.CurrentType)
            .OnDragStateChanged(handler => DragStateChanged += handler)
            //.RelativePanX(State.PanX)
            //.RelativePanY(State.PanY)
            //.ContainerSize(State.ContainerSize)
            //.IsDragging(State.StartDrag != null)
        ;

    void OnPan(object? sender, MauiControls.PanUpdatedEventArgs args)
    {
        var container = (MauiControls.Grid?)sender;
        if (container == null)
        {
            return;
        }

        var dragInfo = State.DragInfo;

        if (args.StatusType == GestureStatus.Started)
        {
            dragInfo.StartDrag = DateTime.Now;
        }

        if (args.StatusType == GestureStatus.Started)
        {
            dragInfo.PanX = Math.Abs(args.TotalX) > Math.Abs(args.TotalY) ? args.TotalX : 0.0;
            dragInfo.PanY = Math.Abs(args.TotalY) > Math.Abs(args.TotalX) ? args.TotalY : 0.0;
            dragInfo.ContainerSize = container.Bounds.Size;
        }
        else if (args.StatusType == GestureStatus.Running)
        {
            dragInfo.PanX = dragInfo.DraggingOrientation == ScrollOrientation.Horizontal ? args.TotalX : 0.0;
            dragInfo.PanY = dragInfo.DraggingOrientation == ScrollOrientation.Vertical ? args.TotalY : 0.0;
            dragInfo.ContainerSize = container.Bounds.Size;
            dragInfo.DraggingOrientation = dragInfo.DraggingOrientation == ScrollOrientation.Neither ? Math.Abs(args.TotalX) > Math.Abs(args.TotalY) ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical : dragInfo.DraggingOrientation;
        }
        else if (args.StatusType == GestureStatus.Canceled)
        {
            dragInfo.PanX = 0;
            dragInfo.PanY = 0;
            dragInfo.ContainerSize = container.Bounds.Size;
            dragInfo.DraggingOrientation = ScrollOrientation.Neither;
            dragInfo.StartDrag = null;
        }
        else //Completed
        {
            if (dragInfo.StartDrag.HasValue && 
                (((DateTime.Now - dragInfo.StartDrag.Value < TimeSpan.FromMilliseconds(200)) && Math.Abs(dragInfo.PanY) < 10) || Math.Abs(dragInfo.PanX) > dragInfo.ContainerSize.Width / 3.0))
            {                
                SetState(s =>
                {
                    s.DragInfo.ContainerSize = container.Bounds.Size;
                    s.DragInfo.CurrentType = dragInfo.PanX < 0 ?
                            s.DragInfo.CurrentType.Next() :
                            s.DragInfo.CurrentType.Previous();
                    s.DragInfo.PanX = 0;
                    s.DragInfo.PanY = 0;
                    s.DragInfo.DraggingOrientation = ScrollOrientation.Neither;
                    s.DragInfo.StartDrag = null;
                });
            }
            else
            {
                if (dragInfo.DraggingOrientation == ScrollOrientation.Vertical &&
                    dragInfo.ContainerSize.Height > 0 &&
                    Math.Abs(dragInfo.PanY) / dragInfo.ContainerSize.Height > 0.2)
                {
                    _onSelected?.Invoke(State.DragInfo.CurrentType);
                }

                dragInfo.PanX = 0;
                dragInfo.PanY = 0;
                dragInfo.ContainerSize = container.Bounds.Size;
                dragInfo.DraggingOrientation = ScrollOrientation.Neither;
                dragInfo.StartDrag = null;
            }
        }

        DragStateChanged?.Invoke(this, dragInfo);
    }
}

class MainCarouselViewItemState
{
    public bool IsDragging { get; set; }

    public double Opacity { get; set;}

    public double TranslationX { get; set;}

    public Size ContainerSize { get; set;}

    public double PercHorizontalPan { get; set; }

    public double PercVerticalPan { get; set; }

    public WonderType CurrentType { get; set; }
}

partial class MainCarouselViewItem : Component<MainCarouselViewItemState>
{
    [Prop]
    WonderType _type;

    [Prop]
    Action<EventHandler<DragStateInfo>>? _onDragStateChanged;

    bool IsCurrent => State.CurrentType == _type;

    protected override void OnMountedOrPropsChanged()
    {
        _onDragStateChanged?.Invoke(OnDragStateChanged);
        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        var illustrationConfig = Illustration.Config[_type];

        //System.Diagnostics.Debug.WriteLine($"RenderIllustration({_type}(IsCurrent={IsCurrent}) Opacity:{State.Opacity}, TransX:{State.TranslationX}, PercHPan:{State.PercHorizontalPan}, PercVPan:{State.PercVerticalPan})");

        if (State.ContainerSize.IsZero)
        {
            return Grid();
        }

        return Grid(
            AbsoluteLayout(
                [.. illustrationConfig.BackgroundImages.Select(RenderIllustrationImage)]
            ),

            AbsoluteLayout(
                RenderIllustrationImage(illustrationConfig.MainObjectImage)
                    .TranslationX(State.TranslationX)
                    .Opacity(State.Opacity)
                    .When(!State.IsDragging, _=>_.WithAnimation(duration: 300))
            ),

            AbsoluteLayout([.. illustrationConfig.ForegroundImages.Select(_=> 
                RenderIllustrationImage(_)
                    .Scale(1.0 + (float)State.PercVerticalPan * 0.3f)
                    .WithAnimation(duration: 300)
                    )]
            ),

            IsCurrent ?
            Rectangle()
                .Background(illustrationConfig.ForegroundBrush)
                : null,

            Label(illustrationConfig.Title)
                .Opacity(State.Opacity)
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
                    (Colors.White.WithAlpha(0.0f), 1.0f - (float)State.PercVerticalPan),
                    (Colors.White, 1)
                ))
                .StrokeCornerRadius(30)
                .VCenter()
                .VEnd()
                .Margin(0,25)
                .WidthRequest(50)
                .HeightRequest(800)
                .StrokeThickness(0)
                .Opacity(State.Opacity)
                ,

            Image("common_arrow_indicator.png")
                .HCenter()
                .VEnd()
                .Margin(0,30)
                .Opacity(State.Opacity)

        );
    }

    Image RenderIllustrationImage(IllustrationImage image) 
        => Image(image.Source)
            .Opacity(image.Opacity * State.Opacity)
            .AbsoluteLayoutBounds(IsCurrent ?
                image.GetFinalBounds(State.ContainerSize) :
                image.GetInitialBounds(State.ContainerSize))
            .WithAnimation(duration: 400)
            ;

    void OnDragStateChanged(object? sender, DragStateInfo dragInfo)
    {
        var config = Illustration.Config[_type];
        var translationX = 0.0;
        var opacity = 0.0;
        var dragging = dragInfo.StartDrag != null && _type == dragInfo.CurrentType;

        var percHorizontalPan = dragInfo.ContainerSize.Width > 0 ? Easing.CubicIn.Ease(Math.Abs(dragInfo.PanX / dragInfo.ContainerSize.Width)) : 0.0;
        var percVerticalPan = dragInfo.ContainerSize.Height > 0 ? (-dragInfo.PanY / dragInfo.ContainerSize.Height) : 0.0;

        if (_type == dragInfo.CurrentType)
        {
            translationX = dragInfo.PanX;
            opacity = 1.0 - percHorizontalPan;
        }
        else if (dragInfo.PanX < 0 && _type.IsNextOf(dragInfo.CurrentType))
        {
            translationX = dragInfo.ContainerSize.Width + dragInfo.PanX;
            opacity = percHorizontalPan;
        }
        else if (dragInfo.PanX > 0 && _type.IsPreviousOf(dragInfo.CurrentType))
        {
            translationX = dragInfo.PanX - dragInfo.ContainerSize.Width;
            opacity = percHorizontalPan;
        }
        else if (_type.IsNextOf(dragInfo.CurrentType))
        {
            translationX = dragInfo.ContainerSize.Width;
        }
        else if (_type.IsPreviousOf(dragInfo.CurrentType))
        {
            translationX = -dragInfo.ContainerSize.Width;
        }

        if (State.ContainerSize != dragInfo.ContainerSize ||
            State.CurrentType != dragInfo.CurrentType ||
            State.Opacity != opacity ||
            State.TranslationX != translationX ||
            State.PercHorizontalPan != percHorizontalPan ||
            State.PercVerticalPan != percVerticalPan)
        {
            bool containerSizeChanged = State.ContainerSize != dragInfo.ContainerSize;
            bool draggingChanged = State.IsDragging != dragging;
            SetState(s => 
            {
                s.Opacity = opacity;
                s.TranslationX = translationX;
                s.ContainerSize = dragInfo.ContainerSize;
                s.PercVerticalPan = percVerticalPan;
                s.PercHorizontalPan = percHorizontalPan;
                s.CurrentType = dragInfo.CurrentType;
                s.IsDragging = dragging;
            }, containerSizeChanged || dragging);
        }
    }
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