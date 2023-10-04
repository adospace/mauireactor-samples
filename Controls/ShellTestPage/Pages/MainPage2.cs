using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage2 : Component
{
    public override VisualNode Render()
        => new Shell
        {
            new FlyoutItem
            {
                new Tab
                {
                    new ShellContent("Home")
                        .Icon("home.png")
                        .RenderContent(()=> new HomePage()),

                    new ShellContent("Comments")
                        .Icon("comments.png")
                        .RenderContent(()=> new CommentsPage()),
                }
                .Title("Notifications")
                .Icon("bell.png"),


                new ShellContent("Home")
                    .Icon("database.png")
                    .RenderContent(()=> new DatabasePage()),

                new ShellContent("Comments")
                    .Icon("bell.png")
                    .RenderContent(()=> new NotificationsPage()),
            }
            .FlyoutDisplayOptions(MauiControls.FlyoutDisplayOptions.AsMultipleItems),


            new ShellContent("Settings")
                .Icon("gear.png")
                .RenderContent(()=> new SettingsPage()),
        };
}
