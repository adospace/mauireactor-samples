using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;

namespace DigitsGame.Pages.Components;

class AnimatingButtonState
{
    public float TranslateX { get; set; }

    public bool InError { get; set; }
}

partial class AnimatingButton : Component<AnimatingButtonState>
{
    [Prop]
    bool _inError;

    protected override void OnPropsChanged()
    {
        State.InError = _inError;
        System.Diagnostics.Debug.WriteLine($"AnimatingButton.OnPropsChanged(State.InError={State.InError})");

        base.OnPropsChanged();
    }

    public override VisualNode Render()
    {
        return new Group
        {
            new Align
            {
                Children()[0]
            },

            new AnimationController
            {
                new SequenceAnimation
                {
                    new DoubleAnimation()
                        .StartValue(0.0)
                        .TargetValue(-10)
                        .OnTick(v => SetState(s => s.TranslateX = (float)v))
                        .Duration(50),

                    new DoubleAnimation()
                        .StartValue(-10.0)
                        .TargetValue(10)
                        .OnTick(v => SetState(s => s.TranslateX = (float)v))
                        .Duration(100),

                    new DoubleAnimation()
                        .StartValue(10.0)
                        .TargetValue(0)
                        .OnTick(v => SetState(s => s.TranslateX = (float)v))
                        .Duration(50),
                }
            }
            .IsEnabled(State.InError)
            .OnIsEnabledChanged(enabled => SetState(s => s.InError = false))
        }
        .TranslationX(State.TranslateX);
    }


}
