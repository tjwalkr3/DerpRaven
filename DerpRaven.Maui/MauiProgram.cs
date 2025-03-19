using DerpRaven.Maui.ViewModels;
using DerpRaven.Maui.Views;
using Microsoft.Extensions.Logging;
namespace DerpRaven.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MainPage>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<MainPageViewModel>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {

        return builder;
    }

}

