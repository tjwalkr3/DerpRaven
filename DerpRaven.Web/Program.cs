using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["BaseAddress"] ?? "http://localhost:8080")
});

// Comment these out 
builder.Services.AddScoped<IKeycloakClient, KeycloakClient>(k =>
{
    var oktaClientConfiguration = new OktaClientConfiguration()
    {
        //Verify by adding .well-known/openid-configuration to the URL
        Domain = "https://engineering.snow.edu/auth/realms/SnowCollege/",
        ClientId = "DerpRavenBlazorAuth",
        RedirectUri = "myapp://callback",
        Browser = new WebBrowserAuthenticator()
    };
    return new KeycloakClient(oktaClientConfiguration);
});
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<ICustomRequestClient, CustomRequestClient>();
builder.Services.AddScoped<IImageClient, ImageClient>();

await builder.Build().RunAsync();
