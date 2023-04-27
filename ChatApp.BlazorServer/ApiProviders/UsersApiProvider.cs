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
        var url = QueryHelpers.AddQueryString($"{_apiUrl}/api/Users", queryParams);

        return await _jwtHttpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> LoadSingleUserAsync(string username)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/Users/{username}");
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(UpdateUserDto updateUserDto, string username)
    {
        var response = await _jwtHttpClient.PatchAsJsonAsync(
            $"{_apiUrl}/api/Users/update-user/{username}",
            updateUserDto, false
        );
        return response;
    }

    public async Task<HttpResponseMessage> ChangeUsernameAsync(string oldUsername, string newUsername)
    {
        var response = await _jwtHttpClient.PatchAsJsonAsync(
            $"{_apiUrl}/api/Users/change-username/{oldUsername}",
            newUsername,
            false
        );
        return response;
    }
}