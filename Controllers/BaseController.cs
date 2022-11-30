using Microsoft.AspNetCore.Mvc;
[Route("api/[controller]")]
[ApiController]
public abstract class BaseController {
    [HttpGet]
    public IActionResult Index() {
        return new OkObjectResult("Works");
    }
}