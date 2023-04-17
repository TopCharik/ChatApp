using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecretController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public ActionResult GetSecret()
    {
        return Ok($"Secret message for {HttpContext.User.Identity.Name} from api");
    }
}