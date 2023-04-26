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

    public async Task<HttpResponseMessage> GetChatParticipation(string chatLink)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/Chats/Participation/{chatLink}");
    }

    public async Task<HttpResponseMessage> JoinChat(string chatLink)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Chats/{chatLink}/Join", new {});
    }
    
    public async Task<HttpResponseMessage> LeaveChat(string chatLink)
    {
        return await _jwtHttpClient.DeleteAsync($"{_apiUrl}/api/Chats/Participation/{chatLink}");
    }
}