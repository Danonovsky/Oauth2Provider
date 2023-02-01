namespace Oauth2Provider.Entities;

sealed class User : BaseEntity
{
    public string Email {get;set;} = "";
    public string Password {get;set;} = "";
    public string FirstName {get;set;} = "";
    public string LastName {get;set;} = "";
    public string GithubUrl {get;set;} = "";
}