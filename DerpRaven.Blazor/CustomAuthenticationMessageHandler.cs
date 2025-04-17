using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
namespace DerpRaven.Blazor;

public class CustomAuthenticationMessageHandler : AuthorizationMessageHandler
{
    public CustomAuthenticationMessageHandler(IAccessTokenProvider provder, NavigationManager nav) : base(provder, nav)
    {
        ConfigureHandler(["http://localhost:5077"]);
    }
}