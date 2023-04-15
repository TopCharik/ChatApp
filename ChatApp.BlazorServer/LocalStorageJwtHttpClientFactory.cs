using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChatApp.BlazorServer;

public class LocalStorageJwtHttpClientFactory : IJwtHttpClientFactory
{
    private readonly ProtectedLocalStorage _localStorage;
    private readonly IHttpClientFactory _httpClientFactory;

    public LocalStorageJwtHttpClientFactory(
        ProtectedLocalStorage localStorage,
        IHttpClientFactory httpClientFactory
    )
    {
        _localStorage = localStorage;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpClient> CreateJwtClientAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        try
        {
            var result = await _localStorage.GetAsync<string>("token");
            if (result.Success)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {result.Value}");
            }
        }
        catch (InvalidOperationException e) { }

        
        return httpClient;
    }
}