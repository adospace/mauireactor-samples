using DigitsGame.Models;
using DigitsGame.Resources.Styles;
using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using System;

namespace DigitsGame.Pages.Components;

class GameBoardNumberButton : Component<GameBoardNumberButtonState>
{
    private GameNumber _number;
    private Action _action;
    private bool _isSelected;
    private bool _inError;

    public GameBoardNumberButton Number(GameNumber number)
    {
        _number = number;
        return this;
    }

    public GameBoardNumberButton OnClick(Action action)
    {
        _action = action;
        return this;
    }

    public GameBoardNumberButton IsSelected(bool isSelected)
    {
        _isSelected = isSelected;
        return this;
    }

    public GameBoardNumberButton InError(bool inError)
    {
        _inError = inError;
        return this;
    }

    protected override void OnMounted()
    {
        State.Number = _number;
        base.OnMounted();
    }

    protected override void OnPropsChanged()
    {
        if (State.Number != _number && State.Number?.Value > 0)
        {
            State.PreviousValue = State.Number?.Value;
        }
        State.Number = _number;

        base.OnPropsChanged();
    }

    public override VisualNode Render()
    {
        return new Align
        {
            new PointInteractionHandler
            {
                new Align
                {
                    new Group
                    {
                        new Ellipse()
                            .StrokeSize(_isSelected ? 0 : 3)
                            .StrokeColor(_isSelected ? Theme.GreenColor : Colors.Black)
                            .StrokeDashPattern(_isSelected ? null : new[] { 3.0f, 2.0f })
                            .FillColor(_isSelected ? Theme.GreenColor : Colors.White)
                            ,

                        new Text($"{State.PreviousValue ?? State.Number?.Value}")
                            .FontColor(_isSelected ? Colors.White : Colors.Black)
                            .FontSize(24)
                            .FontWeight(800)
                            .VerticalAlignment(VerticalAlignment.Center)
                            .HorizontalAlignment(HorizontalAlignment.Center)
                            .AutomationId($"Number_Button_Label_{_number?.Id}"),

                        new Timer(0, 300, ()=> SetState(s =>
                        {
                            s.PreviousValue = null;
                            s.OperationCompleted = State.Number?.Value > 0;
                        }))
                        .IsEnabled(State.PreviousValue != null),

                        new AnimationController
                        {
                            new SequenceAnimation
                            {
                                new DoubleAnimation()
                                    .StartValue(1)
                                    .TargetValue(1.1)
                                    .Duration(200)
                                    .OnTick(v => SetState(s=>s.Scale = (float)v)),
                                new DoubleAnimation()
                                    .StartValue(1.1)
                                    .TargetValue(1)
                                    .Duration(200)
                                    .OnTick(v => SetState(s=>s.Scale = (float)v)),
                            }
                        }
                        .IsEnabled(State.OperationCompleted)
                        .OnIsEnabledChanged(enabled => SetState(s => s.OperationCompleted = false))
                    }
                    .ScaleX(State.Scale)
                    .ScaleY(State.Scale)
                    .AnchorX(0.5f)
                    .AnchorY(0.5f)
                }
                .ScaleX(State.IsPressed ? 0.95f : 1)
                .ScaleY(State.IsPressed ? 0.95f : 1)
                .WithAnimation(duration:200)
                .AnchorX(0.5f)
                .AnchorY(0.5f)
            }
            .OnTapDown(()=>
            {
                SetState(s => s.IsPressed = true);
                _action?.Invoke();
            })
            .OnTapUp(()=>SetState(s => s.IsPressed = false))
            .AutomationId($"Number_Button_PIH_{_number?.Id}")
        }
        .IsVisible(State.Number?.Value != 0 || State.PreviousValue != null)
        .ZIndex(State.Number?.Value == 0 ? -1 : 0)
        .Height(86)
        .Width(86)
        .HStart()
        .VStart()
        .Margin((8 + State.Number?.Position.Column * 100).GetValueOrDefault(), (8 + State.Number?.Position.Row * 100).GetValueOrDefault())
        .WithAnimation(duration: 300);
    }
}
