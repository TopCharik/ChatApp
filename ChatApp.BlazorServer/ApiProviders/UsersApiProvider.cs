using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;
using Microsoft.AspNetCore.WebUtilities;

namespace ChatApp.BlazorServer.ApiProviders;

public class UsersApiProvider : BaseApiProvider, IUsersApiProvider
{
    public UsersApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config)
        : base(jwtHttpClient, config) { }

    public async Task<HttpResponseMessage> LoadUsersAsync(Dictionary<string, string> queryParams)
    {
        var url = QueryHelpers.AddQueryString($"{_apiUrl}/api/Users", queryParams);

        return await _jwtHttpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> LoadSingleUserAsync(string username)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/Users/{username}");
    }

    public async Task<HttpResponseMessage> UpdateUserAsync(EditUserDto editUserDto, string username)
    {
        var response = await _jwtHttpClient.PatchAsJsonAsync(
            $"{_apiUrl}/api/Users/update-user/{username}",
            editUserDto
        );
        return response;
    }

    public async Task<HttpResponseMessage> ChangeUsernameAsync(string oldUsername, NewUsernameDto newUsername)
    {
        var response = await _jwtHttpClient.PatchAsJsonAsync(
            $"{_apiUrl}/api/Users/change-username/{oldUsername}",
            newUsername
        );
        return response;
    }

    public async Task<HttpResponseMessage> StartCall(CallUsernamesDto callUsernamesDto)
    {
        var response = await _jwtHttpClient.PostAsJsonAsync(
            $"{_apiUrl}/api/Users/call",
            callUsernamesDto
        );
        return response;
    }
}