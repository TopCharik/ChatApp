using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecretController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetSecret()
    {
        return Ok("Secret message");
    }
}