using ChatApp.BlazorServer.Services.Authentication;

namespace ChatApp.BlazorServer.ApiProviders;

public class ParticipationApiProvider : IParticipationApiProvider
{
    private readonly IJwtHttpClient _jwtHttpClient;
    private readonly string? _apiUrl;

    public ParticipationApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, HttpClient httpClient)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"];
    }
    
    public async Task<HttpResponseMessage> GetChatParticipation(string chatLink)
    {
        return await _jwtHttpClient.GetAsync($"{_apiUrl}/api/Participation/{chatLink}");
    }

    public async Task<HttpResponseMessage> JoinChat(string chatLink)
    {
        return await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Participation/{chatLink}", new {});
    }
    
    public async Task<HttpResponseMessage> LeaveChat(string chatLink)
    {
        return await _jwtHttpClient.DeleteAsync($"{_apiUrl}/api/Participation/{chatLink}");
    }
}