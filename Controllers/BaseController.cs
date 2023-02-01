using Microsoft.AspNetCore.Mvc;

namespace Oauth2Provider.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController {
    [HttpGet]
    public IActionResult Index() {
        return new OkObjectResult("Works");
    }
}