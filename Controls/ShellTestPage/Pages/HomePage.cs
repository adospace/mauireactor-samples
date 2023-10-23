using MauiReactor;

namespace ShellTestPage.Pages;

class HomePage : Component
{
    protected override void OnMounted()
    {
        System.Diagnostics.Debug.WriteLine("HomePage.OnMounted()");

        base.OnMounted();
    }

    protected override void OnWillUnmount()
    {
        System.Diagnostics.Debug.WriteLine("HomePage.OnWillUnmount()");

        base.OnWillUnmount();
    }

    public override VisualNode Render()
    {
        return new ContentPage("Home")
        {
            new Label("Home")
                .VCenter()
                .HCenter()
        }
        .OnAppearing(()=> System.Diagnostics.Debug.WriteLine("HomePage.OnAppearing()"))
        .OnDisappearing(() => System.Diagnostics.Debug.WriteLine("HomePage.OnDisappearing()"));
    }
}
