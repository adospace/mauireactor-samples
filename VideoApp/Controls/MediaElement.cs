using CommunityToolkit.Maui.Views;
using MauiReactor.Internals;
using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoApp.Controls;


public partial interface IMediaElement : MauiReactor.IView
{
    PropertyValue<bool>? ShowsPlaybackControls { get; set; }
    Uri? SourceUri { get; set; }
}

public partial class MediaElement<T>
{
    class NativeMediaElementCache
    {
        public const int CacheSize = 3;
        private readonly List<CommunityToolkit.Maui.Views.MediaElement> _cache = new();

        public bool TryGetValue(Uri sourceUri, out CommunityToolkit.Maui.Views.MediaElement? mediaElement)
        {
            mediaElement = null;

            for (int i = 0; i < _cache.Count; i++)
            {
                if (_cache[i].Source is UriMediaSource uriMediaSource &&
                    uriMediaSource.Uri != null &&
                    uriMediaSource.Uri.Equals(sourceUri) == true)
                {
                    mediaElement = _cache[i];
                    _cache.RemoveAt(i);
                    _cache.Insert(0, mediaElement);
                    return true;
                }
            }

            return false;
        }

        public void Add(CommunityToolkit.Maui.Views.MediaElement mediaElement)
        {
            if (_cache.Count == CacheSize)
            {
                _cache[^1].Handler?.DisconnectHandler();
                _cache.RemoveAt(_cache.Count - 1);
            }

            _cache.Insert(0, mediaElement);
        }
    }

    static readonly NativeMediaElementCache _controlCache = new();

    PropertyValue<bool>? IMediaElement.ShowsPlaybackControls { get; set; }
    Uri? IMediaElement.SourceUri { get; set; }

    protected override void OnMigrating(VisualNode newNode)
    {
        var newAsIMediaElement = (IMediaElement)newNode;
        if (newAsIMediaElement.SourceUri != null &&
            _controlCache.TryGetValue(newAsIMediaElement.SourceUri, out var player))
        {
            _nativeControl = player;
        }
        else
        {
            _nativeControl = null;
        }

        base.OnMigrating(newNode);
    }

    protected override void OnMount()
    {
        base.OnMount();

        var thisAsIMediaElement = (IMediaElement)this;

        if (thisAsIMediaElement.SourceUri != null)
        {
            _controlCache.Add(Validate.EnsureNotNull(NativeControl));
        }
    }

    partial void OnEndUpdate()
    {
        Validate.EnsureNotNull(NativeControl);
        var thisAsIMediaElement = (IMediaElement)this;
        SetPropertyValue(NativeControl, CommunityToolkit.Maui.Views.MediaElement.ShowsPlaybackControlsProperty, thisAsIMediaElement.ShowsPlaybackControls);

        if (thisAsIMediaElement.SourceUri != null)
        {
            if (NativeControl.Source is not UriMediaSource ||
                (NativeControl.Source is UriMediaSource uriMediaSource &&
                uriMediaSource.Uri != null &&
                uriMediaSource.Uri.Equals(thisAsIMediaElement.SourceUri) == false))
            {
                NativeControl.Source = thisAsIMediaElement.SourceUri;
            }
        }
    }
}

[Scaffold(typeof(CommunityToolkit.Maui.Views.MediaElement))]
public partial class MediaElement
{
}

public static partial class MediaElementExtensions
{
    public static T ShowsPlaybackControls<T>(this T mediaElement, bool showsPlaybackControls)
        where T : IMediaElement
    {
        mediaElement.ShowsPlaybackControls = new PropertyValue<bool>(showsPlaybackControls);
        return mediaElement;
    }

    public static T SourceUri<T>(this T mediaElement, Uri sourceUri)
        where T : IMediaElement
    {
        mediaElement.SourceUri = sourceUri;
        return mediaElement;
    }
}
