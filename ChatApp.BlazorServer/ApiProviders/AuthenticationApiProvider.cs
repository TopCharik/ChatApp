using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class AuthenticationApiProvider : BaseApiProvider ,IAuthenticationApiProvider
{
    public AuthenticationApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, string apiUrl)
        : base(jwtHttpClient, config, apiUrl)
    {
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