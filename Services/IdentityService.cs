using Microsoft.EntityFrameworkCore;

interface IIdentityService {
    public Task SignUp(SignUp request);
    public Task<JwtDto> SignIn(SignIn request);
}
class IdentityService : IIdentityService
{
    private readonly IdentityContext _db;

    public IdentityService(IdentityContext db)
    {
        _db = db;
    }

    public async Task<JwtDto> SignIn(SignIn request)
    {
        if(request is null) throw new ArgumentNullException("SignIn request cannot be null");
        throw new NotImplementedException();

    }

    public async Task SignUp(SignUp request)
    {
        if(request is null) throw new ArgumentNullException("SignUp request cannot be null");
        await ValidateUser(request);
        await _db.Users.AddAsync(new User {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            GithubUrl = request.GithubUrl
        });
    }

    private async Task ValidateUser(SignUp user) {
        if(await _db.Users.AnyAsync(_ => _.Email == user.Email)) throw new UserAlreadyExistsException($"User with email '{user.Email}' already exists.");
        if(user.PasswordsMatch is false) throw new PasswordsDoesntMatchException("Passwords doesn't match.");
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

sealed class JwtDto {
    public string Token {get;set;} = "";
}