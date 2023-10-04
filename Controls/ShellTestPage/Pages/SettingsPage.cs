using MauiReactor;

namespace ShellTestPage.Pages;

class SettingsPage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Settings")
        {
            new Label("Settings")
                .VCenter()
                .HCenter()
        };
    }
}
