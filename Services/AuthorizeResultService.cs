using Oauth2Provider.Entities;
using Oauth2Provider.Extensions;
using Oauth2Provider.OauthRequest;
using Oauth2Provider.OauthResponse;

namespace Oauth2Provider.Services;
public interface IAuthorizeResultService
{
    AuthorizeResponse AuthorizeRequest(IHttpContextAccessor httpContextAccessor, AuthorizationRequest authorizationRequest);
}

public class AuthorizeResultService : IAuthorizeResultService
{
    private static string keyAlg = "66007d41-6924-49f2-ac0c-e63c4b1a1730";
    private readonly ClientStore _clientStore = new ClientStore();
    private readonly ICodeStoreService _codeStoreService;

    public AuthorizeResultService(ICodeStoreService codeStoreService)
    {
        _codeStoreService = codeStoreService;
    }

    public AuthorizeResponse AuthorizeRequest(IHttpContextAccessor httpContextAccessor, AuthorizationRequest authorizationRequest)
    {
        var response = new AuthorizeResponse();

        if (httpContextAccessor == null)
            return new AuthorizeResponse {Error = ErrorTypeEnum.ServerError.GetEnumDescription()};

        var client = VerifyClientById(authorizationRequest.client_id);
        if (!client.IsSuccess)
            return new AuthorizeResponse {Error = client.ErrorDescription};

        if (string.IsNullOrEmpty(authorizationRequest.response_type) || authorizationRequest.response_type != "code")
        {
            response.Error = ErrorTypeEnum.InvalidRequest.GetEnumDescription();
            response.ErrorDescription = "response_type is required or is not valid";
            return response;
        }

        if (!authorizationRequest.redirect_uri.IsRedirectUriStartWithHttps() && !httpContextAccessor.HttpContext.Request.IsHttps)
        {
            response.Error = ErrorTypeEnum.InvalidRequest.GetEnumDescription();
            response.ErrorDescription = "redirect_url is not secure, MUST be TLS";
            return response;
        }
        // check the return url is match the one that in the client store


        // check the scope in the client store with the
        // one that is comming from the request MUST be matched at leaset one

        var scopes = authorizationRequest.scope.Split(' ');
        var clientScopes = client.Client.AllowedScopes.Where(m => scopes.Contains(m)).ToList();

        if (!clientScopes.Any())
        {
            response.Error = ErrorTypeEnum.InValidScope.GetEnumDescription();
            response.ErrorDescription = "scopes are invalids";
            return response;
        }

        var nonce = httpContextAccessor.HttpContext.Request.Query["nonce"].ToString();

        // Verify that a scope parameter is present and contains the openid scope value.
        // (If no openid scope value is present,
        // the request may still be a valid OAuth 2.0 request, but is not an OpenID Connect request.)
        var code = _codeStoreService.GenerateAuthorizationCode(authorizationRequest.client_id, clientScopes.ToList());
        if (code == null)
        {
            response.Error = ErrorTypeEnum.TemporarilyUnAvailable.GetEnumDescription();
            return response;
        }

        response.RedirectUri = client.Client.RedirectUri + "?response_type=code" + "&state=" + authorizationRequest.state;
        response.Code = code;
        response.State = authorizationRequest.state;
        response.RequestedScopes = clientScopes.ToList();
        response.Nonce = nonce;

        return response;
    }
    
    private CheckClientResult VerifyClientById(string clientId, bool checkWithSecret = false, string clientSecret = null)
    {
        var result = new CheckClientResult { IsSuccess = false };

        if (string.IsNullOrWhiteSpace(clientId))
        {
            result.ErrorDescription = ErrorTypeEnum.AccessDenied.GetEnumDescription();
            return result;
        }
        var client = _clientStore.Clients.FirstOrDefault(x => x.ClientId.Equals(clientId, StringComparison.OrdinalIgnoreCase));
        if (client is null) return result;
        
        if (checkWithSecret && !string.IsNullOrEmpty(clientSecret))
        {
            var hasSameSecretId = client.ClientSecret.Equals(clientSecret, StringComparison.InvariantCulture);
            if (!hasSameSecretId)
                return new CheckClientResult { IsSuccess = false, Error = ErrorTypeEnum.InvalidClient.GetEnumDescription() };
        }

        return client.IsActive ? 
            new CheckClientResult { IsSuccess = true, Client = client } : 
            new CheckClientResult { IsSuccess = false, ErrorDescription = ErrorTypeEnum.UnAuthoriazedClient.GetEnumDescription() };
    }

}