using System.Security.Claims;
using Blazored.LocalStorage;
using ChatApp.BlazorServer.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChatApp.BlazorServer.Services.Authentication;

public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly IJwtParser _jwtParser;

    public JwtAuthenticationStateProvider(ILocalStorageService localStorage, IJwtParser jwtParser)
    {
        _localStorage = localStorage;
        _jwtParser = jwtParser;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("token");

        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(_jwtParser.ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }
}