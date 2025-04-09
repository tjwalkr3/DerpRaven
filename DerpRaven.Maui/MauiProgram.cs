using DerpRaven.Maui.ViewModels;
using DerpRaven.Maui.Views;
using Microsoft.Extensions.Logging;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.ApiClients;
using CommunityToolkit.Maui;
namespace DerpRaven.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
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
        builder.Services.AddTransient<CreateCustomRequestPage>();
        builder.Services.AddTransient<ViewCustomRequestsPage>();
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<SplashScreen>();
        builder.Services.AddTransient<App>();
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
        builder.Services.AddTransient<CreateCustomRequestPageViewModel>();
        builder.Services.AddTransient<ViewCustomRequestsPageViewModel>();
        builder.Services.AddTransient<PlushiePortfolioPage>();
        builder.Services.AddTransient<ArtPortfolioPage>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddScoped<IKeycloakClient, KeycloakClient>(k =>
        {
            var oktaClientConfiguration = new OktaClientConfiguration()
            {
                //Verify by adding .well-known/openid-configuration to the URL
                Domain = "https://engineering.snow.edu/auth/realms/SnowCollege/",
                ClientId = "JonathanMauiAuth",
                RedirectUri = "myapp://callback",
                Browser = new WebBrowserAuthenticator()
            };
            return new KeycloakClient(oktaClientConfiguration);
        });

        builder.Services.AddSingleton(sp => new HttpClient
        {
            BaseAddress = new Uri("http://10.0.2.2:5077")
        });

        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<IImageClient, ImageClient>();
        builder.Services.AddSingleton<ICustomRequestClient, CustomRequestClient>();
        builder.Services.AddSingleton<IPortfolioClient, PortfolioClient>();
        return builder;
    }
}

