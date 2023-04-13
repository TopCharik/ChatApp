using Microsoft.IdentityModel.Tokens;

namespace ChatApp.API;

public interface IJwtConfiguration
{
    public string Issuer { get; }
    public SigningCredentials SigningCredentials { get; }
}