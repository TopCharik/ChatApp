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
        IJwtHelper _jwtHelper, 
        AuthenticationStateProvider authenticationStateProvider
    )
    {
        _jwtStorage = jwtStorage;
        _httpClientFactory = httpClientFactory;
        this._jwtHelper = _jwtHelper;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<HttpClient> CreateJwtClientAsync()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var token = await _jwtStorage.GetJwtTokenAsync();

        
        if (!string.IsNullOrEmpty(token))
        {
            if (!_jwtHelper.IsTokenValid(token))
            {
                await _jwtStorage.RemoveJwtTokenAsync();
                await _authenticationStateProvider.GetAuthenticationStateAsync();
            }
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }


        return httpClient;
    }
}