using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.API;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _key;
    private readonly string _issuer;

    public JwtTokenService(IConfiguration configuration)
    {
        _issuer = configuration["Token:Issuer"]  ?? throw new ArgumentException("Jwt Issuer is required");
        _key = configuration["Token:Key"] ?? throw new ArgumentException("Jwt Key is required");
    }

    public string CreateToken(IdentityUser user)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);
        
        var claims = new List<Claim>
        {
            new Claim("username", user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };


        var token = new JwtSecurityToken(
            issuer: _issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}