using MauiReactor;
using ShellTestPage.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

public class MainPage8 : Component
{
    public override VisualNode Render()
        => Shell(
            TabBar(
                Tab(
                    ShellContent("Notifications")
                        .RenderContent(() => new NotificationsPage())
                )
                .OnAndroid(_ => _.Icon(new MauiControls.FontImageSource 
                    {
                        FontFamily = "FontSolid",
                        Glyph = AwesomeIconFont.Bell,
                        Size = 20,
                    }))
                .OniOS(_=>_.Style("FixIOSNotificationsTabStyle"))
                .AutomationId("NotificationsItem")
                .Title("Notifications"),

                Tab(
                    ShellContent()
                        .RenderContent(() => new DatabasePage())
                        .Route("Database")
                )
                .OnAndroid(_ => _.Icon(new MauiControls.FontImageSource
                {
                    FontFamily = "FontSolid",
                    Glyph = AwesomeIconFont.Database,
                    Size = 20,
                }))
                .OniOS(_ => _.Style("FixIOSDatabaseTabStyle"))
                .AutomationId("DatabaseItem")
                .Title("Database")
            ))
            .AutomationId("MainShell");
}
