using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage5 : Component
{
    public override VisualNode Render()
        => new Shell
        {
            new ShellContent("Home")
                .Route(nameof(HomePage))
                .Icon("home.png")
                .RenderContent(()=> new HomePage()),

            new ShellContent("Comments")
                .Route(nameof(CommentsPage))
                .Icon("comments.png")
                .RenderContent(()=> new CommentsPage()),

            new MenuItem("Click me!")
                .IconImageSource("gear.png")
                .OnClicked(async ()=> await ContainerPage.DisplayAlert("MauiReactor", "Clicked!", "OK"))
        }
        .MenuItemTemplate(menuItem =>
            new Grid("65", "Auto, *")
            {
                new Image()
                    .Source(menuItem.IconImageSource)
                    .VCenter(),

                new Label(menuItem.Text)
                    .TextColor(Colors.Red)
                    .VCenter()
                    .Margin(10,0)
                    .FontAttributes(MauiControls.FontAttributes.Bold)
                    .GridColumn(1)
            }
            .Padding(10,0)
            )
        ;
}
