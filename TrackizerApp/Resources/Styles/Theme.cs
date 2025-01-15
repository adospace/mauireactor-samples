using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackizerApp.Resources.Styles;

static class ApplicationTheme
{
    public static Color Grey100 => Color.FromArgb("#0E0E12");
    public static Color Grey80 => Color.FromArgb("#1C1C23");
    public static Color Grey70 => Color.FromArgb("#353542");
    public static Color Grey60 => Color.FromArgb("#4E4E61");
    public static Color Grey50 => Color.FromArgb("#666680");
    public static Color Grey40 => Color.FromArgb("#83839C");
    public static Color Grey30 => Color.FromArgb("#A2A2B5");
    public static Color Grey20 => Color.FromArgb("#C1C1CD");
    public static Color Grey10 => Color.FromArgb("#E0E0E6");
    public static Color White => Color.FromArgb("#FFFFFF");
    public static Color Primary100 => Color.FromArgb("#AD7BFF");
    public static Color Primary50 => Color.FromArgb("#7722FF");
    public static Color Primary20 => Color.FromArgb("#924EFF");
    public static Color Primary05 => Color.FromArgb("#C9A7FF");
    public static Color Primary0 => Color.FromArgb("#E4D3FF");
    public static Color Accentp100 => Color.FromArgb("#FF7966");
    public static Color Accentp50 => Color.FromArgb("#FFA699");
    public static Color Accentp0 => Color.FromArgb("#FFD2CC");
    public static Color Accents100 => Color.FromArgb("#00FAD9");
    public static Color Accents50 => Color.FromArgb("#7DFFEE");


    public static double FontSizeDisplay => 72;
    public static double FontSizeH8 => 56;
    public static double FontSizeH7 => 40;
    public static double FontSizeH6 => 32;
    public static double FontSizeH5 => 24;
    public static double FontSizeH4 => 20;
    public static double FontSizeH3 => 16;
    public static double FontSizeH2 => 14;
    public static double FontSizeH1 => 12;
    public static double FontSizeSubtitle => 20;
    public static double FontSizeBodyLarge => 16;
    public static double FontSizeBodyMedium => 14;
    public static double FontSizeBodySmall => 12;
    public static double FontSizeBodyExtraSmall => 10;

    public static Label H1(string? text = null)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeH1);
    public static Label H2(string? text = null)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeH2);
    public static Label H3(string? text = null)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeH3);
    public static Label H4(string? text = null)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeH4);
    public static Label H5(string? text = null)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeH5);
    public static Label H7(string? text = null)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeH7);
    public static Label BodySmall(string? text)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeBodySmall);
    public static Label BodyMedium(string? text)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeBodyMedium);
    public static Label BodyLarge(string? text)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeBodyLarge);
    public static Label BodyExtraSmall(string? text)
        => Component.Label(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeBodyExtraSmall);

    public static Button Button(string text, Action? onClicked)
        => Component.Button(text)
            .FontFamily("InterRegular")
            .FontSize(FontSizeH2)
            .TextColor(White)
            .CornerRadius(45)
            .OniOS(_=>_.CornerRadius(25))
            .BackgroundColor(White.WithAlpha(0.1f))
            .BorderColor(White.WithAlpha(0.15f))
            .BorderWidth(1)
            .HeightRequest(48)
            .OnClicked(onClicked);

    public static Grid PrimaryButton(string text, Action? onClicked)
        => Component.Grid(
            Component.Border()
                .StrokeCornerRadius(45)
                .BackgroundColor(Color.FromRgba("#FF7966")),

            Component.Border()
                .StrokeCornerRadius(45)
                .Background(new RadialGradient((Colors.Transparent, 0.5f), (Color.FromRgba("#FF7F37").WithAlpha(0.5f), 1.0f))),

            Component.Button()
                .Text(text)
                .TextColor(White)
                .FontFamily("InterRegular")
                .FontSize(FontSizeH2)
                .CornerRadius(45)
                .OniOS(_=>_.CornerRadius(25))
                .BackgroundColor(Colors.Transparent)
                .HeightRequest(48)
                .BorderColor(White.WithAlpha(0.15f))
                .BorderWidth(1)
                .OnClicked(onClicked)
            )
        .Shadow(new Shadow()
            .Brush(new MauiControls.SolidColorBrush(Color.FromRgba(255, 121, 102, 0.50))));

    public static Grid PrimaryImageButton(string text, Action? onClicked, Color baseColor, Color fontColor, string imageSource)
        => Component.Grid(
            Component.Border()
                .StrokeCornerRadius(45)
                .BackgroundColor(baseColor),

            Component.Border()
                .StrokeCornerRadius(45)
                .Background(new RadialGradient((Colors.Transparent, 0.5f), (baseColor.WithLuminosity(0.5f).WithAlpha(0.1f), 1.0f))),

            Component.HStack(spacing: 8,
                Component.Image(imageSource)
                    .WidthRequest(16),
                Component.Label(text)
                    .FontFamily("InterRegular")
                    .FontSize(FontSizeH2)
                    .TextColor(fontColor)
                )
            .HCenter()
            .VCenter(),

            Component.Button()
                .CornerRadius(45)
                .OniOS(_=>_.CornerRadius(25))
                .BackgroundColor(Colors.Transparent)
                .HeightRequest(48)
                .BorderColor(White.WithAlpha(0.15f))
                .BorderWidth(1)
                .OnClicked(onClicked)
            )
        .Shadow(Component.Shadow()
            .Brush(new MauiControls.SolidColorBrush(baseColor.WithLuminosity(0.5f).WithAlpha(0.5f))));


    /*
const gray_100 = '#0E0E12';
const grey_80 = '#1C1C23';
const grey_70 = '#353542';
const grey_60 = '#4E4E61';
const grey_50 = '#666680';
const grey_40 = '#83839C';
const grey_30 = '#A2A2B5';
const grey_20 = '#C1C1CD';
const grey_10 = '#E0E0E6';
const white = '#FFFFFF';
const primary_100 = '#AD7BFF';
const primary_50 = '#7722FF';
const primary_20 = '#924EFF';
const primary_05 = '#C9A7FF';
const primary_0 = '#E4D3FF';
const accent_p_100 = '#FF7966';
const accent_p_50 = '#FFA699';
const accent_p_0 = '#FFD2CC';
const accent_s_100 = '#00FAD9';
const accent_s_50 = '#7DFFEE';
     */
}
