using Blazored.LocalStorage;

namespace ChatApp.BlazorServer;

public class LocalStorageJwtHttpClientFactory : IJwtHttpClientFactory
{
    private readonly ILocalStorageService _localStorage;
    private readonly IHttpClientFactory _httpClientFactory;

    public LocalStorageJwtHttpClientFactory(
        ILocalStorageService localStorage,
        IHttpClientFactory httpClientFactory
    )
    {
        _localStorage = localStorage;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpClient> CreateJwtClientAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var token = await _localStorage.GetItemAsync<string>("token");
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }


        return httpClient;
    }
}