using System.ComponentModel;

namespace Oauth2Provider.Entities;

internal enum AuthorizationGrantTypesEnum : byte
{
    [Description("code")]
    Code,

    [Description("Implicit")]
    Implicit,

    [Description("ClientCredentials")]
    ClientCredentials,

    [Description("ResourceOwnerPassword")]
    ResourceOwnerPassword
}