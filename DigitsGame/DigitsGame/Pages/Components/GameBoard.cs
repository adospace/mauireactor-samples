using DigitsGame.Models;
using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using System;
using System.Linq;

namespace DigitsGame.Pages.Components;
class GameBoardState
{
    public Operation? CurrentOperation { get; set; }

    public GameNumber CurrentNumber { get; set; }

    public OperationItem OperationInError { get; set; }
}

partial class GameBoard : Component<GameBoardState>
{
    [Prop]
    private GameNumber[] _board;
    
    [Prop]
    private int _target;

    [Prop]
    private Action<OperationItem> _onNewOperation;

    [Prop]
    private Action _onUndoOperation;


    public override VisualNode Render()
    {
        var mainState = GetParameter<MainState>();
        return new CanvasView
        {
            new Column("100, *, 100")
            {
                new Text($"{_target}")
                    .VerticalAlignment(VerticalAlignment.Center)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .FontSize(46)
                    .FontWeight(800),

                new Align
                {
                    new Group
                    {
                        _board?.Select(RenderBoardNumberButton).ToArray()
                    }
                }
                .VCenter()
                .HCenter()
                .Height(200)
                .Width(300),

                new Row
                {
                    RenderUndoButton(),

                    Enum.GetValues<Operation>().Select(RenderOperationButton).ToArray()
                },

                new Timer(0, 1000, ()=> SetState(s =>
                {
                    s.OperationInError = null;
                }))
                .IsEnabled(State.OperationInError != null)
            }
        }
        .HeightRequest(400)
        .WidthRequest(300)
        .BackgroundColor(Colors.Transparent)
        .IsVisible(mainState.Value.PageView == PageView.GameBoard)
        ;
    }

    VisualNode RenderOperationButton(Operation operation)
    {
        return new AnimatingButton
        {
            new GameBoardOperationButton()
                .Operation(operation)
                .IsSelected(State.CurrentOperation == operation || State.OperationInError?.Operation == operation)
                .OnClick(() => OnOperationSelected(operation))
        }
        .InError(State.OperationInError?.Operation == operation);
    }

    PointInteractionHandler RenderUndoButton()
    {
        return new PointInteractionHandler
        {
            new Align
            {
                new Picture($"DigitsGame.Resources.Images.undo_icon_green.png")
            }
            .Margin(5,0)
        }
        .OnTap(_onUndoOperation);
    }

    void OnOperationSelected(Operation operation)
    {
        if (State.CurrentNumber != null)
        {
            SetState(s => s.CurrentOperation = operation);
        }
    }

    AnimatingButton RenderBoardNumberButton(GameNumber number)
    {
        return new AnimatingButton
        {
            new GameBoardNumberButton()
                .Number(number)
                .IsSelected(State.CurrentNumber == number || State.OperationInError?.Left == number || State.OperationInError?.Right == number)
                .OnClick(() => OnNumberClicked(number))
        }
        .InError(State.OperationInError?.Left == number || State.OperationInError?.Right == number);
    }

    void OnNumberClicked(GameNumber number)
    {
        if (State.CurrentOperation == null)
        {
            SetState(s => s.CurrentNumber = s.CurrentNumber == number ? null : number);
        }
        else if (State.CurrentNumber != null)
        {
            var newOperation = new OperationItem(State.CurrentNumber, number, State.CurrentOperation.Value);

            if (!newOperation.IsValid())
            {
                SetState(s =>
                {
                    s.CurrentNumber = null;
                    s.CurrentOperation = null;
                    s.OperationInError = newOperation;
                });
            }
            else
            {
                SetState(s =>
                {
                    s.CurrentNumber = null;
                    s.CurrentOperation = null;
                });

                _onNewOperation?.Invoke(newOperation);
            }
        }
    }
}
