

using Chateo.Controls;
using MauiReactor;
using MauiReactor.Shapes;
using System;

namespace Chateo.Resources.Styles;

abstract class Theme
{
    public static LightTheme Light { get; } = new();
    public static DarkTheme Dark { get; } = new();

    public static bool IsDarkTheme => MauiControls.Application.Current?.RequestedTheme == Microsoft.Maui.ApplicationModel.AppTheme.Dark;

    public static Theme Current => IsDarkTheme ? Dark : Light;

    public static void ToggleCurrentAppTheme()
    {
        if (MauiControls.Application.Current != null)
        {
            MauiControls.Application.Current.UserAppTheme = IsDarkTheme ? Microsoft.Maui.ApplicationModel.AppTheme.Light : Microsoft.Maui.ApplicationModel.AppTheme.Dark;
        }
    }

    public abstract Color Background { get; }

    public abstract Color Foreground { get; }

    public abstract Color Accent { get; }

    public abstract Color MediumBackground { get; }

    public abstract Color ForegroundAccent { get; }

    public abstract Color MediumForeground { get; }

    public abstract Color Green { get; }


    public abstract Color Neutral { get; }

    public Label Label(string content)
        => new Label(content)
            .FontFamily("MulishSemiBold")
            .TextColor(Foreground);

    public BorderlessEntry Entry()
        => new BorderlessEntry()
            .FontFamily("MulishSemiBold")
            .PlaceholderColor(MediumForeground)
            .TextColor(Foreground)
        ;

    public Button PrimaryButton(string content)
        => new Button(content)
            .CornerRadius(30)
            .FontFamily("MulishSemiBold")
            .TextColor(ForegroundAccent)
            .BackgroundColor(Accent);


    public ImageButton ImageButton(string image)
        => new ImageButton(image)
            .BackgroundColor(Colors.Transparent)
            .BorderWidth(0)
            .BorderColor(Colors.Transparent);

    public ImageButton ImageButton(Icon icon)
        => new ImageButton($"{icon.ToString().ToLowerInvariant()}_icon_{(Current == Light ? "light" : "dark")}.png")
            .BackgroundColor(Colors.Transparent)
            .BorderWidth(0)
            .BorderColor(Colors.Transparent);

    public Image Image(Icon icon)
        => new Image($"{icon.ToString().ToLowerInvariant()}_icon_{(Current == Light ? "light" : "dark")}.png")
            .VCenter()
            .HCenter()
            .HeightRequest(24)
            .WidthRequest(24);

    public Grid Avatar(string avatar, bool online = false)
        => new Grid("*", "*")
        {
            new Image($"{avatar}.png")
                .HeightRequest(48)
                .WidthRequest(48)
                .HCenter()
                .VCenter(),

            new Ellipse()
                .Fill(Green)
                .StrokeThickness(2)
                .Stroke(Colors.White)
                .HeightRequest(14)
                .WidthRequest(14)
                .HEnd()
                .VStart()
                .IsVisible(online)
        }
        .HeightRequest(52)
        .WidthRequest(52);

    public Border BorderedImage(Icon icon)
        => new Border
        {
            Image(icon)
                .HeightRequest(48)
                .WidthRequest(48)
                .VCenter()
                .HCenter()
        }
        .Margin(0)
        .Stroke(MediumForeground)
        .StrokeThickness(3)
        .StrokeCornerRadius(18);

    public Border BorderedImage(string image, bool highlighted = false)
        => new Border
        {
            new Image(image)
                .HeightRequest(48)
                .WidthRequest(48)
                .VCenter()
                .HCenter()
        }
        .Margin(0)
        .Stroke(MediumForeground)
        .StrokeThickness(3)
        .StrokeCornerRadius(18);
}

public enum Icon
{
    Back,

    Chat,

    Chats,

    ChatsMenu,

    Check,

    Comment,

    Contacts,

    Dots,

    Email,

    Folder,

    Help,

    Notification,

    Plus,

    Privacy,

    User,

    UserPlus,

    Search,
    
    SearchOverlay,

    StoryPlus,

    Send,

    Menu,

    Forward,

    Account,

    Appearance,

}

class LightTheme : Theme
{
    public override Color Background => Colors.White;    

    public override Color Accent => Color.FromArgb("#002DE3");

    public override Color Foreground => Color.FromArgb("#0F1828");

    public override Color MediumBackground => Color.FromArgb("#F7F7FC");

    public override Color ForegroundAccent => Color.FromArgb("#F7F7FC");

    public override Color MediumForeground => Color.FromArgb("#ADB5BD");

    public override Color Neutral => Color.FromRgba("#EDEDED");

    public override Color Green => Color.FromArgb("#2CC069");
}

class DarkTheme : Theme
{
    public override Color Background => Color.FromArgb("#0F1828");

    public override Color Accent => Color.FromArgb("#375FFF");

    public override Color Foreground => Color.FromArgb("#F7F7FC");
    

    public override Color MediumBackground => Color.FromArgb("#152033");

    public override Color ForegroundAccent => Color.FromArgb("#F7F7FC");

    public override Color MediumForeground => Color.FromArgb("#ADB5BD");

    public override Color Neutral => Color.FromRgba("#152033");

    public override Color Green => Color.FromArgb("#2CC069");
}