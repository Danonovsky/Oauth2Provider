using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Oauth2Provider.Pages.Auth;

public class SignOut : PageModel
{
    public IActionResult OnGet()
    {
        HttpContext.Session.Remove("Token");
        return LocalRedirect("/");
    }
}