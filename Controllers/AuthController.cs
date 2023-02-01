using Microsoft.AspNetCore.Mvc;

namespace Oauth2Provider.Controllers;

public class AuthController : BaseController {
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("sign-in")]
    public async Task<JwtDto> SignIn(SignIn request) {
        var result = await _identityService.SignIn(request);
        return result;
    }

    [HttpPost("sign-up")]
    public async Task SignUp(SignUp request) {
        await _identityService.SignUp(request);
    }
}