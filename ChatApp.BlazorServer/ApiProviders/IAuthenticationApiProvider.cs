using ChatApp.API.Controllers;
using ChatApp.API.DTOs;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IAuthenticationApiProvider
{
    public Task<UserDto> Login(LoginDto loginDto);
    public Task<UserDto> Register(UserRegisterDto userRegisterDto);
    public Task<UserDto> ChangePassword(ChangePasswordDto changePasswordDto);
}