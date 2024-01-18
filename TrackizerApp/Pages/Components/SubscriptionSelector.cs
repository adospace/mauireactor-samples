using MauiReactor;
using MauiReactor.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackizerApp.Models;

namespace TrackizerApp.Pages.Components;

class SubscriptionSelectorState
{
    public double ViewportWidth {  get; set; }

    public SubscriptionType SelectedType {  get; set; }

    public double PanningX { get; set; }

    public bool AnimatingPan {  get; set; }
}

partial class SubscriptionSelector : Component<SubscriptionSelectorState>
{
    const double _cellWidth = 200;
    static SubscriptionType[] _subscriptionTypes = Enum.GetValues<SubscriptionType>();

    [Prop]
    Action<SubscriptionType> _onSelectedType;

    double GetTranslationX(SubscriptionType subscriptionType)
    {
        var translationX = _cellWidth * (int)subscriptionType + State.PanningX - (int)State.SelectedType * _cellWidth;
        if (translationX > State.ViewportWidth)
        {
            translationX -= _subscriptionTypes.Length * _cellWidth;
        }
        else if (translationX < -State.ViewportWidth)
        {
            translationX += _subscriptionTypes.Length * _cellWidth;
        }
        return translationX;
    }

    static double GetSize(double translationX)
    {
        var distance = Math.Abs(translationX);

        if (distance > _cellWidth)
        {
            return 95;
        }

        return 160 - 65 * distance / _cellWidth;
    }

    public override VisualNode Render()
        => Grid(
            [..
            _subscriptionTypes.Select(type=>
            {
                var translationX = GetTranslationX(type);

                return Grid(
                    Image($"{type.ToString().ToLower()}.png")
                        .HeightRequest(GetSize(translationX))
                        .Center()
                )
                .Margin(0, 0, 0, 54)
                .WidthRequest(_cellWidth)
                .TranslationX(translationX);
            }),

            Theme.H2()
                .Text(State.SelectedType.GetDisplayName() ?? string.Empty)
                .TextColor(Theme.White)
                .VEnd()
                .HCenter()
                .Margin(40),


            new AnimationController
            {
                new SequenceAnimation
                {
                    new DoubleAnimation()
                        .StartValue(State.PanningX)
                        .TargetValue(0)
                        .Duration(100)
                        .OnTick(v => SetState(s => s.PanningX = v))
                }
            }
            .OnIsEnabledChanged(enabled => 
            {
                if (!enabled)
                {
                    SetState(s => s.AnimatingPan = false);
                }
            })
            .IsEnabled(State.AnimatingPan)
            ]
        )
        .OnSizeChanged(size => SetState(s => s.ViewportWidth = size.Width))
        .OnPanUpdated(PanUpdated);

    void PanUpdated(MauiControls.PanUpdatedEventArgs args)
    {
        if (args.StatusType == GestureStatus.Canceled)
        {
            SetState(s => s.PanningX = 0);
        }
        else if (args.StatusType == GestureStatus.Running)
        {
            SetState(s => s.PanningX = args.TotalX);
        }
        else if (args.StatusType == GestureStatus.Completed)
        {
            if (State.PanningX < -10)
            {
                SetState(s =>
                {
                    s.AnimatingPan = true;
                    s.PanningX += _cellWidth;
                    s.SelectedType = (int)s.SelectedType == _subscriptionTypes.Length - 1 ? 0 : (SubscriptionType)((int)s.SelectedType + 1);
                });

                _onSelectedType?.Invoke(State.SelectedType);
            }
            else if (State.PanningX > 10)
            {
                SetState(s =>
                {
                    s.AnimatingPan = true;
                    s.PanningX -= _cellWidth;
                    s.SelectedType = s.SelectedType == 0 ? (SubscriptionType)(_subscriptionTypes.Length - 1) : s.SelectedType - 1;
                });

                _onSelectedType?.Invoke(State.SelectedType);
            }
            else
            {
                SetState(s => s.PanningX = 0);
            }
        }
    }
}
