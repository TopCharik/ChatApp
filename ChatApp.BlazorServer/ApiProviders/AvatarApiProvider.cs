using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class AvatarApiProvider : BaseApiProvider, IAvatarApiProvider
{
    public AvatarApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, string apiUrl)
        : base(jwtHttpClient, config, apiUrl) { }
    
    public async Task<HttpResponseMessage> UploadNewAvatar(NewAvatarDto newAvatarDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/avatar", newAvatarDto);
    }
}