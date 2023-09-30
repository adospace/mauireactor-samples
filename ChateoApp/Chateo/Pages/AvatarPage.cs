using Chateo.Resources.Styles;
using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages;

class AvatarPageState { }

class AvatarPageProps
{
    public Action<string>? OnAvatarSelected { get; set; }
}

class AvatarPage : Component<AvatarPageState, AvatarPageProps>
{
    public override VisualNode Render()
    {
        return new ContentPage()
        {
            new VStack
            {
                new Grid("*", "24, *")
                {
                    Theme.Current.Image(Icon.Back)
                        .VEnd()
                        .OnTapped(OnBackClicked),

                    Theme.Current.Label("Your Avatar")
                        .VEnd()
                        .Margin(8,0)
                        .FontSize(18)
                        .GridColumn(1),
                }
                .Padding(0,13),


                new CollectionView()
                    .ItemsLayout(new VerticalGridItemsLayout().HorizontalItemSpacing(16).VerticalItemSpacing(16).Span(4))
                    .ItemsSource(Enumerable.Range(1, 8), RenderAvatarItem)
                    .VFill()
                    .Margin(0,16)
            }
            .Margin(16)
        }
        .BackgroundColor(Theme.Current.Background)
        .Set(MauiControls.Shell.NavBarIsVisibleProperty, false);
    }

    private VisualNode RenderAvatarItem(int avatarIndex)
    {
        return new Image($"avatar{avatarIndex}.png")
            .HeightRequest(48)
            .WidthRequest(48)
            .OnTapped(()=> OnAvatarSelected(avatarIndex))
            ;
    }

    private void OnAvatarSelected(int avatarIndex)
    {
        Props.OnAvatarSelected?.Invoke($"avatar{avatarIndex}");
        OnBackClicked();
    }

    private void OnBackClicked()
    {
        if (Navigation == null)
        {
            return;
        }

        if (Navigation.NavigationStack.Count > 0)
        {
            Navigation.PopAsync();
        }
    }
}
