using System.Security.Claims;

namespace ChatApp.BlazorServer.Helpers;

public interface IJwtHelper
{
    IEnumerable<Claim> ParseClaimsFromJwt(string jwt);
    bool IsTokenValid(string token);
    bool IsTokenValid(IEnumerable<Claim> identity);
}