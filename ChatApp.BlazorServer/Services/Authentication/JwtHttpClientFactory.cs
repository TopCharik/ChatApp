using ChatApp.BlazorServer.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChatApp.BlazorServer.Services.Authentication;

public class JwtHttpClientFactory : IJwtHttpClientFactory
{
    private readonly IJwtStorage _jwtStorage;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJwtHelper _jwtHelper;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public JwtHttpClientFactory(
        IJwtStorage jwtStorage,
        IHttpClientFactory httpClientFactory,
        IJwtHelper jwtHelper,
        AuthenticationStateProvider authenticationStateProvider
    )
    {
        _jwtStorage = jwtStorage;
        _httpClientFactory = httpClientFactory;
        _jwtHelper = jwtHelper;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<HttpClient> CreateJwtClientAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var token = await _jwtStorage.GetJwtTokenAsync();

        if (!_jwtHelper.IsTokenValid(token))
        {
            await _jwtStorage.RemoveJwtTokenAsync();
            await _authenticationStateProvider.GetAuthenticationStateAsync();
        }

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");


        return httpClient;
    }
}