using DerpRaven.Blazor.Components;
using DerpRaven.Shared.ApiClients;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add an http client and set the base address to the API
builder.Services.AddHttpClient<CustomRequestClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseAddress"] ?? "http://localhost:5077");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
