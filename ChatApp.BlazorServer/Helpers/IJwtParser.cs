using System.Security.Claims;

namespace ChatApp.BlazorServer.Helpers;

public interface IJwtParser
{
    IEnumerable<Claim> ParseClaimsFromJwt(string jwt);
}