using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShellTestPage.Pages;

class DatabasePage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Database")
        {
            new Label("Database")
                .VCenter()
                .HCenter()
        };
    }
}
