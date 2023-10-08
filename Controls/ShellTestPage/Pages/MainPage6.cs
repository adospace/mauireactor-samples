using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage6 : Component
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
        }
        .FlyoutBackground(new LinearGradient(45.0, new Color(255, 175, 189), new Color(100, 216, 243)))
        .FlyoutHeader(RenderHeader())
        .FlyoutFooter(RenderFooter())
        ;

    private VisualNode RenderHeader()
    {
        return new VStack(spacing: 5)
        {
            new Label("MauiReactor")
                .TextColor(Colors.White)
                .FontSize(24)
                .HorizontalTextAlignment(TextAlignment.Center)
                .FontAttributes(MauiControls.FontAttributes.Bold)
        };
    }

    private VisualNode RenderFooter()
    {
        return new Image("dotnet_bot.png");
    }
}
