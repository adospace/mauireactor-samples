using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage : Component
{
    public override VisualNode Render()
        => new Shell
        {
            new ShellContent("Home")
                .Icon("home.png")
                .RenderContent(()=> new HomePage()),

            new ShellContent("Comments")
                .Icon("comments.png")
                .RenderContent(()=> new CommentsPage()),
        };
}
