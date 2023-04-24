using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;
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

    public async Task<HttpResponseMessage> LoadUsersAsync(Dictionary<string, string> queryParams)
    {
        var url = QueryHelpers.AddQueryString($"{_apiUrl}/api/User", queryParams);

        return await _jwtHttpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> LoadSingleUserAsync(string username)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/User/{username}");
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(UpdateUserDto updateUserDto, string username)
    {
        var response = await _jwtHttpClient.PutAsJsonAsync(
            $"{_apiUrl}/api/Account/update-user/{username}",
            updateUserDto, false
        );
        return response;
    }

    public async Task<HttpResponseMessage> ChangeUsernameAsync(string oldUsername, string newUsername)
    {
        var response = await _jwtHttpClient.PutAsJsonAsync(
            $"{_apiUrl}/api/Account/change-username/{oldUsername}",
            newUsername,
            false
        );
        return response;
    }
}