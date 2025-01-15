using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiReactor;
using MauiReactor.Animations;
using Microsoft.Maui.Controls;
using TrackizerApp.Models;

namespace TrackizerApp.Pages.Components;

class CardPlate
{
    public int Index { get; init; }

    public int Position { get; set; }

    public CreditCard CreditCard { get; set; } = default!;
}

class CardSelectorState
{
    public List<CardPlate> Cards { get; set; } = [];
}

partial class CardSelector : Component<CardSelectorState>
{
    [Prop]
    IEnumerable<CreditCard>? _cards;

    protected override void OnMountedOrPropsChanged()
    {
        State.Cards = _cards?.Select((card, index) => new CardPlate { Index = index, Position = index + 1, CreditCard = card } ).ToList() ?? [];
        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        return Grid(
                State.Cards
                    .Select(card => new CardPage()
                        .Card(card)
                        .Position(card.Position)
                        .OnMovedBack(cardIndex =>
                        {
                            SetState(s =>
                            {
                                foreach (var card in s.Cards)
                                {
                                    card.Position++;
                                }

                                s.Cards[cardIndex].Position = 1;

                            });
                        }))
                    .ToArray()
            )
            .VStart();
    }
}

enum CardMovingBack
{
    None,

    FromLeft,

    FromRight
}

class CardState
{
    public double Rotation { get; set; } = Random.Shared.NextDouble() * 5 - 2.5;

    public CardMovingBack MovingBack { get; set; }

    public double TranslationX {  get; set; }

    public double InitialTranslationX { get; set; }

    public int ZIndex { get; set; }
}

partial class CardPage : Component<CardState>
{
    [Prop]
    private CardPlate _card = default!;

    [Prop]
    private int _position;

    [Prop]
    private Action<int>? _onMovedBack;

    protected override void OnMountedOrPropsChanged()
    {
        State.ZIndex = _position;
        base.OnMountedOrPropsChanged();
    }

    public override VisualNode Render()
    {
        return Grid(
            Grid(
                Image("card_plate.png"),
                ApplicationTheme.BodyLarge(_card.CreditCard.Type)
                    .HCenter()
                    .Margin(0, 82)
                    .TextColor(ApplicationTheme.White),

                ApplicationTheme.H1(_card.CreditCard.Holder)
                    .HCenter()
                    .VEnd()
                    .Margin(0, 136)
                    .TextColor(ApplicationTheme.Grey20),

                ApplicationTheme.BodyMedium(_card.CreditCard.ExpiringDate)
                    .HCenter()
                    .VEnd()
                    .Margin(0, 76)
                    .TextColor(ApplicationTheme.White),

                new AnimationController
                {
                    new SequenceAnimation
                    {
                        new DoubleAnimation()
                            .StartValue(State.InitialTranslationX)
                            .TargetValue(State.MovingBack == CardMovingBack.FromLeft ? -240 : 240)
                            .Duration(200 - 200 * (Math.Abs(State.InitialTranslationX/240)))
                            .OnTick(v =>
                            {
                                SetState(s =>
                                {
                                    s.TranslationX = v;
                                    s.ZIndex = _position;
                                }, false);
                            }),
                        new DoubleAnimation()
                            .StartValue(State.MovingBack == CardMovingBack.FromLeft ? -240 : 240)
                            .TargetValue(0)
                            .Duration(200)
                            .OnTick(v =>
                            {
                                SetState(s =>
                                {
                                    s.TranslationX = v;
                                    s.ZIndex = 0;
                                }, false);
                            })
                    }
                }
                .OnIsEnabledChanged(s =>
                {
                    if (State.MovingBack != CardMovingBack.None)
                    {
                        State.InitialTranslationX = 0;
                        State.MovingBack = CardMovingBack.None;
                        _onMovedBack?.Invoke(_card.Index);
                    }
                })
                .IsEnabled(State.MovingBack != CardMovingBack.None)
            )
            .TranslationX(() => State.TranslationX)
            .Rotation(State.Rotation)
            .WithAnimation()
            .WidthRequest(232)
            .VStart()
            .HCenter()
            .Margin(0, 80)
        )
        .ZIndex(() => State.ZIndex)
        .OnTapped(() => SetState(s => s.MovingBack = CardMovingBack.FromLeft))
        .OnPanUpdated(OnCardPanning);
    }

    void OnCardPanning(PanUpdatedEventArgs args)
    {
        if (args.StatusType == GestureStatus.Canceled)
        {
            SetState(s => s.TranslationX = 0);
        }
        else if (args.StatusType == GestureStatus.Running)
        {
            SetState(s => s.TranslationX = args.TotalX, false);
        }
        else if (args.StatusType == GestureStatus.Completed)
        {
            if (State.TranslationX > 20)
            {
                SetState(s =>
                {
                    s.InitialTranslationX = State.TranslationX;
                    s.MovingBack = CardMovingBack.FromRight;
                });
            }
            else if (State.TranslationX < -20)
            {
                SetState(s =>
                {
                    s.InitialTranslationX = State.TranslationX;
                    s.MovingBack = CardMovingBack.FromLeft;
                });
            }
            else
            {
                SetState(s => s.TranslationX = 0, false);
            }
        }

    }
}