using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurfingApp;

static class StyleExtensions
{
    public static T Style<T>(this T navigableElement, string styleKey) where T : INavigableElement
    {
        return navigableElement.Style(
            (MauiControls.Style)MauiControls.Application.Current.Resources.MergedDictionaries.Last()[styleKey]);
    }
}
