using System;
using Microsoft.Maui.Platform;
using MauiReactor;

namespace Chateo.Controls.Native
{
    public class BorderlessEntry : MauiControls.Entry
    {
        public BorderlessEntry()
        {
        }

        public static void Configure()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if ANDROID
                    handler.PlatformView.SetSelectAllOnFocus(true);
                    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    handler.PlatformView.FocusChange += (s, e) =>
                    {
                        if (!e.HasFocus &&
                            Microsoft.Maui.ApplicationModel.Platform.CurrentActivity != null)
                            Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.HideKeyboard(handler.PlatformView);
                    };


#elif IOS || MACCATALYST
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                    handler.PlatformView.EditingDidBegin += (s, e) =>
                    {
                        handler.PlatformView.PerformSelector(new ObjCRuntime.Selector("selectAll"), null, 0.0f);
                    };
#elif WINDOWS
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                    handler.PlatformView.Background = null;

                    handler.PlatformView.GotFocus += (s, e) =>
                    {
                        handler.PlatformView.SelectAll();
                    };
#endif
                }
            });
        }
    }
}

namespace Chateo.Controls
{
    [Scaffold(typeof(Native.BorderlessEntry))]
    public partial class BorderlessEntry
    {

    }
}
