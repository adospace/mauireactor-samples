using DigitsGame.Models;
using MauiReactor;
using MauiReactor.Canvas;
using System;

namespace DigitsGame.Pages.Components;

partial class GameBoardOperationButton : Component
{
    [Prop]
    Operation _operation;
    
    [Prop]
    Action _onClick;

    [Prop]
    bool _isSelected;

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
        .OnTap(_onClick)
        .AutomationId($"Operation_Button_PIH_{_operation}");
    }
}
