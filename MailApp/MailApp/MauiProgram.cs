using MailApp.Pages;
using MauiReactor;
using Microsoft.Extensions.Logging;
using ReactorData.Maui;
using Shiny;
using MailApp.Services;


namespace MailApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiReactorApp<MainPage>()
                .UseShiny()
                
#if DEBUG
                .EnableMauiReactorHotReload()
                .OnMauiReactorUnhandledException(args =>
                {
                    System.Diagnostics.Debug.WriteLine(args.ExceptionObject);
                })
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                });

#if DEBUG
        		builder.Logging.AddDebug();
#endif

#if !WINDOWS
            builder.Services.AddJob(typeof(EmailClientJob));
#endif

            builder.UseReactorData(services =>
            {
                services.AddMailAppServices();
            });

            builder.Services.AddMailAppMauiServices();

            return builder.Build();
        }
    }
}
