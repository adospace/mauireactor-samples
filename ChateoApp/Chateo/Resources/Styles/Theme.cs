

using MauiReactor;

namespace Chateo.Resources.Styles;

abstract class Theme
{
    public static LightTheme Light { get; } = new();
    public static DarkTheme Dark { get; } = new();

    public static Theme Current => MauiControls.Application.Current?.RequestedTheme == Microsoft.Maui.ApplicationModel.AppTheme.Dark ? Dark : Light;

    public abstract Color Background { get; }

    public abstract Color Foreground { get; }

    public abstract Color Primary { get; }

    public abstract Color MediumBackground { get; }

    public Label Label(string content)
        => new Label(content)
            .FontFamily("MulishSemiBold")
            .TextColor(Foreground);

    public Entry Entry()
        => new Entry()
            .FontFamily("MulishSemiBold")
            .TextColor(Foreground)
        ;

    public Button PrimaryButton(string content)
        => new Button(content)
            .CornerRadius(30)
            .FontFamily("MulishSemiBold")
            .BackgroundColor(Primary)
            .TextColor(Foreground);

}

class LightTheme : Theme
{
    public override Color Background => Colors.White;    

    public override Color Primary => Color.FromArgb("#002DE3");

    public override Color Foreground => Color.FromArgb("#0F1828");

    public override Color MediumBackground => Color.FromArgb("#F7F7FC");
}

class DarkTheme : Theme
{
    public override Color Background => Color.FromArgb("#0F1828");

    public override Color Primary => Color.FromArgb("#375FFF");

    public override Color Foreground => Color.FromArgb("#F7F7FC");

    public override Color MediumBackground => Color.FromArgb("#152033");
}