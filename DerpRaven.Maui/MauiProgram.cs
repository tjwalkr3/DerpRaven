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
        builder.Services.AddTransient<CartPage>();
        builder.Services.AddTransient<CustomRequestPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<OrderHistoryPage>();
        builder.Services.AddTransient<PaymentPage>();
        builder.Services.AddTransient<PortfolioPage>();
        builder.Services.AddTransient<ProductPage>();
        builder.Services.AddTransient<ProductsListPage>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<CartPageViewModel>();
        builder.Services.AddTransient<CustomRequestPageViewModel>();
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<OrderHistoryPageViewModel>();
        builder.Services.AddTransient<PaymentPageViewModel>();
        builder.Services.AddTransient<PortfolioPageViewModel>();
        builder.Services.AddTransient<ProductPageViewModel>();
        builder.Services.AddTransient<ProductsListPageViewModel>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {

        return builder;
    }

}

