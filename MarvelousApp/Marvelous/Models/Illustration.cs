using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.Models;



record Illustration(
    WonderType Type,
    string ForegroundLeft,
    string ForegroundRight,
    string? TopLeft,
    string? TopRight,
    string MainObject,
    string Photo1,
    string Photo2,
    string Photo3,
    string Photo4,
    string Map,
    string SkySphere,
    string WonderButton,
    string Flattened,
    double ScaleX,
    double ScaleY,
    double MarginTop,
    double MarginLeft,
    Color PrimaryColor,
    Color SecondaryColor,
    IllustrationImage[]? BackgroundImages = null,
    IllustrationImage[]? ForegroundImages = null
    )
{
    Brush? _backgroundBrush;
    public Brush BackgroundBrush
        => _backgroundBrush ??= new LinearGradientBrush(new GradientStopCollection
        {
            new GradientStop(SecondaryColor, 0),
            new GradientStop(SecondaryColor, 0.5f),
            new GradientStop(PrimaryColor, 1),
        }, new Point(0, 0), new Point(0, 1));

    Brush? _foregroundBrush;
    public Brush ForegroundBrush
        => _foregroundBrush ??= new LinearGradientBrush(new GradientStopCollection
        {
            new GradientStop(SecondaryColor.WithAlpha(0.0f), 0),
            new GradientStop(SecondaryColor.WithAlpha(0.0f), 0.5f),
            new GradientStop(PrimaryColor, 1),
        }, new Point(0, 0), new Point(0, 1));

    Brush? _secondaryBrush;
    public Brush SecondaryBrush
        => _secondaryBrush ??= new SolidColorBrush(SecondaryColor);

    public static Dictionary<WonderType, Illustration> Config { get; } = new()
    {
        {
            WonderType.Colosseum, new Illustration(
                Type: WonderType.Colosseum,
                ForegroundLeft: "colosseum_foreground_left.png",
                ForegroundRight: "colosseum_foreground_right.png",
                TopLeft: null,
                TopRight: null,
                MainObject: "colosseum_colosseum.png",
                Photo1: "colosseum_photo_1.jpg",
                Photo2: "colosseum_photo_2.jpg",
                Photo3: "colosseum_photo_3.jpg",
                Photo4: "colosseum_photo_4.jpg",
                Map: "colosseum_map.jpg",
                SkySphere: "colosseum_sun.png",
                WonderButton: "colosseum_wonder_button.png",
                Flattened: "colosseum_flattened.jpg",
                ScaleX: 1.76,
                ScaleY: 1.76,
                MarginTop: 0,
                MarginLeft: 0,
                PrimaryColor: Color.FromArgb("#1c746b"),
                SecondaryColor: Color.FromArgb("#47a49b"),
                BackgroundImages: new[]
                {
                    new IllustrationImage("colosseum_sun.png", new Rect(0.1, -0.1, 0.54, 0.54), new Rect(0.1, -0.02, 0.54, 0.54)),

                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, 0.13, 0.5, 0.5), new Rect(0.1, 0.13, 0.5, 0.5), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(1.2, 0.14, 0.34, 0.34), new Rect(-0.06, 0.14, 0.34, 0.34), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, -0.05, 0.6, 0.6), new Rect(0.2, -0.05, 0.6, 0.6), Opacity: 0.3),
                },

                ForegroundImages: new[]
                {
                    new IllustrationImage("colosseum_foreground_left.png", new Rect(-1.4, 0.48, 0.6, 0.6), new Rect(-0.09, 0.48, 0.6, 0.6)),
                    new IllustrationImage("colosseum_foreground_right.png", new Rect(1, 0.48, 0.6, 0.6), new Rect(0.48, 0.56, 0.6, 0.6)),
                }
            )
        },
        {
            WonderType.ChichenItza, new Illustration(
                Type: WonderType.ChichenItza,
                ForegroundLeft: "chichen_itza_foreground_left.png",
                ForegroundRight: "chichen_itza_foreground_right.png",
                TopLeft: "chichen_itza_top_left.png",
                TopRight: "chichen_itza_top_right.png",
                MainObject: "chichen_itza_chichen.png",
                Photo1: "chichen_itza_photo_1.jpg",
                Photo2: "chichen_itza_photo_2.jpg",
                Photo3: "chichen_itza_photo_3.jpg",
                Photo4: "chichen_itza_photo_4.jpg",
                Map: "chichen_itza_map.jpg",
                SkySphere: "chichen_itza_sun.png",
                WonderButton: "chichen_itza_wonder_button.png",
                Flattened: "chichen_itza_flattened.jpg",
                ScaleX: 2.88,
                ScaleY: 2.88,
                MarginTop: 0,
                MarginLeft: 0,
                PrimaryColor: Color.FromArgb("#144f27"),
                SecondaryColor: Color.FromArgb("#e0cfb7"),
                BackgroundImages: new[]
                {
                    new IllustrationImage("chichen_itza_sun.png", new Rect(0.57, 0.3, 0.35, 0.4), new Rect(0.57, 0.20, 0.35, 0.4)),
                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, 0, 0.61, 0.6), new Rect(-0.05, 0, 0.61, 0.6), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(1.2, -0.1, 0.4, 0.4), new Rect(0.5, -0.1, 0.4, 0.4), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, -0.25, 0.6, 0.6), new Rect(0, -0.25, 0.6, 0.6), Opacity: 0.3),
                },

                ForegroundImages: new[]
                {
                    new IllustrationImage("chichen_itza_foreground_left.png", new Rect(-1, 0.20, 1.15, 1.15), new Rect(-0.46, 0.20, 1.15, 1.15)),
                    new IllustrationImage("chichen_itza_foreground_right.png", new Rect(1, 0.45, 0.7, 0.7), new Rect(0.65, 0.45, 0.7, 0.7)),

                    new IllustrationImage("chichen_itza_top_left.png", new Rect(-1, -0.55, 1.1, 1.1), new Rect(-0.4, -0.55, 1.1, 1.1)),
                    new IllustrationImage("chichen_itza_top_right.png", new Rect(1, -0.5, 1.1, 1.1), new Rect(0.4, -0.5, 1.1, 1.1)),
                }
            )
        },
        {
            WonderType.ChristRedeemer, new Illustration(
                Type: WonderType.ChristRedeemer,
                ForegroundLeft: "christ_the_redeemer_foreground_left.png",
                ForegroundRight: "christ_the_redeemer_foreground_right.png",
                TopLeft: null,
                TopRight: null,
                MainObject: "christ_the_redeemer_redeemer.png",
                Photo1: "christ_the_redeemer_photo_1.jpg",
                Photo2: "christ_the_redeemer_photo_2.jpg",
                Photo3: "christ_the_redeemer_photo_3.jpg",
                Photo4: "christ_the_redeemer_photo_4.jpg",
                Map: "christ_redeemer_map.jpg",
                SkySphere: "christ_the_redeemer_sun.png",
                WonderButton: "christ_the_redeemer_wonder_button.png",
                Flattened: "christ_the_redeemer_flattened.jpg",
                ScaleX: 2.8,
                ScaleY: 2.8,
                MarginTop: 330,
                MarginLeft: 0,
                PrimaryColor: Color.FromArgb("#194d43"),
                SecondaryColor: Color.FromArgb("#eb7a65"),
                BackgroundImages: new[]
                {
                    new IllustrationImage("christ_the_redeemer_sun.png", new Rect(0.45, -0.1, 0.43, 0.43), new Rect(0.46, 0.02, 0.43, 0.43)),

                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, 0.13, 0.5, 0.5), new Rect(0.1, 0.13, 0.5, 0.5), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(1.2, 0.14, 0.34, 0.34), new Rect(-0.06, 0.14, 0.34, 0.34), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, -0.05, 0.6, 0.6), new Rect(0.2, -0.05, 0.6, 0.6), Opacity: 0.3),
                },

                ForegroundImages: new[]
                {
                    new IllustrationImage("christ_the_redeemer_foreground_left.png", new Rect(-1.4, -0.05, 1.64, 1.64), new Rect(-0.74, -0.05, 1.64, 1.64), Opacity: 0.8),
                    new IllustrationImage("christ_the_redeemer_foreground_right.png", new Rect(1, 0.08, 1.6, 1.6), new Rect(0.13, 0.08, 1.6, 1.6)),
                }
            )
        },
        {
            WonderType.GreatWall, new Illustration(
                Type: WonderType.GreatWall,
                ForegroundLeft: "great_wall_of_china_foreground_left.png",
                ForegroundRight: "great_wall_of_china_foreground_right.png",
                TopLeft: null,
                TopRight: null,
                MainObject: "great_wall_of_china_great_wall.png",
                Photo1: "great_wall_of_china_photo_1.jpg",
                Photo2: "great_wall_of_china_photo_2.jpg",
                Photo3: "great_wall_of_china_photo_3.jpg",
                Photo4: "great_wall_of_china_photo_4.jpg",
                Map: "great_wall_map.jpg",
                SkySphere: "great_wall_of_china_sun.png",
                WonderButton: "great_wall_of_china_wonder_button.png",
                Flattened: "great_wall_of_china_flattened.jpg",
                ScaleX: 1.2,
                ScaleY: 1.2,
                MarginTop: 330,
                MarginLeft: 0,
                PrimaryColor: Color.FromArgb("#612925"),
                SecondaryColor: Color.FromArgb("#8faa7b"),
                BackgroundImages: new[]
                {
                    new IllustrationImage("great_wall_of_china_sun.png", new Rect(0.16, -0.1, 0.36, 0.36), new Rect(0.16, -0.02, 0.36, 0.36)),

                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, 0.13, 0.5, 0.5), new Rect(0.1, 0.13, 0.5, 0.5), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(1.2, -0.06, 0.34, 0.34), new Rect(0.18, -0.06, 0.34, 0.34), Opacity: 0.3),
                    new IllustrationImage("common_cloud_white.png", new Rect(-0.5, -0.1, 0.6, 0.6), new Rect(0.4, -0.1, 0.6, 0.6), Opacity: 0.3),
                },

                ForegroundImages: new[]
                {
                    new IllustrationImage("great_wall_of_china_foreground_left.png", new Rect(-1.4, -0.05, 1.64, 1.64), new Rect(-0.74, -0.05, 1.64, 1.64), Opacity: 0.8),
                    new IllustrationImage("great_wall_of_china_foreground_right.png", new Rect(1, 0.08, 1.6, 1.6), new Rect(0.13, 0.08, 1.6, 1.6)),
                }
            )
        },
        {
            WonderType.MachuPicchu, new Illustration(
                Type: WonderType.MachuPicchu,
                ForegroundLeft: "machu_picchu_foreground_back.png",
                ForegroundRight: "machu_picchu_foreground_front.png",
                TopLeft: null,
                TopRight: null,
                MainObject: "machu_picchu_machu_picchu.png",
                Photo1: "machu_picchu_photo_1.jpg",
                Photo2: "machu_picchu_photo_2.jpg",
                Photo3: "machu_picchu_photo_3.jpg",
                Photo4: "machu_picchu_photo_4.jpg",
                Map: "machu_picchu_map.jpg",
                SkySphere: "machu_picchu_sun.png",
                WonderButton: "machu_picchu_wonder_button.png",
                Flattened: "machu_picchu_flattened.jpg",
                ScaleX: 2.5,
                ScaleY: 2.0,
                MarginTop: 0,
                MarginLeft: -70,
                PrimaryColor: Color.FromArgb("#0b4161"),
                SecondaryColor: Color.FromArgb("#bfdace")
            )
        },
        {
            WonderType.Petra, new Illustration(
                Type: WonderType.Petra,
                ForegroundLeft: "petra_foreground_left.png",
                ForegroundRight: "petra_foreground_right.png",
                TopLeft: null,
                TopRight: null,
                MainObject: "petra_petra.png",
                Photo1: "petra_photo_1.jpg",
                Photo2: "petra_photo_2.jpg",
                Photo3: "petra_photo_3.jpg",
                Photo4: "petra_photo_4.jpg",
                Map: "petra_map.jpg",
                SkySphere: "petra_moon.png",
                WonderButton: "petra_wonder_button.png",
                Flattened: "petra_flattened.jpg",
                ScaleX: 2.3,
                ScaleY: 2.3,
                MarginTop: 0,
                MarginLeft: 0,
                PrimaryColor: Color.FromArgb("#424c98"),
                SecondaryColor: Color.FromArgb("#181a62")
            )
        },
        {
            WonderType.PyramidsGiza, new Illustration(
                Type: WonderType.PyramidsGiza,
                ForegroundLeft: "pyramids_foreground_back.png",
                ForegroundRight: "pyramids_foreground_front.png",
                TopLeft: null,
                TopRight: null,
                MainObject: "pyramids_pyramids.png",
                Photo1: "pyramids_photo_1.jpg",
                Photo2: "pyramids_photo_2.jpg",
                Photo3: "pyramids_photo_3.jpg",
                Photo4: "pyramids_photo_4.jpg",
                Map: "pyramids_giza_map.jpg",
                SkySphere: "pyramids_moon.png",
                WonderButton: "pyramids_wonder_button.png",
                Flattened: "pyramids_flattened.jpg",
                ScaleX: 1.8,
                ScaleY: 1.8,
                MarginTop: 0,
                MarginLeft: 0,
                PrimaryColor: Color.FromArgb("#14194c"),
                SecondaryColor: Color.FromArgb("#424c98")
            )
        },
        {
            WonderType.TajMahal, new Illustration(
                Type: WonderType.TajMahal,
                ForegroundLeft: "taj_mahal_foreground_left.png",
                ForegroundRight: "taj_mahal_foreground_right.png",
                TopLeft: "taj_mahal_pool.png",
                TopRight: null,
                MainObject: "taj_mahal_taj_mahal.png",
                Photo1: "taj_mahal_photo_1.jpg",
                Photo2: "taj_mahal_photo_2.jpg",
                Photo3: "taj_mahal_photo_3.jpg",
                Photo4: "taj_mahal_photo_4.jpg",
                Map: "taj_mahal_map.jpg",
                SkySphere: "taj_mahal_sun.png",
                WonderButton: "taj_mahal_wonder_button.png",
                Flattened: "taj_mahal_flattened.jpg",
                ScaleX: 1.8,
                ScaleY: 1.9,
                MarginTop: 0,
                MarginLeft: 0,
                PrimaryColor: Color.FromArgb("#c86552"),
                SecondaryColor: Color.FromArgb("#612925")
            )
        },
    };
}

record IllustrationImage(string Source, Rect InitialBounds, Rect FinalBounds, double Opacity = 1.0)
{
    public Rect GetFinalBounds(Size containerSize)
    {
        return new Rect(
            FinalBounds.X * containerSize.Width,
            FinalBounds.Y * containerSize.Height,
            FinalBounds.Width * containerSize.Width,
            FinalBounds.Height * containerSize.Height);
    }

    public Rect GetInitialBounds(Size containerSize)
    {
        return new Rect(
            InitialBounds.X * containerSize.Width,
            InitialBounds.Y * containerSize.Height,
            InitialBounds.Width * containerSize.Width,
            InitialBounds.Height * containerSize.Height);
    }
}
