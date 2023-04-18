using System.Security.Claims;
using System.Text.Json;

namespace ChatApp.BlazorServer.Helpers;

public class JwtHelper : IJwtHelper
{
    public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrEmpty(jwt)) return new List<Claim>();
        
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return (keyValuePairs ?? new Dictionary<string, object>())
            .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));
    }
    
    public bool IsTokenValid(string token)
    {
        var claims = ParseClaimsFromJwt(token);
        return IsTokenValid(claims);
    }
    
    public bool IsTokenValid(IEnumerable<Claim> claims)
    {
        var expires = long.Parse(claims.SingleOrDefault(claim => claim.Type == "exp")?.Value ?? "0");
        return expires != 0 && DateTimeOffset.FromUnixTimeSeconds(expires) > DateTimeOffset.Now;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
}