namespace ChatApp.BlazorServer.Services.Authentication;

public class LocalStorageJwtHttpClientFactory : IJwtHttpClientFactory
{
    private readonly IJwtPersistService _jwtPersistService;
    private readonly IHttpClientFactory _httpClientFactory;

    public LocalStorageJwtHttpClientFactory(
        IJwtPersistService jwtPersistService,
        IHttpClientFactory httpClientFactory
    )
    {
        _jwtPersistService = jwtPersistService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpClient> CreateJwtClientAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();

        var token = await _jwtPersistService.GetJwtTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }


        return httpClient;
    }
}