using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatApp.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IJwtConfiguration _jwtConfiguration;

    public AccountController(IJwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    [HttpGet]
    public ActionResult<string> Login()
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "TopCharik"),
            new Claim(JwtRegisteredClaimNames.Email, "test@example.com"),
        };


        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: _jwtConfiguration.SigningCredentials
            );

        var value = new JwtSecurityTokenHandler().WriteToken(token);
        return value;
    }
}