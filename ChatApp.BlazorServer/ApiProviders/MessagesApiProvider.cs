using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class MessagesApiProvider : BaseApiProvider, IMessagesApiProvider
{
    public MessagesApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config)
        : base(jwtHttpClient, config) { }
    
    public async Task<HttpResponseMessage> LoadMessagesAsync(int conversationId)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/Messages/{conversationId}", false);
    }

    public async Task<HttpResponseMessage> LoadMessagesAfterAsync(int conversationId, DateTime after)
    {
        var queryParams = $"After={after:yyyy-MM-ddTHH:mm:ss.ffff}";
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/Messages/{conversationId}?{queryParams}", false);
    }

    public async Task<HttpResponseMessage> SendMessageAsync(int conversationId, NewMessageDto newMessageDto)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Messages/{conversationId}", newMessageDto, false);
    }
}