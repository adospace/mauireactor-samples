using Marvelous.Models;
using MauiReactor;
using MauiReactor.Canvas;
using MauiReactor.Compatibility;
using MauiReactor.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous.Pages.Components;

class WonderWikiState
{
    public Size ContainerSize { get; set; }
    public double ScrollY { get; set; }
}

partial class WonderWiki : Component<WonderWikiState>
{
    [Prop]
    WonderType _wonderType;

    public override VisualNode Render() 
        => Grid(
            RenderBackground(),

            RenderMain()
        );

    Grid RenderBackground()
    {
        var config = Illustration.Config[_wonderType];
        return Grid(
            Grid(
                AbsoluteLayout(
                    config.BackgroundImages?.Select(RenderBackgroundIllustrationImage).ToArray()
                ),
                AbsoluteLayout(
                    RenderBackgroundIllustrationImage(config.MainObjectEditorialImage)
                )
            )
            .BackgroundColor(config.SecondaryColor)
            .OnSizeChanged(OnContainerSizeChanged)
            .HeightRequest(260)
            .VStart(),

            Rectangle()
                .Margin(0,290,0,0)
                .Fill(config.PrimaryColor)
                .VEnd()
        )
        .BackgroundColor(config.PrimaryColor);
    }

    Image RenderBackgroundIllustrationImage(IllustrationImage image)
    {
        return Image(image.Source)
            .AbsoluteLayoutBounds(image.GetFinalBounds(State.ContainerSize))
            .Opacity(image.Opacity);
    }

    void OnContainerSizeChanged(object? sender, EventArgs args)
    {
        var container = (MauiControls.Grid?)sender;
        if (container == null)
        {
            return;
        }

        SetState(s => s.ContainerSize = container.Bounds.Size);
    }

    VisualNode RenderMain()
        => Render<double>(context =>
        {
            var wikiConfig = Wonder.Config[_wonderType];
            var wonderConfig = Illustration.Config[_wonderType];

            var scrollY = context.Value;

            return new ScrollView
            {
                new VStack
                {
                    new Grid("52", "* Auto *")
                    {
                        Rectangle()
                            .HeightRequest(1)
                            .BackgroundColor(wonderConfig.SecondaryColor)
                            .VCenter()
                            .Margin(20,0)
                            ,

                        Label(wikiConfig.SubTitle.ToUpper())
                            .BackgroundColor(Colors.Transparent)
                            .TextColor(Colors.White)
                            .FontSize(14)
                            .FontFamily("TenorSans")
                            .HCenter()
                            .VCenter()
                            .GridColumn(1),

                        Rectangle()
                            .HeightRequest(1)
                            .BackgroundColor(wonderConfig.SecondaryColor)
                            .VCenter()
                            .Margin(20,0)
                            .GridColumn(2)
                            ,
                    },

                    Label(wikiConfig.Title)
                        .FontFamily("YesevaOne")
                        .FontSize(60)
                        .TextColor(Colors.White)
                        .HCenter()
                        .VerticalTextAlignment(TextAlignment.End)
                        .HeightRequest(150),

                    Label(wikiConfig.RegionTitle.ToUpper())
                        .BackgroundColor(Colors.Transparent)
                        .TextColor(Colors.White)
                        .FontSize(16)
                        .FontFamily("TenorSans")
                        .HCenter()
                        .VerticalTextAlignment(TextAlignment.End)
                        .HeightRequest(70),

                    Separator(isOpen: scrollY < 10),

                    Label($"{wikiConfig.StartYr} {(wikiConfig.StartYr < 0 ? Localization.yearBCE : Localization.yearCE)} to {wikiConfig.EndYr} {(wikiConfig.StartYr < 0 ? Localization.yearBCE : Localization.yearCE)}")
                        .BackgroundColor(Colors.Transparent)
                        .TextColor(Colors.White)
                        .FontFamily("RalewayBold")
                        .HCenter()
                        .VerticalTextAlignment(TextAlignment.End)
                        .HeightRequest(80),
                     

                    new Image($"{_wonderType.ToString().ToLower()}_photo_1.png"),
                }
                .Opacity(Math.Clamp((100 - scrollY) / 300, 0.1, 1.0))

            }
            .Padding(0, 300, 0, 0)
            .OnScrolled((sender, args) => context.Set(s => args.ScrollY));
        });

    Grid Separator(bool isOpen)
    {
        var wonderConfig = Illustration.Config[_wonderType];

        return Grid("42", "*,42,*",
        
            Rectangle()
                .HeightRequest(2)
                .Margin(isOpen ? new Thickness(20,0) : new Thickness(40, 0))
                .WithAnimation(easing: Easing.CubicIn)
                .Fill(wonderConfig.SecondaryColor)
                .VCenter(),

            Image("common_compass_full.png")
                .GridColumn(1)
                .Rotation(isOpen ? 0 : 360)
                .WithAnimation(easing: Easing.BounceOut, duration: 1000)
                .AnchorX(0.5f)
                .AnchorY(0.5f)
                ,

            Rectangle()
                .GridColumn(2)
                .HeightRequest(2)
                .Margin(isOpen ? new Thickness(20,0) : new Thickness(40, 0))
                .WithAnimation(easing: Easing.CubicIn)
                .Fill(wonderConfig.SecondaryColor)
                .VCenter()

        )
        .HeightRequest(42);
    }

}
