using DigitsGame.Models;
using DigitsGame.Pages.Components;
using DigitsGame.Resources.Styles;
using MauiReactor;
using MauiReactor.Animations;
using MauiReactor.Canvas;
using MauiReactor.Parameters;
using MauiReactor.Shapes;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitsGame.Pages;

enum PageView
{
    GameBoard,

    OperationList
}

class MainState
{
    public PageView PageView { get; set; }
}

class MainPageState
{
    public GameModel CurrentGame { get; set; } = GameModel.FirstGame;

    public Stack<GameNumber[]> BoardStates { get; set; } = new();

    public Stack<OperationItem> Operations { get; set; } = new();
}

class MainPage : Component<MainPageState>
{
    private IParameter<MainState> _mainStateParameter;

    public MainPage()
    {
        _mainStateParameter = CreateParameter<MainState>();
    }

    public override VisualNode Render()
    {
        if (DeviceIdiom.Desktop == DeviceInfo.Current.Idiom)
        {
            return RenderOnDesktop();
        }
        else
        {
            return RenderOnMobile();
        }
    }

    VisualNode RenderOnMobile()
    {
        var currentView = _mainStateParameter?.Value.PageView ?? PageView.GameBoard;
        return new ContentPage
        {
            new Grid("100,400", "*,*")
            {
                new Button("Game Board")
                    .TextColor(currentView == PageView.GameBoard ? Colors.White : Colors.Black)
                    .BackgroundColor(currentView == PageView.GameBoard ? Theme.GreenColor : Colors.LightGray)
                    .OnClicked(()=>_mainStateParameter?.Set(_ => _.PageView = PageView.GameBoard))
                    .Margin(10),

                new Button("Operations")
                    .TextColor(currentView == PageView.OperationList ? Colors.White : Colors.Black)
                    .BackgroundColor(currentView == PageView.OperationList ? Theme.GreenColor : Colors.LightGray)
                    .OnClicked(()=>_mainStateParameter?.Set(_ => _.PageView = PageView.OperationList))
                    .GridColumn(1)
                    .Margin(10),

                new GameBoard()
                    .Target(State.CurrentGame.TargetValue)
                    .Board(State.BoardStates.Count > 0 ? State.BoardStates.Peek() : State.CurrentGame.Values)
                    .OnNewOperation(OnNewOperationCreated)
                    .OnUndoOperation(OnUndoLastOperation)
                    .GridRow(1)
                    .GridColumnSpan(2),

                new OperationList()
                    .Operations(State.Operations)
                    .GridRow(1)
                    .GridColumnSpan(2)
            }
        };

    }


    VisualNode RenderOnDesktop()
    {
        return new ContentPage
        {
            new Grid("*", "*,*")
            {
                new GameBoard()
                    .Target(State.CurrentGame.TargetValue)
                    .Board(State.BoardStates.Count > 0 ? State.BoardStates.Peek() : State.CurrentGame.Values)
                    .OnNewOperation(OnNewOperationCreated)
                    .OnUndoOperation(OnUndoLastOperation),

                new OperationList()
                    .Operations(State.Operations)
                    .GridColumn(1)
            }
        };

    }

    void OnNewOperationCreated(OperationItem newOperation)
    {
        var currentState = State.BoardStates.Count > 0 ? State.BoardStates.Peek() : State.CurrentGame.Values;

        var newState = currentState
            .Select(n =>
            {
                if (n.Id == newOperation.Left.Id || n.Value == 0)
                    return new GameNumber(newOperation.Left.Id, newOperation.Right.Position, 0);
                else if (n.Id == newOperation.Right.Id)
                    return new GameNumber(newOperation.Right.Id, newOperation.Right.Position, newOperation.CalcValue());

                return n;
            })
            .ToArray();

        State.BoardStates.Push(newState);
        State.Operations.Push(newOperation);

        Invalidate();
    }

    void OnUndoLastOperation()
    {
        if (State.BoardStates.Count == 0)
        {
            return;
        }

        State.BoardStates.Pop();
        State.Operations.Pop();

        Invalidate();
    }
}

class GameBoardState
{
    public Operation? CurrentOperation { get; set; }

    public GameNumber CurrentNumber { get; set; }

    public OperationItem OperationInError { get; set; }
}

class GameBoardNumberButtonState
{
    public GameNumber Number { get; set; }

    public int? PreviousValue { get; set; }

    public bool OperationCompleted { get; set; }

    public float Scale { get; set; } = 1.0f;

    public bool IsPressed { get; set; }
}

class AnimatingButtonState
{
    public float TranslateX { get; set; }

    public bool InError { get; set; }
}
