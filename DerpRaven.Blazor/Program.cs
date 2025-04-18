using DerpRaven.Blazor;
using DerpRaven.Blazor.ApiClients;
using DerpRaven.Shared.ApiClients;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CustomAuthenticationMessageHandler>();

//Uri baseAddress = new Uri(builder.Configuration.GetValue<string>("BaseAddress") ?? "http://localhost:5077");
string baseAddress = Environment.GetEnvironmentVariable("BaseAddress") ?? "http://localhost:5077";

builder.Services.AddHttpClient("testClient", opt => opt.BaseAddress = new Uri(baseAddress))
                .AddHttpMessageHandler<CustomAuthenticationMessageHandler>();


builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("testClient"));

builder.Services.AddOidcAuthentication(opt =>
{
    opt.ProviderOptions.Authority = "https://engineering.snow.edu/auth/realms/SnowCollege/";
    opt.ProviderOptions.ClientId = "DerpClientSpring25";
    opt.ProviderOptions.ResponseType = "code";
    opt.ProviderOptions.DefaultScopes.Add("openid");
    opt.ProviderOptions.DefaultScopes.Add("profile");
});

builder.Services.AddScoped<BlazorImageClient>();
builder.Services.AddScoped<ICustomRequestClient, BlazorCustomRequestClient>();

await builder.Build().RunAsync();
