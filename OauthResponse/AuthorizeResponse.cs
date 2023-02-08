namespace Oauth2Provider.OauthResponse;

public class AuthorizeResponse
{
    public string ResponseType { get; set; } 
    public string Code { get; set; }
    public string State { get; set; }

    public string RedirectUri { get; set; }
    public IList<string> RequestedScopes { get; set; }
    public string GrantType { get; set; }
    public string Nonce { get; set; }
    public string Error { get; set; } = string.Empty;
    public string ErrorUri { get; set; }
    public string ErrorDescription { get; set; }
    public bool HasError => !string.IsNullOrEmpty(Error);
}