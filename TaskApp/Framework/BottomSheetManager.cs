using MauiReactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The49.Maui.BottomSheet;

namespace TaskApp.Framework;

static class BottomSheetManager
{
    class BottomSheetVisualContainer : ContentView
    {
        public void Unmount()
        {
            base.OnUnmount();
        }
    }


    static BottomSheet? _sheet;
    static BottomSheetVisualContainer? _bottomSheetVisualContainer;
    private static ITemplateHost? _templateHost;

    public static async Task ShowAsync(Func<VisualNode> contentRender, Action<BottomSheet>? configureSheet = null)
    {
        if (_sheet != null)
        {
            return;
        }

        _bottomSheetVisualContainer = new BottomSheetVisualContainer()
        {
            contentRender()
        };

        _templateHost = TemplateHost.Create(_bottomSheetVisualContainer);

        _sheet = new BottomSheet
        {
            Content = (MauiControls.View)_templateHost.NativeElement!
        };

        configureSheet?.Invoke(_sheet);

        _sheet.Dismissed += _sheet_Dismissed;

        await _sheet.ShowAsync();
    }

    private static void _sheet_Dismissed(object? sender, DismissOrigin e)
    {
        if (_sheet == null)
        {
            return;
        }

        _bottomSheetVisualContainer?.Unmount();

        (_templateHost as IHostElement)?.Stop();

        _sheet.Dismissed -= _sheet_Dismissed;
        _sheet = null;
    }

    public static async Task DismissAsync()
    {
        if (_sheet == null)
        {
            return;
        }

        await _sheet.DismissAsync();
        _sheet = null;
    }
}
