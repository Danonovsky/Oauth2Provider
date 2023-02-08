namespace Oauth2Provider.Entities;

public class Client
{
    public string ClientName { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public IList<string> GrantType { get; set; }
    public bool IsActive { get; set; } = false;
    public IList<string> AllowedScopes { get; set; }
    public string ClientUri { get; set; }
    public string RedirectUri { get; set; }
}