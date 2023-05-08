using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;
using Microsoft.AspNetCore.WebUtilities;

namespace ChatApp.BlazorServer.ApiProviders;

public class ChatsApiProvider : BaseApiProvider, IChatsApiProvider
{
    public ChatsApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, string apiUrl)
        : base(jwtHttpClient, config, apiUrl) { }
    
    public async Task<HttpResponseMessage> LoadChatsAsync(Dictionary<string, string> queryParams)
    {
        var url = QueryHelpers.AddQueryString($"{_apiUrl}/api/Chats", queryParams);

        return await _jwtHttpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> CreateNewChatAsync(NewChatDto newChatDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Chats", newChatDto);
    }
}