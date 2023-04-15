using Microsoft.AspNetCore.Identity;

namespace ChatApp.API;

public interface IJwtTokenService
{
    string CreateToken(IdentityUser user);
}