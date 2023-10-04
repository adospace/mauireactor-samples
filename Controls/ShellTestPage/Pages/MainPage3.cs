using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage3 : Component
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
        }
        .ItemTemplate(RenderItemTemplate);

    static VisualNode RenderItemTemplate(MauiControls.BaseShellItem item)
         => new Grid("68", "Auto, *")
         {
             new Image()
                .Source(item.FlyoutIcon)
                .Margin(4),

             new Label(item.Title)
                .GridColumn(1)
                .VCenter()
                .TextDecorations(TextDecorations.Underline)
                .FontAttributes(MauiControls.FontAttributes.Bold)
                .Margin(10,0)
         };
}
