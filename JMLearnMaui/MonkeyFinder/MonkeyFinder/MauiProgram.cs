using Microsoft.Extensions.Logging;
using MonkeyFinder.Services;
using MonkeyFinder.View;
using Microsoft.Maui.Platform;

namespace MonkeyFinder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif
        
// #if IOS // add handler in MauiProgram.cs
        // Microsoft.Maui.Handlers.ScrollViewHandler.Mapper.AppendToMapping("custom", (handler,view) => {
        //     handler.PlatformView.UpdateContentSize(handler.VirtualView.ContentSize); 
        //     handler.PlatformArrange(handler.PlatformView.Frame.ToRectangle());
        // });
// #endif

        // Built -in Services
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IMap>(Map.Default);

        // Services
        builder.Services.AddSingleton<MonkeyService>();

        // View Models
        builder.Services.AddSingleton<MonkeysViewModel>();
        builder.Services.AddTransient<MonkeyDetailsViewModel>();
        
        // Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<DetailsPage>();

        return builder.Build();
    }
}