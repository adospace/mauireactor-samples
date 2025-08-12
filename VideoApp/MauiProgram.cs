using CommunityToolkit.Maui;
using MauiReactor;
using VideoApp.Pages;
using VideoApp.Services;

namespace VideoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiReactorApp<MainPage>()
            .UseMauiCommunityToolkitMediaElement()
#if DEBUG
            //.EnableMauiReactorHotReload()
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

        builder.Services.ConfigureServices();

        return builder.Build();
    }
}