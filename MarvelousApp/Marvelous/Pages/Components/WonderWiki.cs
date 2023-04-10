using Marvelous.Models;
using MauiReactor;
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
        return new Grid
        {
            RenderBackground(),
        }
        .OnSizeChanged(OnContainerSizeChanged);
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

    VisualNode RenderBackground()
    {
        var config = Illustration.Config[_wonderType];
        return new Grid
        {
            new AbsoluteLayout
            {
                config.BackgroundImages?.Select(RenderBackgroundIllustrationImage),
            },

            new Image(config.MainObject)
                //.Opacity(opacity)
                .Margin(config.MarginLeft, config.MarginTop, 0, 0)
                //.ScaleX(config.ScaleX)
                //.ScaleY(config.ScaleY)
                ,
        }
        .VStart();
    }

    private Image RenderBackgroundIllustrationImage(IllustrationImage image)
    {
        return new Image(image.Source)
            .AbsoluteLayoutBounds(image.GetFinalBounds(State.ContainerSize))
            ;
    }
}
