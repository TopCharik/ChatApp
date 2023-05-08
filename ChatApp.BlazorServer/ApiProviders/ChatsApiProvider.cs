using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;
using Microsoft.AspNetCore.WebUtilities;

namespace ChatApp.BlazorServer.ApiProviders;

public class ChatsApiProvider : BaseApiProvider, IChatsApiProvider
{
    public ChatsApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config)
        : base(jwtHttpClient, config) { }
    
    public async Task<HttpResponseMessage> LoadChatsAsync(Dictionary<string, string> queryParams)
    {
        var url = QueryHelpers.AddQueryString($"{_apiUrl}/api/Chats", queryParams);

        return await _jwtHttpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> CreateNewChatAsync(NewChatDto newChatDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Chats", newChatDto);
    }
    
        
    public async Task<HttpResponseMessage> UploadNewAvatar(NewChatAvatarDto newAvatarDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Chats/avatar", newAvatarDto);
    }
}