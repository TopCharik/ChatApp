using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChatApp.BlazorServer.Services.Authentication;

public class LocalStorageJwtPersistService : IJwtPersistService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public LocalStorageJwtPersistService(ILocalStorageService localStorageService,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _localStorageService = localStorageService;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<string?> GetJwtTokenAsync()
    {
        return await _localStorageService.GetItemAsync<string>("token");
    }

    public async Task SetJwtTokenAsync(string token)
    {
        await _localStorageService.SetItemAsync("token", token);
        await _authenticationStateProvider.GetAuthenticationStateAsync();
    }

    public async Task RemoveJwtTokenAsync()
    {
        await _localStorageService.RemoveItemAsync("token");
        await _authenticationStateProvider.GetAuthenticationStateAsync();
    }
}