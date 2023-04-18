using ChatApp.BlazorServer.Services.Authentication;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public class AuthenticationApiProvider : IAuthenticationApiProvider
{
    private readonly IJwtHttpClientFactory _jwtHttpClientFactory;
    private string _apiUrl;

    public AuthenticationApiProvider(IJwtHttpClientFactory jwtHttpClientFactory, IConfiguration config)
    {
        _jwtHttpClientFactory = jwtHttpClientFactory;
        _apiUrl = config["ApiUrl"];
    }
    
    public async Task<HttpResponseMessage> Login(LoginDto loginDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response = 
            await httpClient.PostAsJsonAsync($"{_apiUrl}/api/Account/login", loginDto);

        return response;
    }

    public async Task<HttpResponseMessage> Register(UserRegisterDto userRegisterDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response = 
            await httpClient.PostAsJsonAsync($"{_apiUrl}/api/Account/register", userRegisterDto);
        
        return response;
    }

    public async Task<HttpResponseMessage> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response =
            await httpClient.PostAsJsonAsync($"{_apiUrl}/api/Account/change-password", changePasswordDto);

        return response;
    }
}