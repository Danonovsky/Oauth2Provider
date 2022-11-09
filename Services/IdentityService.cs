using Microsoft.EntityFrameworkCore;

interface IIdentityService {
    public Task SignUp(SignUp request);
    public Task SignIn(SignIn request);
}
class IdentityService : IIdentityService
{
    private readonly IdentityContext _db;

    public IdentityService(IdentityContext db)
    {
        _db = db;
    }

    public Task SignIn(SignIn request)
    {
        throw new NotImplementedException();
    }

    private async Task ValidateUser(SignUp user) {
        if(await _db.Users.AnyAsync(_ => _.Email == user.Email)) throw new UserAlreadyExistsException($"User with email '{user.Email}' already exists.");
        if(user.PasswordsMatch is false) throw new PasswordsDoesntMatchException("Passwords doesn't match.");
    }

    public Task SignUp(SignUp request)
    {
        throw new NotImplementedException();
    }
}

sealed class SignUp {
    public string Email { get; set; } = "";
    public string Password {get;set;} = "";
    public string ConfirmPassword {get;set;} = "";
    public string FirstName {get;set;} = "";
    public string LastName {get;set;} = "";
    public string GithubUrl {get;set;} = "";

    public bool PasswordsMatch => Password == ConfirmPassword;
}

sealed class SignIn {
    public string Email {get;set;} = "";
    public string Password {get;set;} = "";
}