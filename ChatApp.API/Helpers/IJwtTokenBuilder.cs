using ChatApp.DAL.Identity;

namespace ChatApp.API.Helpers;

public interface IJwtTokenBuilder
{
    string CreateToken(ExtendedIdentityUser user);
}