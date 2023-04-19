using DigitsGame.Models;
using DigitsGame.Pages;
using DigitsGame.Pages.Components;
using MauiReactor;
using Shouldly;

namespace DigitsGame.Tests
{
    [TestClass]
    public class SampleComponentTests
    {
        [TestMethod]
        public void GameBoardNumberButton_should_show_the_correct_value()
        {
            var button = TemplateHost.Create(new GameBoardNumberButton().Number(
                new Models.GameNumber(1, new Models.GameNumberPosition(0, 0), 32)));

            var numberLabel = button.Find<MauiReactor.Canvas.Internals.Text>($"Number_Button_Label_1");

            numberLabel.Value.ShouldBe("32");
        }

        [TestMethod]
        public void An_operation_is_correctly_handled()
        {
            var gameBoard = TemplateHost.Create(new MainPage());

            var firstNumberButton = gameBoard.Find<MauiReactor.Canvas.Internals.PointInteractionHandler>($"Number_Button_PIH_1");
            var operationButton = gameBoard.Find<MauiReactor.Canvas.Internals.PointInteractionHandler>($"Operation_Button_PIH_Add");
            var secondNumberButton = gameBoard.Find<MauiReactor.Canvas.Internals.PointInteractionHandler>($"Number_Button_PIH_1");
            var operationsList = gameBoard.Find<Microsoft.Maui.Controls.CollectionView>($"Operations_List");

            operationsList.ItemsSource.OfType<OperationItem>().Count().ShouldBe(0);

            firstNumberButton.SendTapDown();
            operationButton.SendTap();
            secondNumberButton.SendTapDown();

            operationsList.ItemsSource.OfType<OperationItem>().Count().ShouldBe(1);
        }
    }
}