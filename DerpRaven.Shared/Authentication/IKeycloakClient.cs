using Duende.IdentityModel.OidcClient;

namespace DerpRaven.Shared.Authentication
{
    public interface IKeycloakClient
    {
        OktaClientConfiguration Configuration { get; }
        string? IdentityToken { get; }

        event Action? IdentityTokenChanged;

        Task<LoginResult> LoginAsync(CancellationToken cancellationToken = default);
        Task<LogoutResult> LogoutAsync(string idToken, CancellationToken cancellationToken = default);
    }
}