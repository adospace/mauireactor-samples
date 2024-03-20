using MauiReactor;
using Microsoft.Extensions.Logging;
using ReactorData;
using ReactorData.Maui;
using ReactorData.Sqlite;
using System;
using System.IO;
using System.Reflection.Metadata;
using TodoApp.Models;
using TodoApp.Pages;


namespace TodoApp;

public static class MauiProgram
{
    static readonly string _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "todo_app.db");

    public static MauiApp CreateMauiApp()
    {
        //if (File.Exists(_dbPath))
        //{
        //    File.Delete(_dbPath);
        //}

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiReactorApp<MainPage>(app =>
            {
                //app.AddResource("Resources/Styles/Colors.xaml");
                //app.AddResource("Resources/Styles/Styles.xaml");
            })
#if DEBUG
            .EnableMauiReactorHotReload()
#endif
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

        //ReactorData
        builder.UseReactorData(services =>
        {
            services.AddReactorData(
                connectionStringOrDatabaseName: $"Data Source={_dbPath}",
                configure: _ => _.Model<Todo>(),
                modelContextConfigure: options =>
                {
                    options.ConfigureContext = context => context.Load<Todo>();
                });
        });




        return builder.Build();
    }
}
