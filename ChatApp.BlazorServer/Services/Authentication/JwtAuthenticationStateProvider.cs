using System.Security.Claims;
using ChatApp.BlazorServer.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChatApp.BlazorServer.Services.Authentication;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJwtStorage _jwtStorage;
    private readonly IJwtHelper _jwtHelper;

    public JwtAuthenticationStateProvider( IJwtHelper jwtHelper, IJwtStorage jwtStorage)
    {
        _jwtHelper = jwtHelper;
        _jwtStorage = jwtStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _jwtStorage.GetJwtTokenAsync();
        var claims = _jwtHelper.ParseClaimsFromJwt(token);

        var identity = _jwtHelper.IsTokenValid(claims)
            ? new ClaimsIdentity(claims, "jwt") 
            : new ClaimsIdentity();
            

        
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }
}