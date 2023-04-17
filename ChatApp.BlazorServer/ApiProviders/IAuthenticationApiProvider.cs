using ChatApp.API.Controllers;
using ChatApp.API.DTOs;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IAuthenticationApiProvider
{
    public Task<HttpResponseMessage> Login(LoginDto loginDto);
    public Task<HttpResponseMessage> Register(UserRegisterDto userRegisterDto);
    public Task<HttpResponseMessage> ChangePassword(ChangePasswordDto changePasswordDto);
}