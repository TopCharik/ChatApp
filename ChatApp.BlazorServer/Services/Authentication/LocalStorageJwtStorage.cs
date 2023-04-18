using Blazored.LocalStorage;

namespace ChatApp.BlazorServer.Services.Authentication;

public class LocalStorageJwtStorage : IJwtStorage
{
    private readonly ILocalStorageService _localStorageService;

    public LocalStorageJwtStorage(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<string?> GetJwtTokenAsync()
    {
        return await _localStorageService.GetItemAsync<string>("token");
    }

    public async Task SetJwtTokenAsync(string token)
    {
        await _localStorageService.SetItemAsync("token", token);
    }

    public async Task RemoveJwtTokenAsync()
    {
        await _localStorageService.RemoveItemAsync("token");
    }
}