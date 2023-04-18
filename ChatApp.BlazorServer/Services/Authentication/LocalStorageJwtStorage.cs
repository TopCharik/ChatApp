using Blazored.LocalStorage;

namespace ChatApp.BlazorServer.Services.Authentication;

public class LocalStorageJwtStorage : IJwtStorage
{
    private const string TOKEN_KEY = "token";
    private readonly ILocalStorageService _localStorageService;

    public LocalStorageJwtStorage(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<string?> GetJwtTokenAsync()
    {
        return await _localStorageService.GetItemAsync<string>(TOKEN_KEY);
    }

    public async Task SetJwtTokenAsync(string token)
    {
        await _localStorageService.SetItemAsync(TOKEN_KEY, token);
    }

    public async Task RemoveJwtTokenAsync()
    {
        await _localStorageService.RemoveItemAsync(TOKEN_KEY);
    }
}