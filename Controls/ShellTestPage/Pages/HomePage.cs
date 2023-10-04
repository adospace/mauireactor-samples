using MauiReactor;

namespace ShellTestPage.Pages;

class HomePage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Home")
        {
            new Label("Home")
                .VCenter()
                .HCenter()
        };
    }
}
