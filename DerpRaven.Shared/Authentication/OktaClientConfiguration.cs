namespace DerpRaven.Shared.Authentication;
using IBrowser = Duende.IdentityModel.OidcClient.Browser.IBrowser;

public class OktaClientConfiguration
{
    public string ClientId { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
    public string PostLogoutRedirectUri { get; set; } = string.Empty;
    public IList<string> Scope { get; set; } = new string[] { "openid", "profile" };
    public string Domain { get; set; } = string.Empty;
    public IBrowser Browser { get; set; } = default!;
}
