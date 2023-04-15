namespace ChatApp.BlazorServer;

public interface IJwtHttpClientFactory
{
    public Task<HttpClient> CreateJwtClientAsync();
}