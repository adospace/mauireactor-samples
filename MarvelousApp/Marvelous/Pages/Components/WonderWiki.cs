using Marvelous.Models;
using MauiReactor;
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
}

class WonderWiki : Component<WonderWikiState>
{
    private WonderType _wonderType;

    public WonderWiki Type(WonderType wonderType)
    {
        _wonderType = wonderType;
        return this;
    }

    public override VisualNode Render()
    {
        var config = Illustration.Config[_wonderType];
        return new Grid
        {
            RenderBackground(),
        };
    }

    VisualNode RenderBackground()
    {
        var config = Illustration.Config[_wonderType];
        return new Grid
        {
            new Grid
            {
                new AbsoluteLayout
                {
                    config.BackgroundImages?.Select(RenderBackgroundIllustrationImage),
                },
                new AbsoluteLayout
                {
                    RenderBackgroundIllustrationImage(config.MainObjectEditorialImage)
                },
            }
            .BackgroundColor(config.SecondaryColor)
            .OnSizeChanged(OnContainerSizeChanged)
            .HeightRequest(260)
            .VStart(),

            new Rectangle()
                .Margin(0,290,0,0)
                .Fill(config.PrimaryColor)
                .VEnd()
        }
        .BackgroundColor(config.PrimaryColor);
    }

    private Image RenderBackgroundIllustrationImage(IllustrationImage image)
    {
        return new Image(image.Source)
            .AbsoluteLayoutBounds(image.GetFinalBounds(State.ContainerSize))
            .Opacity(image.Opacity)
            ;
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
}
