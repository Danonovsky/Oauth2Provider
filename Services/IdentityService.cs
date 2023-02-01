using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Oauth2Provider.DAL;
using Oauth2Provider.Entities;
using Oauth2Provider.Exceptions;

public interface IIdentityService {
    public Task SignUp(SignUpDto request);
    public Task<JwtDto> SignIn(SignInDto request);
}
class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;
    private readonly IdentityContext _db;

    public IdentityService(IdentityContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<JwtDto> SignIn(SignInDto request)
    {
        if(request is null) throw new ArgumentNullException("SignIn request cannot be null");
        var user = await AuthenticateUser(request);
        var tokenString = GenerateJSONWebToken(user);

        return new JwtDto { Token = tokenString };

    }

    public async Task SignUp(SignUpDto request)
    {
        if(request is null) throw new ArgumentNullException("SignUp request cannot be null");
        await ValidateUser(request);
        var newUser = new User {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Password = request.Password,
            GithubUrl = request.GithubUrl
        };
        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();
    }

    private async Task ValidateUser(SignUpDto user) {
        if(await _db.Users.AnyAsync(_ => _.Email == user.Email)) throw new UserAlreadyExistsException($"User with email '{user.Email}' already exists.");
        if(user.PasswordsMatch is false) throw new PasswordsDoesntMatchException("Passwords doesn't match.");
    }

    private async Task<User> AuthenticateUser(SignInDto request) {
        var user = await _db.Users.Where(_ => _.Email == request.Email && _.Password == request.Password)
        .FirstOrDefaultAsync();
        if(user is null) throw new UserNotFoundException($"User with given credentials was not found.");
        return user;
    }
    
    private string GenerateJSONWebToken(User userInfo)    
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],    
              _configuration["Jwt:Issuer"],    
              null,    
              expires: DateTime.Now.AddMinutes(120),    
              signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);    
        } 
}

public sealed class SignUpDto {
    public string Email { get; set; } = "";
    public string Password {get;set;} = "";
    public string ConfirmPassword {get;set;} = "";
    public string FirstName {get;set;} = "";
    public string LastName {get;set;} = "";
    public string GithubUrl {get;set;} = "";

    public bool PasswordsMatch => Password == ConfirmPassword;
}

public sealed class SignInDto {
    public string Email {get;set;} = "";
    public string Password {get;set;} = "";
}

public sealed class JwtDto {
    public string Token {get;set;} = "";
}