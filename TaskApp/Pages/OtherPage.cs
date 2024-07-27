using MauiReactor;

namespace TaskApp.Pages
{
    internal class OtherPage : Component
    {
        public override VisualNode Render()
            => ContentPage(
                ScrollView(
                    VStack(
                        Label("Other Page")
                            .FontSize(32)
                            .HCenter()
                    )
                    .VCenter()
                    .Spacing(25)
                    .Padding(30, 0)
                )
            );
    }
}
