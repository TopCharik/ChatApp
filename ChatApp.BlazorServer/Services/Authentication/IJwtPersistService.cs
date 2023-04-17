namespace ChatApp.BlazorServer.Services.Authentication;

public interface IJwtPersistService
{
    Task<string?> GetJwtTokenAsync();
    Task SetJwtTokenAsync(string token);
    Task RemoveJwtTokenAsync();
}