using ChatApp.Core.Entities.Identity;

namespace ChatApp.API;

public interface IJwtTokenService
{
    string CreateToken(AppUser user);
}