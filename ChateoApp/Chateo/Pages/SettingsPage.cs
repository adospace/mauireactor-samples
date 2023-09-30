using Chateo.Pages.Components;
using Chateo.Resources.Styles;
using MauiReactor;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Pages;

class SettingsPage : Component
{
    public override VisualNode Render()
    {
        return new Grid
        {
            new Grid("56, 66, *, 83", "*")
            {
                RenderTitleBar(),

                RenderUserItem(),

                new VStack(spacing: 5)
                {
                    RenderSettingItem(Icon.Account, "Account"),

                    RenderSettingItem(Icon.Chats, "Chats"),

                    new Rectangle()
                        .HeightRequest(2)
                        .Margin(-24,0)
                        .Fill(Theme.Current.MediumBackground),

                    RenderSettingItem(Icon.Appearance, "Appearance", Theme.ToggleCurrentAppTheme),

                    RenderSettingItem(Icon.Notification, "Notification"),

                    RenderSettingItem(Icon.Privacy, "Privacy"),

                    RenderSettingItem(Icon.Folder, "Data Usage"),

                    new Rectangle()
                        .HeightRequest(2)
                        .Margin(-24,0)
                        .Fill(Theme.Current.MediumBackground),

                    RenderSettingItem(Icon.Help, "Help"),

                    RenderSettingItem(Icon.Email, "Invite Your Friends"),
                }
                .GridRow(2)
                .Margin(0,16),
            }
        }
        .Margin(24, 16);
        ;
    }

    private VisualNode RenderUserItem()
    {
        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        return new Grid("66", "50, *, 42")
        {
            Theme.Current.Avatar(currentUser.Avatar)
                .HeightRequest(50)
                .WidthRequest(50)
                .Margin(0,8,0,8),

            new VStack
            {
                Theme.Current.Label($"{currentUser.FirstName} {currentUser.LastName}")
                    .FontSize(14),

                Theme.Current.Label("+62 1309 - 1710 - 1920")
                    .FontSize(12)
                    .TextColor(Theme.Current.MediumForeground)
                    .VEnd()
            }
            .Margin(20, 8)
            .GridColumn(1),

            Theme.Current.Image(Icon.Forward)
                .HeightRequest(24)
                .WidthRequest(24)
                .HCenter()
                .VCenter()
                .Margin(16,0,0,0)
                .GridColumn(2)
        }
        .GridRow(1);
    }

    private VisualNode RenderSettingItem(Icon icon, string text, Action? tapAction = null)
    {
        var mainState = GetParameter<MainState>();
        var currentUser = mainState.Value.CurrentUser ?? throw new InvalidOperationException();

        return new Grid("40", "50, *, 42")
        {
            Theme.Current.Image(icon)
                .HCenter()
                .VCenter()
                .Margin(0,8,0,8),

            Theme.Current.Label(text)
                .FontSize(14)
                .Margin(20, 8)
                .GridColumn(1),

            Theme.Current.Image(Icon.Forward)
                .HeightRequest(24)
                .WidthRequest(24)
                .HCenter()
                .VCenter()
                .Margin(16,0,0,0)
                .GridColumn(2)
        }
        .OnTapped(tapAction)
        .GridRow(1);
    }

    VisualNode RenderTitleBar()
    {
        return new Grid("24", "*, 32, 24")
        {
            Theme.Current.Label("Settings")
                .FontSize(18),

        }
        .VEnd()
        .Margin(0, 13);
    }
}
