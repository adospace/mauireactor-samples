using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class MainPage7 : Component
{
    private MauiControls.ShellContent _notificationsPage;

    public override VisualNode Render()
        => new Shell
        {
            new TabBar
            {
                new ShellContent("Home")
                    .Icon("home.png")
                    .RenderContent(()=> new HomePage()),

                new Tab("Engage")
                {
                    new ShellContent(pageRef => _notificationsPage = pageRef)
                        .Title("Notifications")
                        .RenderContent(()=> new NotificationsPage())
                        .Set(MauiControls.Shell.TabBarIsVisibleProperty, true)
                    ,

                    new ShellContent("Comments")
                    { 
                        new ContentPage
                        {
                            new Button("Go to notifications")
                                .VCenter()
                                .HCenter()
                                .OnClicked(()=> MauiControls.Shell.Current.CurrentItem = _notificationsPage)
                        }
                    }
                }
                .Icon("comments.png")
            }
            .Set(MauiControls.Shell.TabBarBackgroundColorProperty, Colors.Aquamarine)
        };
}
