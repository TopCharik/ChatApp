using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.API;

public class JwtConfiguration : IJwtConfiguration
{
    public string Issuer { get; }
    public SigningCredentials SigningCredentials { get; }

    public JwtConfiguration(IConfiguration configuration)
    {
        Issuer = configuration["Token:Issuer"]  ?? throw new ArgumentException("Issuer is required");
        
        var key = configuration["Token:Key"] ?? throw new ArgumentException("Jwt Key is required");
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);
    }



}