using MauiReactor;
using TaskApp.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Pages;

partial class SettingsPage : Component
{
    public override VisualNode Render()
    {
        return ContentPage(
            Grid("64,Auto,*", "*",

                RenderHeader(),

                RenderPreferences(),

                RenderMore()
            ));
    }

    Grid RenderHeader()
        => Grid("*", "56,*,Auto",
            ImageButton("back.png")
                .BackgroundColor(Colors.Transparent)
                .HeightRequest(48)
                .WidthRequest(48)
                .OnClicked(async () =>
                {
                    if (Navigation != null && Navigation.NavigationStack.Count > 0)
                    {
                        await Navigation.PopAsync();
                    }
                })
                .Margin(8, 0, 4, 0)
            ,

            Label("Settings")
                .GridColumn(1)
                .VCenter()
                .ThemeKey(ApplicationTheme.Selector.Header),


            Border()
                .HeightRequest(1)
                .VEnd()
                .GridColumnSpan(3)
                .BackgroundColor(ApplicationTheme.LightGray)
        );

    VStack RenderPreferences()
        => VStack(spacing: 8,
            Label("Preferences")
                .ThemeKey(ApplicationTheme.Selector.Body),
            RenderOptionsMenuItem("theme.svg", "Theme", () =>
            {
                MauiControls.Application.Current!.UserAppTheme = Theme.IsDarkTheme ? Microsoft.Maui.ApplicationModel.AppTheme.Light : Microsoft.Maui.ApplicationModel.AppTheme.Dark;
                Invalidate();
            }),
            RenderOptionsMenuItem("color.svg", "Color scheme", null)
            )
        .Margin(20, 10, 20, 0)
        .GridRow(1);

    VStack RenderMore()
        => VStack(spacing: 8,
                Label("More")
                    .ThemeKey(ApplicationTheme.Selector.Body),
                RenderOptionsMenuItem("feedback.svg", "Feedback", null),
                RenderOptionsMenuItem("rate.svg", "Rate", null),
                RenderOptionsMenuItem("support.svg", "Support", null),
                RenderOptionsMenuItem("faq.svg", "FAQ", null),
                RenderOptionsMenuItem("privacy.svg", "Privacy policy", null),
                RenderOptionsMenuItem("terms.svg", "Terms of use", null),
                RenderOptionsMenuItem("about.svg", "About us", null),
                RenderOptionsMenuItem(Theme.IsDarkTheme ? "logout_dark.svg" : "logout.svg", "Logout", null, ApplicationTheme.Red)
                )
            .Margin(20, 16, 20, 0)
            .GridRow(2);

    static Grid RenderOptionsMenuItem(string icon, string text, Action? openAction, Color? textColor = null)
    {
        return Grid("*", "24,*,Auto",
            Button()
                .GridColumnSpan(3)
                .BackgroundColor(Colors.Transparent)
                .BorderWidth(0)
                .OnClicked(openAction),

            Border()
                .HeightRequest(1)
                .VEnd()
                .GridColumnSpan(3)
                .BackgroundColor(ApplicationTheme.LightGray),

            Image(icon)
                .HeightRequest(24)
                .WidthRequest(24)
                .Margin(0, 12),

            Label(text)
                .GridColumn(1)
                .VerticalTextAlignment(TextAlignment.Center)
                .Margin(12, 12)
                .When(textColor != null, _ => _.TextColor(textColor!))
        );
    }


}
