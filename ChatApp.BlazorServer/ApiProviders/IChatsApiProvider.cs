using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IChatsApiProvider
{
    public Task<HttpResponseMessage> LoadChatsAsync(Dictionary<string, string> queryParams);
    public Task<HttpResponseMessage> CreateNewChatAsync(NewChatDto newChatDto);
}