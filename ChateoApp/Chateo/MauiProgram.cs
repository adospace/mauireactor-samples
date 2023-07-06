using Chateo.Pages;
using Chateo.Services;
using MauiReactor;
using System;

namespace Chateo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiReactorApp<MainPage>()
#if DEBUG
            .EnableMauiReactorHotReload()
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Mulish-Regular.ttf", "MulishRegular");
                fonts.AddFont("Mulish-SemiBold.ttf", "MulishSemiBold");
            });


        builder.Services.AddChatServices(new Uri("http://192.168.1.33:5000"));

        return builder.Build();
    }
}