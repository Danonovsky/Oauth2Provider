using Oauth2Provider.Extensions;
using Oauth2Provider.OauthRequest;

namespace Oauth2Provider.OauthResponse;

public class TokenResponse
{
    public string access_token { get; set; }
    public string id_token { get; set; }
    public string token_type { get; set; } = TokenTypeEnum.Bearer.GetEnumDescription();
    public string code { get; set; }
    
    public string Error { get; set; } = string.Empty;
    public string ErrorUri { get; set; }
    public string ErrorDescription { get; set; }
    public bool HasError => !string.IsNullOrEmpty(Error);
}