using MauiReactor;

namespace ShellTestPage.Pages;

class CommentsPage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Comments")
        {
            new Label("Comments")
                .VCenter()
                .HCenter()
        };
    }
}
