using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;
using Microsoft.AspNetCore.WebUtilities;

namespace ChatApp.BlazorServer.ApiProviders;

public class ChatsApiProvider : IChatsApiProvider
{
    private readonly IJwtHttpClient _jwtHttpClient;
    private readonly string? _apiUrl;

    public ChatsApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, HttpClient httpClient)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"];
    }

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