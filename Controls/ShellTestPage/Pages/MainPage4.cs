using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage4 : Component
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
        .FlyoutContent(RenderFlyoutContent());


    VisualNode RenderFlyoutContent()
    {
        return new ScrollView
        {
            new VStack(spacing: 5)
            {
                new Button("Home")
                    .OnClicked(async ()=> await MauiControls.Shell.Current.GoToAsync($"//{nameof(HomePage)}")),

                new Button("Comments")
                    .OnClicked(async ()=> await MauiControls.Shell.Current.GoToAsync($"//{nameof(CommentsPage)}")),
            }
        };
    }
}
