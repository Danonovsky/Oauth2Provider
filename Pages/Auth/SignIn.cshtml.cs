using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Oauth2Provider.Pages.Auth;

public class SignIn : PageModel
{
    private readonly IIdentityService _identity;

    public SignIn(IIdentityService identity)
    {
        _identity = identity;
    }

    [BindProperty] public string Email { get; set; }
    [BindProperty] public string Password { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        var jwt = await _identity.SignIn(new SignInDto
        {
            Email = Email,
            Password = Password
        });
        if (jwt?.Token is null) return RedirectToPage("/sign-in");
        HttpContext.Session.SetString("Token",jwt.Token);
        return LocalRedirect("/");
    }
}

