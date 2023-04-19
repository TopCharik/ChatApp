namespace ChatApp.BlazorServer.Services.Authentication;

public interface IJwtProvider
{
    Task<string> GetTokenAsync();
    Task SetTokenAsync(string token);
    Task DeleteTokenAsync();
}