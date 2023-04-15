using ChatApp.API.Controllers;
using ChatApp.API.DTOs;

namespace ChatApp.BlazorServer.ApiProviders;

public class AuthenticationApiProvider : IAuthenticationApiProvider
{
    private readonly IJwtHttpClientFactory _jwtHttpClientFactory;

    public AuthenticationApiProvider(IJwtHttpClientFactory jwtHttpClientFactory)
    {
        _jwtHttpClientFactory = jwtHttpClientFactory;
    }

    public async Task<UserDto> Login(LoginDto loginDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response = 
            await httpClient.PostAsJsonAsync("http://localhost:5000/api/Account/login", loginDto);

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<UserDto> Register(UserRegisterDto userRegisterDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response = 
            await httpClient.PostAsJsonAsync("http://localhost:5000/api/Account/register", userRegisterDto);
        
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<UserDto> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var httpClient = await _jwtHttpClientFactory.CreateJwtClientAsync();
        var response =
            await httpClient.PostAsJsonAsync("http://localhost:5000/api/Account/change-password", changePasswordDto);

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}