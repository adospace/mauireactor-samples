using MauiReactor;
using ShellTestPage.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage8 : Component
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
                .OniOS(_=>_.Style(ResourceManager.FindStyle("FixIOSNotificationsTabStyle")))

                .Title("Notifications"),

                Tab(
                    ShellContent()
                        .RenderContent(() => new DatabasePage())
                )
                .OnAndroid(_ => _.Icon(new MauiControls.FontImageSource
                {
                    FontFamily = "FontSolid",
                    Glyph = AwesomeIconFont.Database,
                    Size = 20,
                }))
                .OniOS(_ => _.Style(ResourceManager.FindStyle("FixIOSDatabaseTabStyle")))

                .Title("Database")
            ));
}
