using MauiReactor;
using OrderingApp.Pages;


namespace OrderingApp
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
                    fonts.AddFont("Mulish-Regular.ttf", "MulishRegular");
                    fonts.AddFont("Mulish-SemiBold.ttf", "MulishSemiBold");
                    fonts.AddFont("Mulish-Bold.ttf", "MulishBold");
                });

            return builder.Build();
        }
    }
}