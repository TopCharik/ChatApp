using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IUsersApiProvider
{
    public Task<HttpResponseMessage> LoadUsersAsync(Dictionary<string, string> queryParams);
    public Task<HttpResponseMessage> LoadSingleUserAsync(string username);
    public Task<HttpResponseMessage> UpdateUserAsync(EditUserDto editUserDto, string username);
    public Task<HttpResponseMessage> ChangeUsernameAsync(string oldUsername, NewUsernameDto newUsername);
    public Task<HttpResponseMessage> StartCall(CallUsernamesDto callUsernamesDto);
    public Task<HttpResponseMessage> UploadNewAvatar(NewUserAvatarDto newAvatarDto);
}