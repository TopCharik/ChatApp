namespace ChatApp.BlazorServer.Services.Authentication;

public interface IJwtStorage
{
    Task<string?> GetJwtTokenAsync();
    Task SetJwtTokenAsync(string token);
    Task RemoveJwtTokenAsync();
}