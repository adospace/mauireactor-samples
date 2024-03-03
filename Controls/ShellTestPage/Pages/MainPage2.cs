using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

public class MainPage2 : Component
{
    public override VisualNode Render()
        => Shell(
            FlyoutItem(
                Tab(
                    ShellContent("Home")
                        .Icon("home.png")
                        .RenderContent(()=> new HomePage())
                        .AutomationId("home_item"),

                    ShellContent("Comments")
                        .Icon("comments.png")
                        .RenderContent(()=> new CommentsPage())
                        .AutomationId("comments_item")
                )
                .Title("Notifications")
                .Icon("bell.png")
                .AutomationId("tab"),


                ShellContent("Database")
                    .Icon("database.png")
                    .RenderContent(()=> new DatabasePage())
                    .AutomationId("database_item"),

                ShellContent("Notifications")
                    .Icon("bell.png")
                    .RenderContent(()=> new NotificationsPage())
                    .AutomationId("notifications_item")
            )
            .AutomationId("flyout_item")
            .FlyoutDisplayOptions(MauiControls.FlyoutDisplayOptions.AsMultipleItems),


            ShellContent("Settings")
                .Icon("gear.png")
                .RenderContent(()=> new SettingsPage())
                .AutomationId("settings_item")
        )
        .AutomationId("MainShell");
}
