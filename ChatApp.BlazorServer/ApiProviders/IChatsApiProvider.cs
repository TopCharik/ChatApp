using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IChatsApiProvider
{
    Task<HttpResponseMessage> LoadChatsAsync(Dictionary<string, string> queryParams);
    Task<HttpResponseMessage> CreateNewChatAsync(NewChatDto newChatDto);
    public Task<HttpResponseMessage> UploadNewAvatar(NewChatAvatarDto newAvatarDto);
}