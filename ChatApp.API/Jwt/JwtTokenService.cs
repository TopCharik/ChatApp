using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChatApp.Core.Entities.Identity;

namespace ChatApp.API;

public class JwtTokenService : IJwtTokenService
{
    private readonly IJwtConfiguration _jwtConfiguration;

    public JwtTokenService(IJwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim("UserName", user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };


        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: _jwtConfiguration.SigningCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}