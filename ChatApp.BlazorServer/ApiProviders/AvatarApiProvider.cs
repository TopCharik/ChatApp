using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class AvatarApiProvider : IAvatarApiProvider
{
    private readonly IJwtHttpClient _jwtHttpClient;
    private readonly string? _apiUrl;

    public AvatarApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, HttpClient httpClient)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"];
    }
    
    public async Task<HttpResponseMessage> UploadNewAvatar(NewAvatarDto newAvatarDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/avatar", newAvatarDto);
    }
}