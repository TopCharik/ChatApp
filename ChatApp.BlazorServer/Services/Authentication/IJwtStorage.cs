namespace ChatApp.BlazorServer.Services.Authentication;

public interface IJwtStorage
{
    Task<string> GetJwtTokenAsync();
    Task SaveJwtTokenAsync(string token);
    Task RemoveJwtTokenAsync();
}