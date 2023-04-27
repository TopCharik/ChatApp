using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class MessagesApiProvider : IMessagesApiProvider
{
    private readonly IJwtHttpClient _jwtHttpClient;
    private readonly string? _apiUrl;

    public MessagesApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, HttpClient httpClient)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"];
    }
    
    public async Task<HttpResponseMessage> LoadMessagesAsync(int conversationId)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/Messages/{conversationId}", false);
    }
    
    public async Task<HttpResponseMessage> SendMessageAsync(int conversationId, NewMessageDto newMessageDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Messages/{conversationId}", newMessageDto, false);
    }
}