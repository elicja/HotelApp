using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using HotelAppLibrary.Interfaces;
using HotelAppLibrary.Data.EF;
using HotelAppLibrary.Data.Sqllite;
using HotelAppLibrary.Data.Sql;

namespace HotelApp.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServiceProvider serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        IConfiguration config = builder.Build();

        var services = new ServiceCollection();
        services.AddTransient<MainWindow>();
        services.AddTransient<CheckInForm>();
        services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();
        services.AddTransient<IEFDataAccess, EFDataAccess>();

        

        services.AddSingleton(config);

        string dbChoice = config.GetValue<string>("DatabaseChoice").ToLower();
        if (dbChoice == "sql")
        {
            services.AddTransient<IDatabaseData, SqlData>();
        }
        else if (dbChoice == "sqlite")
        {
            services.AddTransient<IDatabaseData, SqliteData>();
        }
        else if (dbChoice == "ef")
        {
            services.AddTransient<IDatabaseData, EFDataAccess>();
        }
        else
        {
            // Default
            services.AddTransient<IDatabaseData, SqlData>();
        }

        serviceProvider = services.BuildServiceProvider();
        var mainWindow = serviceProvider.GetService<MainWindow>();

        mainWindow.Show();
    }
}
