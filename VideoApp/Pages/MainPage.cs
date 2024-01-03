using MauiReactor;
using MauiReactor.Animations;
using VideoApp.Models;
using VideoApp.Controls;
using System;
using VideoApp.Pages.Components;

namespace VideoApp.Pages;

class MainPageState
{
    public Thickness SafeAreaExtent { get; set; }
    public int CurrentVideoIndex { get; set; }
}

class MainPage : Component<MainPageState>
{
    public override VisualNode Render()
    {
        return ContentPage(
             Grid(
                new InfiniteScroller()
                    .SafeAreaExtent(State.SafeAreaExtent)
                    .OnCurrentVideoChanged(videoIndex => SetState(s => s.CurrentVideoIndex = videoIndex)),

                new FeedbackFlow()
                    .VideoId(State.CurrentVideoIndex >= 0 ? VideoModel.All[State.CurrentVideoIndex].Source.ToString() : null)
            )
        )
        .OnAppearing(OnCalcSafeAreaSize)
        .OniOS(_ => _
            .Set(MauiControls.PlatformConfiguration.iOSSpecific.Page.UseSafeAreaProperty, false));
    }

    private void OnCalcSafeAreaSize(object? sender, EventArgs args)
    {
#if IOS
        if (sender is MauiControls.ContentPage page)
        {
            var safeAreaInsets = Microsoft.Maui.ApplicationModel.WindowStateManager.Default.GetCurrentUIWindow()?.SafeAreaInsets;
            if (safeAreaInsets != null)
            {
                SetState(s => s.SafeAreaExtent = new Thickness(safeAreaInsets.Value.Left, safeAreaInsets.Value.Top, safeAreaInsets.Value.Right, safeAreaInsets.Value.Bottom));
            }
        }
#endif


    }
}

enum ScrollToMode
{
    None,

    Up,

    Down
}

class InfiniteScrollerState
{
    public double PanY { get; set; }

    public Size VideoSize { get; set; }

    public int VideoIndex { get; set; }

    public bool IsPanning { get; set; }

    public ScrollToMode ScrollTo { get; set; }

    public DateTime StartDragTime { get; set; }
}

partial class InfiniteScroller : Component<InfiniteScrollerState>
{
    [Prop]
    private Action<int>? _onCurrentVideoChanged;

    [Prop]
    private Thickness _safeAreaExtent;

    public override VisualNode Render()
    {
        var currentVideo = VideoModel.All[State.VideoIndex];
        var upVideo = VideoModel.All[State.VideoIndex > 0 ? State.VideoIndex - 1 : VideoModel.All.Length - 1];
        var downVideo = VideoModel.All[State.VideoIndex < VideoModel.All.Length - 1 ? State.VideoIndex + 1 : 0];

        return Grid(
            new MediaElement()
                .Aspect(Aspect.AspectFill)
                .ShowsPlaybackControls(false)
                .ShouldAutoPlay(true)
                .ShouldLoopPlayback(true)
                .SourceUri(upVideo.Source)
                .TranslationY(-State.VideoSize.Height + State.PanY)
                ,


            new MediaElement()
                .Aspect(Aspect.AspectFill)
                .ShowsPlaybackControls(false)
                .ShouldAutoPlay(true)
                .ShouldLoopPlayback(true)
                .SourceUri(downVideo.Source)
                .TranslationY(State.VideoSize.Height + State.PanY)
                ,

            new MediaElement()
                .Aspect(Aspect.AspectFill)
                .ShowsPlaybackControls(false)
                .ShouldAutoPlay(true)
                .ShouldLoopPlayback(true)
                .SourceUri(currentVideo.Source)
                .TranslationY(State.PanY)
                ,

            new AnimationController
            {
                new SequenceAnimation
                {
                    new DoubleAnimation()
                        .StartValue(State.PanY)
                        .TargetValue(State.ScrollTo == ScrollToMode.Down ? State.VideoSize.Height : -State.VideoSize.Height)
                        .OnTick(v => SetState(s => s.PanY = v))
                        .Duration(300)
                }
            }
            .IsEnabled(State.ScrollTo != ScrollToMode.None)
            .OnIsEnabledChanged(enabled =>
            {
                SetState(s =>
                {
                    if (!enabled)
                    {
                        s.VideoIndex = s.ScrollTo == ScrollToMode.Up ?
                            (s.VideoIndex < VideoModel.All.Length - 1 ? s.VideoIndex + 1 : 0)
                            :
                            (s.VideoIndex > 0 ? s.VideoIndex - 1 : VideoModel.All.Length - 1);
                        s.ScrollTo = ScrollToMode.None;
                        s.PanY = 0;
                        System.Diagnostics.Debug.WriteLine($"CurrentIndex = {s.VideoIndex}");

                        _onCurrentVideoChanged?.Invoke(s.VideoIndex);
                    }
                });
            })
        )
        .OnSizeChanged(videoSize =>
        {
            SetState(s => s.VideoSize = new Size(videoSize.Width, videoSize.Height - _safeAreaExtent.VerticalThickness));
        })
        .OnPanUpdated(OnPan)
        .Margin(0, -_safeAreaExtent.Top, 0, -_safeAreaExtent.Bottom)
        ;
    }


    void OnPan(object? sender, MauiControls.PanUpdatedEventArgs args)
    {
        //System.Diagnostics.Debug.WriteLine($"PanY: {args.TotalY} Status: {args.StatusType}");
        if (State.VideoSize.IsZero)
        {
            return;
        }

        if (State.ScrollTo != ScrollToMode.None)
        {
            return;
        }

        if (args.StatusType == GestureStatus.Started ||
            args.StatusType == GestureStatus.Running)
        {
            if (args.StatusType == GestureStatus.Started)
            {
                State.StartDragTime = DateTime.Now;
            }

            SetState(s =>
            {
                s.PanY = args.TotalY;
                s.IsPanning = true;
            });
        }
        else if (args.StatusType == GestureStatus.Canceled)
        {
            SetState(s =>
            {
                s.PanY = 0;
                s.IsPanning = true;
            });
        }
        else// if (args.StatusType == GestureStatus.Completed)
        {
            if (Math.Abs(State.PanY) > State.VideoSize.Height / 4.0 ||
                (Math.Abs(State.PanY) > 10 && (DateTime.Now - State.StartDragTime).TotalMilliseconds < 100))
            {
                SetState(s =>
                {
                    s.IsPanning = false;
                    s.ScrollTo = State.PanY > 0 ? ScrollToMode.Down : ScrollToMode.Up;
                });
            }
            else
            {
                SetState(s =>
                {
                    s.PanY = 0;
                    s.IsPanning = false;
                });
            }
        }
    }
}