using ChatApp.DAL.Identity;

namespace ChatApp.API;

public interface IJwtTokenService
{
    string CreateToken(ExtendedIdentityUser user);
}