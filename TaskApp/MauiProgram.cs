using CommunityToolkit.Maui;
using MauiReactor;
using Microsoft.Extensions.Logging;
using TaskApp.Pages;
using TaskApp.Resources.Styles;
using The49.Maui.BottomSheet;


namespace TaskApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiReactorApp<AppShell>(app =>
            {
                app.UseTheme<ApplicationTheme>();
            })
            .UseMauiCommunityToolkit()
            .UseBottomSheet()
#if DEBUG
            .EnableMauiReactorHotReload()
            .OnMauiReactorUnhandledException(ex =>
            {
                System.Diagnostics.Debug.WriteLine(ex.ExceptionObject);
            })
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("PlusJakartaSans-Regular.ttf", "PlusJakartaSansRegular");
                fonts.AddFont("PlusJakartaSans-SemiBold.ttf", "PlusJakartaSansSemiBold");
                fonts.AddFont("PlusJakartaSans-Medium.ttf", "PlusJakartaSansMedium");
            });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

        CustomizeNativeControls();

        return builder.Build();
    }

    static void CustomizeNativeControls()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });
    }

}