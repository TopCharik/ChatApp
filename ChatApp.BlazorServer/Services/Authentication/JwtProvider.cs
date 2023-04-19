using Microsoft.AspNetCore.Components.Authorization;

namespace ChatApp.BlazorServer.Services.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly IJwtStorage _jwtStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public JwtProvider(IJwtStorage jwtStorage, AuthenticationStateProvider authenticationStateProvider)
    {
        _jwtStorage = jwtStorage;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _jwtStorage.GetJwtTokenAsync();
    }

    public async Task SetTokenAsync(string token)
    {
        await _jwtStorage.SaveJwtTokenAsync(token);
        await _authenticationStateProvider.GetAuthenticationStateAsync();
    }

    public async Task DeleteTokenAsync()
    {
        await _jwtStorage.RemoveJwtTokenAsync();
        await _authenticationStateProvider.GetAuthenticationStateAsync();
    }
}