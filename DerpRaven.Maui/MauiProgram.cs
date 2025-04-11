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
        builder.Services.AddSingleton<CartPage>();
        builder.Services.AddSingleton<CustomRequestPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<OrderHistoryPage>();
        builder.Services.AddSingleton<PaymentPage>();
        builder.Services.AddSingleton<PortfolioPage>();
        builder.Services.AddSingleton<ProductPage>();
        builder.Services.AddSingleton<ProductsListPage>();
        builder.Services.AddSingleton<CreateCustomRequestPage>();
        builder.Services.AddSingleton<ViewCustomRequestsPage>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<SplashScreen>();
        builder.Services.AddSingleton<App>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<CartPageViewModel>();
        builder.Services.AddSingleton<CustomRequestPageViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<OrderHistoryPageViewModel>();
        builder.Services.AddSingleton<PaymentPageViewModel>();
        builder.Services.AddSingleton<PortfolioPageViewModel>();
        builder.Services.AddSingleton<ProductPageViewModel>();
        builder.Services.AddSingleton<ProductsListPageViewModel>();
        builder.Services.AddSingleton<CreateCustomRequestPageViewModel>();
        builder.Services.AddSingleton<ViewCustomRequestsPageViewModel>();
        builder.Services.AddSingleton<PlushiePortfolioPage>();
        builder.Services.AddSingleton<ArtPortfolioPage>();
        return builder;
    }

    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IKeycloakClient, KeycloakClient>(k =>
        {
            var oktaClientConfiguration = new OktaClientConfiguration()
            {
                //Verify by adding .well-known/openid-configuration to the URL
                Domain = "https://engineering.snow.edu/auth/realms/SnowCollege/",
                ClientId = "DerpClientSpring25",
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

