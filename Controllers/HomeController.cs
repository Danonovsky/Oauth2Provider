using Microsoft.AspNetCore.Mvc;
using Oauth2Provider.OauthRequest;
using Oauth2Provider.Services;

namespace Oauth2Provider.Controllers;

public class HomeController : Controller
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IAuthorizeResultService _authorizeResultService;
    private readonly ICodeStoreService _codeStoreService;


    public HomeController(IHttpContextAccessor accessor, IAuthorizeResultService authorizeResultService, ICodeStoreService codeStoreService)
    {
        _accessor = accessor;
        _authorizeResultService = authorizeResultService;
        _codeStoreService = codeStoreService;
    }

    public IActionResult Authorize(AuthorizationRequest request)
    {
        var result = _authorizeResultService.AuthorizeRequest(_accessor, request);

        if (result.HasError)
            return RedirectToAction("Error", new { error = result.Error });

        var loginModel = new OpenIdConnectLoginRequest
        {
            RedirectUri = result.RedirectUri,
            Code = result.Code,
            RequestedScopes = result.RequestedScopes,
            Nonce = result.Nonce
        };


        return View("Login", loginModel);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(OpenIdConnectLoginRequest loginRequest)
    {
        // here I have to check if the username and passowrd is correct
        // and I will show you how to integrate the ASP.NET Core Identity
        // With our framework

        var result = _codeStoreService.UpdatedClientDataByCode(loginRequest.Code, loginRequest.RequestedScopes,
            loginRequest.UserName, nonce: loginRequest.Nonce);
        if (result == null) return RedirectToAction("Error", new {error = "invalid_request"});
        loginRequest.RedirectUri = loginRequest.RedirectUri + "&code=" + loginRequest.Code;
        return Redirect(loginRequest.RedirectUri);
    }

    public IActionResult Error(string error)
    {
        return View(error);
    }
}