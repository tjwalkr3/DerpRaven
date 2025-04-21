using DerpRaven.Maui.ViewModels;
using DerpRaven.Maui.Views;
using Microsoft.Extensions.Logging;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.ApiClients;
using CommunityToolkit.Maui;
using IeuanWalker.Maui.StateButton;
namespace DerpRaven.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseStateButton()
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
        builder.Services.AddTransient<CustomRequestPage>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<OrderHistoryPage>();
        builder.Services.AddTransient<PortfolioPage>();
        builder.Services.AddSingleton<ProductPage>();
        builder.Services.AddTransient<ProductsListPage>();
        builder.Services.AddSingleton<CreateCustomRequestPage>();
        builder.Services.AddSingleton<ViewCustomRequestsPage>();
        builder.Services.AddSingleton<PlushieProductsListPage>();
        builder.Services.AddSingleton<ArtProductsListPage>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<SplashScreen>();
        builder.Services.AddSingleton<App>();
        builder.Services.AddSingleton<AppShell>();
        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<CartPageViewModel>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<OrderHistoryPageViewModel>();
        builder.Services.AddSingleton<PortfolioPageViewModel>();
        builder.Services.AddSingleton<ProductPageViewModel>();
        builder.Services.AddSingleton<ProductsListPageViewModel>();
        builder.Services.AddSingleton<CreateCustomRequestPageViewModel>();
        builder.Services.AddSingleton<ViewCustomRequestsPageViewModel>();
        builder.Services.AddSingleton<PlushiePortfolioPage>();
        builder.Services.AddSingleton<ArtPortfolioPage>();
        builder.Services.AddSingleton<AppShellViewModel>();
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
#if DEBUG
            BaseAddress = new Uri("http://10.0.2.2:5077")
#else
            BaseAddress = new Uri("http://derpraven-api.westus3.azurecontainer.io:8080")
#endif
        });

        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<IImageClient, ImageClient>();
        builder.Services.AddSingleton<ICustomRequestClient, CustomRequestClient>();
        builder.Services.AddSingleton<IPortfolioClient, PortfolioClient>();
        builder.Services.AddSingleton<IProductClient, ProductClient>();
        builder.Services.AddSingleton<IImageHelpers, ImageHelpers>();
        builder.Services.AddSingleton<ICartStorage, CartStorage>();
        builder.Services.AddSingleton<IOrderClient, OrderClient>();
        builder.Services.AddSingleton<IOrderedProductClient, OrderedProductClient>();
        builder.Services.AddSingleton<ICartStorage, CartStorage>();
        builder.Services.AddSingleton<IUserClient, UserClient>();
        return builder;
    }
}

