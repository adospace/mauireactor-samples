using DigitsGame.Models;
using MauiReactor;
using MauiReactor.Canvas;
using MauiReactor.Shapes;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DigitsGame.Pages.Components;

class OperationList : Component
{
    private IEnumerable<OperationItem> _operations;

    public OperationList Operations(IEnumerable<OperationItem> operations)
    {
        _operations = operations;
        return this;
    }

    public override VisualNode Render()
    {
        var mainState = GetParameter<MainState>();
        return new Grid("50, 400", "*")
        {
            new Label("Your operations:")
                .HorizontalTextAlignment(TextAlignment.Center)
                .TextColor(Colors.Black),

            new CollectionView()
                .ItemsSource(_operations?.Reverse().ToArray() ?? Array.Empty<OperationItem>(), RenderOperation)
                .GridRow(1)
                .AutomationId("Operations_List")
        }
        .Margin(100)
        .IsVisible(DeviceIdiom.Desktop == DeviceInfo.Current.Idiom || mainState.Value.PageView == PageView.OperationList);
    }

    private VisualNode RenderOperation(OperationItem operation)
    {
        return new VStack(spacing: 5)
        {
            new Label($"{operation.Left.Value} {GetOperationSign(operation.Operation)} {operation.Right.Value} = {operation.CalcValue()}")
                .TextColor(Colors.Black),

            new Line()
                .Stroke(Colors.Black)
                .StrokeThickness(1)
                .X1(0)
                .X2(100)
                .Y1(2)
                .Y2(2)
                .HeightRequest(3)
        }
        .Margin(0, 8);
    }

    static string GetOperationSign(Operation operation)
    {
        return operation switch
        {
            Operation.Add => "+",
            Operation.Subtract => "-",
            Operation.Multiply => "x",
            Operation.Divide => "/",
            _ => throw new NotSupportedException(),
        };
    }
}