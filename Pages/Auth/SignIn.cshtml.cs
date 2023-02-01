using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Oauth2Provider.Pages.Auth;

public class SignInModel : PageModel
{
    public string Email { get; set; }
    public string Password { get; set; }

    public void OnGet()
    {
    }
}

