using Marvelous.Pages;
using MauiReactor;


namespace Marvelous
{
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("B612Mono-Regular.ttf", "Mono");
                    fonts.AddFont("CinzelDecorative-Black.ttf", "CinzelDecorativeBlack");
                    fonts.AddFont("CinzelDecorative-Bold.ttf", "CinzelDecorativeBold");
                    fonts.AddFont("CinzelDecorative-Regular.ttf", "CinzelDecorativeRegular");
                    fonts.AddFont("Raleway-Bold.ttf", "RalewayBold");
                    fonts.AddFont("Raleway-BoldItalic.ttf", "RalewayBoldItalic");
                    fonts.AddFont("Raleway-ExtraBold.ttf", "RalewayExtraBold");
                    fonts.AddFont("Raleway-ExtraBoldItalic.ttf", "RalewayExtraBoldItalic");
                    fonts.AddFont("Raleway-Italic.ttf", "RalewayItalic");
                    fonts.AddFont("Raleway-Medium.ttf", "RalewayMedium");
                    fonts.AddFont("Raleway-MediumItalic.ttf", "RalewayMediumItalic");
                    fonts.AddFont("Raleway-Regular.ttf", "RalewayRegular");
                    fonts.AddFont("TenorSans-Regular.ttf", "TenorSans");
                    fonts.AddFont("YesevaOne-Regular.ttf", "YesevaOne");
                });

            return builder.Build();
        }
    }
}