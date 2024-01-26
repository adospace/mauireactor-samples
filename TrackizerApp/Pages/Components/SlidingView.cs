using MauiReactor;
using MauiReactor.Animations;
using System.Linq;

namespace TrackizerApp.Pages.Components;

class SlidingViewState
{
    public double ViewportWidth {  get; set; }

    public int SelectedIndex {  get; set; } = 0;

    public double PanningX { get; set; }

    public bool AnimatingPan {  get; set; }
}

partial class SlidingView : Component<SlidingViewState>
{
    [Prop]
    Action<int>? _onSelectedViewIndex;

    [Prop]
    int _selectedIndex;

    protected override void OnMountedOrPropsChanged()
    {
        if (State.SelectedIndex != _selectedIndex)
        {
            State.AnimatingPan = true;
            State.PanningX = State.ViewportWidth * (_selectedIndex - State.SelectedIndex);
            State.SelectedIndex = _selectedIndex;
        }

        base.OnMountedOrPropsChanged();
    }

    double GetTranslationX(int indexOfItem)
    {
        var translationX = State.ViewportWidth * indexOfItem + State.PanningX - State.SelectedIndex * State.ViewportWidth;
        if (translationX > State.ViewportWidth)
        {
            translationX -= Children().Count * State.ViewportWidth;
        }
        else if (translationX < -State.ViewportWidth)
        {
            translationX += Children().Count * State.ViewportWidth;
        }
        return translationX;
    }

    public override VisualNode Render()
        => Grid(
            [..
            Children()
                .Select((item, index) =>
                    Grid(item)
                        .TranslationX(() => GetTranslationX(index)))
                .ToArray(),

            new AnimationController
            {
                new SequenceAnimation
                {
                    new DoubleAnimation()
                        .StartValue(State.PanningX)
                        .TargetValue(0)
                        .Duration(1200)
                        .OnTick(v => SetState(s => s.PanningX = v, false))
                }
            }
            .OnIsEnabledChanged(enabled => 
            {
                if (!enabled)
                {
                    if (State.AnimatingPan)
                    {
                        SetState(s => s.AnimatingPan = false);

                        _onSelectedViewIndex?.Invoke(State.SelectedIndex);
                    }
                }
            })
            .IsEnabled(State.AnimatingPan)
            ]
        )
        .OnPanUpdated(PanUpdated)
        .OnSizeChanged(size => SetState(s => s.ViewportWidth = size.Width));

    void PanUpdated(MauiControls.PanUpdatedEventArgs args)
    {
        if (args.StatusType == GestureStatus.Canceled)
        {
            SetState(s => s.PanningX = 0);
        }
        else if (args.StatusType == GestureStatus.Running)
        {
            SetState(s => s.PanningX = args.TotalX, false);
        }
        else if (args.StatusType == GestureStatus.Completed)
        {
            if (State.PanningX < -10)
            {
                SetState(s =>
                {
                    s.AnimatingPan = true;
                    s.PanningX += State.ViewportWidth;
                    s.SelectedIndex = s.SelectedIndex == Children().Count - 1 ? 0 : s.SelectedIndex + 1;
                });

            }
            else if (State.PanningX > 10)
            {
                SetState(s =>
                {
                    s.AnimatingPan = true;
                    s.PanningX -= State.ViewportWidth;
                    s.SelectedIndex = s.SelectedIndex == 0 ? Children().Count - 1 : s.SelectedIndex - 1;
                });
            }
            else
            {
                SetState(s => s.PanningX = 0, false);
            }
        }
    }
}
