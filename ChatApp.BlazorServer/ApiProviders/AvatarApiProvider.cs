using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class AvatarApiProvider : BaseApiProvider, IAvatarApiProvider
{
    public AvatarApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config)
        : base(jwtHttpClient, config) { }
    
    public async Task<HttpResponseMessage> UploadNewAvatar(NewAvatarDto newAvatarDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/avatar", newAvatarDto);
    }
}