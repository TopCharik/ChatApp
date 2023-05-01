using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IAvatarApiProvider
{
    public Task<HttpResponseMessage> UploadNewAvatar(NewAvatarDto newAvatarDto);
}