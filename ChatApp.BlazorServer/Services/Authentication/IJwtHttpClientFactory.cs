namespace ChatApp.BlazorServer.Services.Authentication;

public interface IJwtHttpClientFactory
{
    public Task<HttpClient> CreateJwtClientAsync();
}