using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Oauth2Provider.Pages.Auth;

public class SignUp : PageModel
{
    private readonly IIdentityService _identity;

    public SignUp(IIdentityService identity)
    {
        _identity = identity;
    }

    [BindProperty] public string Email { get; set; }
    [BindProperty] public string Password { get; set; }
    [BindProperty] public string ConfirmPassword { get; set; }
    [BindProperty] public string FirstName { get; set; }
    [BindProperty] public string LastName { get; set; }
    [BindProperty] public string GithubUrl { get; set; }
    
    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        await _identity.SignUp(new SignUpDto
        {
            LastName = LastName,
            Password = Password,
            ConfirmPassword = ConfirmPassword,
            FirstName = FirstName,
            GithubUrl = GithubUrl,
            Email = Email
        });
        return RedirectToPage("/sign-in");
    }
}