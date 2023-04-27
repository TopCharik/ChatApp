using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class AuthenticationApiProvider : IAuthenticationApiProvider
{
    private readonly IJwtHttpClient _jwtHttpClient;
    private string _apiUrl;

    public AuthenticationApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"];
    }
    
    public async Task<HttpResponseMessage> Login(LoginDto loginDto)
    {
        var response = await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Users/login", loginDto);

        return response;
    }

    public async Task<HttpResponseMessage> Register(RegisterAppUserDto registerAppUserDto)
    {
        var response = await _jwtHttpClient.PostAsJsonAsync($"{_apiUrl}/api/Users/register", registerAppUserDto);
        return response;
    }

    public async Task<HttpResponseMessage> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var response = await _jwtHttpClient.PatchAsJsonAsync($"{_apiUrl}/api/Users/change-password", changePasswordDto);

        return response;
    }
}