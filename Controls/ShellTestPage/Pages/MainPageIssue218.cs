using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

public class MainPageIssue218 : Component
{
    private MauiControls.Shell _shellRef;

    public override VisualNode Render()
        => new Shell(shellRef => _shellRef = shellRef)
        {
            new FlyoutItem("Page1")
            {
                new ShellContent()                
                    .RenderContent(() => new ContentPage("Page1"))
                    .Route("page-1")
            }
            .AutomationId("FlyoutItem_Page1"),

            new FlyoutItem("Page2")
            {
                new ShellContent()                
                    .RenderContent(() => new ContentPage("Page2")
                    {
                        new Button("Goto to Page1")
                            .HCenter()
                            .VCenter()
                            .AutomationId("GotoPage1Button")
                        .OnClicked(async ()=> await _shellRef.GoToAsync("//page-1"))
                    })
                    .Route("page-2")
            }
            .AutomationId("FlyoutItem_Page2")
        }
        .AutomationId("MainShell")
        .ItemTemplate(RenderItemTemplate);

    static VisualNode RenderItemTemplate(MauiControls.BaseShellItem item)
        => new Grid("68", "*")
        {
            new Label(item.Title)
                .VCenter()
                .Margin(10,0)
        };
}