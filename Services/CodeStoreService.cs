using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Oauth2Provider.Entities;

namespace Oauth2Provider.Services;

public interface ICodeStoreService
{
    string GenerateAuthorizationCode(string clientId, IList<string> requestedScope);
    AuthorizationCode? GetClientDataByCode(string key);
    void RemoveClientDataByCode(string key);

    public AuthorizationCode? UpdatedClientDataByCode(string key, IList<string> requestedScopes,
        string userName, string password = null, string nonce = null);
}
public class CodeStoreService : ICodeStoreService
{
    private readonly ConcurrentDictionary<string, AuthorizationCode> _codeIssued = new ConcurrentDictionary<string, AuthorizationCode>();
    private readonly ClientStore _clientStore = new ClientStore();
    
    public string GenerateAuthorizationCode(string clientId, IList<string> requestedScope)
    {
        var client = _clientStore.Clients.FirstOrDefault(x => x.ClientId == clientId);

        if (client == null) return null;
        var code = Guid.NewGuid().ToString();

        var authCode = new AuthorizationCode
        {
            ClientId = clientId,
            RedirectUri = client.RedirectUri,
            RequestedScopes = requestedScope,
        };

        // then store the code is the Concurrent Dictionary
        _codeIssued[code] = authCode;

        return code;
    }

    public AuthorizationCode? GetClientDataByCode(string key)
    {
        return _codeIssued.TryGetValue(key, out var authorizationCode) ? authorizationCode : null;
    }

    public void RemoveClientDataByCode(string key)
    {
        _codeIssued.TryRemove(key, out _);
    }

    public AuthorizationCode? UpdatedClientDataByCode(string key, IList<string> requestedScopes, string userName, string password = null,
        string nonce = null)
    {
        var oldValue = GetClientDataByCode(key);
        if (oldValue is null) return null;
        
        // check the requested scopes with the one that are stored in the Client Store 
        var client = _clientStore.Clients.FirstOrDefault(x => x.ClientId == oldValue.ClientId);

        if (client is null) return null;
        
        var clientScope = client.AllowedScopes.Where(m => requestedScopes.Contains(m)).ToList();
        if (!clientScope.Any())
            return null;

        var newValue = new AuthorizationCode
        {
            ClientId = oldValue.ClientId,
            CreationTime = oldValue.CreationTime,
            IsOpenId = requestedScopes.Contains("openId") || requestedScopes.Contains("profile"),
            RedirectUri = oldValue.RedirectUri,
            RequestedScopes = requestedScopes,
            Nonce = nonce
        };


        // ------------------ I suppose the user name and password is correct  -----------------
        var claims = new List<Claim>();
                
        if (newValue.IsOpenId)
        {
            // TODO
            // Add more claims to the claims

        }

        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        newValue.Subject = new ClaimsPrincipal(claimIdentity);
        // ------------------ -----------------------------------------------  -----------------

        var result = _codeIssued.TryUpdate(key, newValue, oldValue);

        return result ? newValue : null;
    }
}