﻿using System.ComponentModel;

namespace Oauth2Provider.OauthRequest;

public class TokenRequest
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Code { get; set; }
    public string GrantType { get; set; }
    public string RedirectUri { get; set; }
    public string CodeVerifier { get; set; }
}

public enum TokenTypeEnum : byte
{
    [Description("Bearer")] Bearer
}