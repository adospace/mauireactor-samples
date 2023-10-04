using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class NotificationsPage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Notifications")
        {
            new Label("Notifications")
                .VCenter()
                .HCenter()
        };
    }
}

