using MauiReactor;
using TaskApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp;

internal class AppShell : Component
{
    public override VisualNode Render()
        => Shell(
            new MainPage()
        );
}
