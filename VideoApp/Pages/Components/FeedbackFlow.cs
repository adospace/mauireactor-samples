using MauiReactor;
using MauiReactor.Animations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoApp.Services;

namespace VideoApp.Pages.Components;

record FeedbackIcon(FeedbackType Type);

class FeedbackFlowState
{
    public FeedbackIcon?[] FeedbackIcons { get; set; } = new FeedbackIcon[20];
}

partial class FeedbackFlow : Component<FeedbackFlowState>
{
    [Prop]
    string? _videoId;

    protected override void OnMountedOrPropsChanged()
    {
        SetupFeedbackService();
        base.OnMountedOrPropsChanged();
    }

    void SetupFeedbackService()
    {
        for (int i = 0; i < State.FeedbackIcons.Length; i++)
        {
            State.FeedbackIcons[i] = null;
        }

        var feedbackService = Services.GetRequiredService<IFeedbackService>();
        if (_videoId != null)
        {
            feedbackService.Connect(_videoId);
            feedbackService.OnFeedbackReceived(OnFeedbackReceived);
        }
        else
        {
            feedbackService.OnFeedbackReceived(null);
        }
    }

    void OnFeedbackReceived(FeedbackType type)
    {
        SetState(s =>
        {
            for (int i = 0; i < State.FeedbackIcons.Length; i++)
            {
                if (s.FeedbackIcons[i] == null)
                {
                    s.FeedbackIcons[i] = new FeedbackIcon(type);
                    break;
                }
            }
        });
    }

    public override VisualNode Render() 
        => Grid(
            [.. State.FeedbackIcons.Select(RenderIcon)]
        );

    private VisualNode RenderIcon(FeedbackIcon? icon, int index)
    {
        return new FeedbackFlowIcon()
            .Icon(icon)
            .OnEnded(() => SetState(s => s.FeedbackIcons[index] = null))
            ;
    }
}


class FeedbackFlowIconState
{ 
    public Point Translate { get; set; }

    public Point StartPoint { get; set; }

    public Point EndPoint { get; set; }

    public Point ControlPoint1 { get; set; }

    public Point ControlPoint2 { get; set; }

    public double Scale { get; set; } = 1.0;

    public double Opacity { get; set; } = 1.0;

    public int Delay { get; set; }

    public bool IsAnimating { get; set; } = true;

    public FeedbackIcon? Icon { get; set; }
}

partial class FeedbackFlowIcon : Component<FeedbackFlowIconState>
{
    [Prop]
    FeedbackIcon? _icon;

    [Prop]
    private Action? _onEnded;

    static readonly Random _rnd = new();

    protected override void OnMountedOrPropsChanged()
    {
        InitializeState();
        base.OnMountedOrPropsChanged();
    }

    void InitializeState()
    {
        if (State.Icon != _icon && _icon != null)
        {
            State.StartPoint = State.Translate = new Point(_rnd.Next(-100, 100), 0);
            State.EndPoint = new Point(State.StartPoint.X, -200 + _rnd.Next(-10, 10));
            State.ControlPoint1 = State.StartPoint.Offset(-20 + _rnd.Next(-5, 5), -50 + _rnd.Next(-10, 10));
            State.ControlPoint2 = State.StartPoint.Offset(+20 + _rnd.Next(-5, 5), -150 + _rnd.Next(-10, 10));
            State.Delay = 4000 + _rnd.Next(-500, 500);
            State.IsAnimating = true;
            State.Icon = _icon;
        }
        else if (_icon == null)
        {
            State.Opacity = 1.0;
            State.Scale = 1.0;
            State.StartPoint = new Point();
            State.EndPoint = new Point();
            State.ControlPoint1 = new Point();
            State.ControlPoint2 = new Point();
            State.Delay = 0;
            State.IsAnimating = false;
            State.Icon = null;
        }
    }

    static string GetEmoji(FeedbackIcon? icon)
        => icon?.Type switch
        {
            FeedbackType.Like => "👍",
            FeedbackType.Love => "❤️",
            FeedbackType.Face => "😀",
            _ => string.Empty,
        };

    public override VisualNode Render()
        => Grid(
            Label(GetEmoji(_icon))
                .FontSize(32)
                .TranslationX(State.Translate.X)
                .TranslationY(State.Translate.Y)
                .Opacity(State.Opacity)
                .TextColor(Colors.Black)
                .Scale(State.Scale)
                .VEnd()
                .HCenter(),

            new AnimationController
            {
                new ParallelAnimation
                {
                    new CubicBezierPathAnimation()
                        .StartPoint(State.StartPoint)
                        .EndPoint(State.EndPoint)
                        .ControlPoint1(State.ControlPoint1)
                        .ControlPoint2(State.ControlPoint2)
                        .OnTick(v => SetState(s => s.Translate = v))
                        .Duration(State.Delay),

                    new SequenceAnimation
                    {
                        new DoubleAnimation()
                            .StartValue(1.0)
                            .TargetValue(0.0)
                            .Easing(ExtendedEasing.InQuint)
                            .OnTick(v => SetState(s => s.Opacity = v))
                            .Duration(1000),
                    }
                    .InitialDelay(State.Delay - 1000),

                    new DoubleAnimation()
                        .StartValue(1.0)
                        .TargetValue(0.8)
                        .Easing(ExtendedEasing.InQuint)
                        .OnTick(v => SetState(s => s.Scale = v))
                        .Duration(State.Delay),
                },
            }
            .OnIsEnabledChanged(enabled =>
            {
                SetState(s => s.IsAnimating = false, false);
                _onEnded?.Invoke();
            })
            .IsEnabled(State.IsAnimating)
        );
}
