using MauiReactor;
using RecipeApp.Pages;


namespace RecipeApp;

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
                fonts.AddFont("Rubik-Bold.ttf", "RubikBold");
                fonts.AddFont("Rubik-Light.ttf", "RubikLight");
                fonts.AddFont("Rubik-Medium.ttf", "RubikMedium");
                fonts.AddFont("Rubik-Regular.ttf", "RubikRegular");
            });

        return builder.Build();
    }
}
