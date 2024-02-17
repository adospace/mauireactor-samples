using MauiReactor;
using ShellTestPage.Pages;


namespace ShellTestPage
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiReactorApp<MainPage8>(app =>
                {
                    app.AddResource("Resources/Styles/Colors.xaml");
                    app.AddResource("Resources/Styles/Styles.xaml");
                    app.AddResource("Resources/Styles/FixTabStyles.xaml");
                })
#if DEBUG
                .EnableMauiReactorHotReload()
                .OnMauiReactorUnhandledException(ex => 
                {
                    System.Diagnostics.Debug.WriteLine(ex.ExceptionObject);
                })
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                    fonts.AddFont("FontAwesomeRegular.otf", "FontRegular");
                    fonts.AddFont("FontAwesomeBrands.otf", "FontBrands");
                    fonts.AddFont("FontAwesomeSolid.otf", "FontSolid");
                });

            return builder.Build();
        }
    }
}
