using ChatApp.BlazorServer.Services.Authentication;
using Microsoft.AspNetCore.WebUtilities;

namespace ChatApp.BlazorServer.ApiProviders;

public class UsersApiProvider : IUsersApiProvider
{
    private readonly IJwtHttpClient _jwtHttpClient;
    private readonly string? _apiUrl;

    public UsersApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, HttpClient httpClient)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"];
    }

    public async Task<HttpResponseMessage> LoadUsers(Dictionary<string, string> queryParams)
    {
        var url = QueryHelpers.AddQueryString($"{_apiUrl}/api/User", queryParams);

        return await _jwtHttpClient.GetAsync(url);
    }
    
    public async Task<HttpResponseMessage> LoadSingleUser(string username)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/User/{username}");
    }
}