using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class UsersApiProvider : IUsersApiProvider
{
    private readonly IJwtHttpClient _jwtHttpClient;
    private readonly HttpClient _httpClient;
    private readonly string? _apiUrl;

    public UsersApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, HttpClient httpClient)
    {
        _jwtHttpClient = jwtHttpClient;
        _httpClient = httpClient;
        _apiUrl = config["ApiUrl"];
    }

    public async Task<HttpResponseMessage> LoadUsers()
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/User");
    }
}