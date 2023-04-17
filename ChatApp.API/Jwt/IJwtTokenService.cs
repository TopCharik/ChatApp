using ChatApp.DAL.Identity;

namespace ChatApp.API.Jwt;

public interface IJwtTokenService
{
    string CreateToken(ExtendedIdentityUser user);
}