using MauiReactor;
using MauiReactor.Animations;

namespace TrackizerApp.Pages.Components;

class RotatingImageState
{
    public double Rotation { get; set; }
}

partial class RotatingImage : Component<RotatingImageState>
{
    [Prop]
    double _width;

    [Prop]
    double _translationX;

    [Prop]
    double _translationY;

    [Prop]
    string _source;

    [Prop]
    double _duration = 140;

    public override VisualNode Render()
        => Image(
            [.. Children(),
            new AnimationController
            {
                new SequenceAnimation
                {
                    new DoubleAnimation()
                        .StartValue(0)
                        .TargetValue(360)
                        .OnTick(v=>SetState(s => s.Rotation = v, false))
                        .Duration(TimeSpan.FromSeconds(_duration))
                }
                .RepeatForever()
            }
            .IsEnabled(true)
            ])
        .Source(_source)
        .WidthRequest(_width)
        .TranslationX(_translationX) 
        .TranslationY(_translationY)
        .Rotation(()=>State.Rotation);

}
