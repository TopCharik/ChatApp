using ChatApp.API.Controllers;
using ChatApp.API.DTOs;
using ChatApp.BlazorServer.Services.Authentication;

namespace ChatApp.BlazorServer.ApiProviders;

public class AuthenticationApiProvider : IAuthenticationApiProvider
{
    private readonly IJwtHttpClientFactory _jwtHttpClientFactory;

    public AuthenticationApiProvider(IJwtHttpClientFactory jwtHttpClientFactory)
    {
        _jwtHttpClientFactory = jwtHttpClientFactory;
    }
    
    public async Task<HttpResponseMessage> Login(LoginDto loginDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response = 
            await httpClient.PostAsJsonAsync("http://localhost:5000/api/Account/login", loginDto);

        return response;
    }

    public async Task<HttpResponseMessage> Register(UserRegisterDto userRegisterDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response = 
            await httpClient.PostAsJsonAsync("http://localhost:5000/api/Account/register", userRegisterDto);
        
        return response;
    }

    public async Task<HttpResponseMessage> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response =
            await httpClient.PostAsJsonAsync("http://localhost:5000/api/Account/change-password", changePasswordDto);

        return response;
    }
}