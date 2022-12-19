using Microsoft.Extensions.Logging;
using MonkeyFinder.View;
using CommunityToolkit.Maui;
using MonkeyFinder.Services;

namespace MonkeyFinder;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        }).UseMauiCommunityToolkit();
#if DEBUG
        //builder.Logging.AddDebug();
#endif

        // Services
        builder.Services.AddSingleton<MonkeyService>();
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IMap>(Map.Default);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

        // View Models
        builder.Services.AddSingleton<MonkeysViewModel>();
        builder.Services.AddTransient<MonkeyDetailsViewModel>();
        
        // Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<DetailsPage>();
        
        return builder.Build();
    }
}