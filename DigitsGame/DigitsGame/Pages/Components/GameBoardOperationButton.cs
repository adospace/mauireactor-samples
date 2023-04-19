using DigitsGame.Models;
using MauiReactor;
using MauiReactor.Canvas;
using System;

namespace DigitsGame.Pages.Components;

class GameBoardOperationButton : Component
{
    Operation _operation;
    Action _action;
    private bool _isSelected;

    public GameBoardOperationButton Operation(Operation operation)
    {
        _operation = operation;
        return this;
    }

    public GameBoardOperationButton IsSelected(bool isSelected)
    {
        _isSelected = isSelected;
        return this;
    }

    public GameBoardOperationButton OnClick(Action action)
    {
        _action = action;
        return this;
    }

    public override VisualNode Render()
    {
        return new PointInteractionHandler
        {
            new Align
            {
                new Picture($"DigitsGame.Resources.Images.{_operation.ToString().ToLowerInvariant()}_icon{(_isSelected ? "_green" : string.Empty)}.png")
            }
            .Margin(5,0),
        }
        .OnTap(_action)
        .AutomationId($"Operation_Button_PIH_{_operation}");
    }
}
