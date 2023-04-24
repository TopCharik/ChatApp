using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IUsersApiProvider
{
    public Task<HttpResponseMessage> LoadUsersAsync(Dictionary<string, string> queryParams);
    public Task<HttpResponseMessage> LoadSingleUserAsync(string username);
    public Task<HttpResponseMessage> UpdateUserAsync(UpdateUserDto updateUserDto, string username);
}