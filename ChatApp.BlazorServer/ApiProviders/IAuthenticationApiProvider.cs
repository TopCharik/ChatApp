using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IAuthenticationApiProvider
{
    public Task<HttpResponseMessage> Login(LoginDto loginDto);
    public Task<HttpResponseMessage> Register(RegisterAppUserDto registerAppUserDto);
    public Task<HttpResponseMessage> ChangePassword(ChangePasswordDto changePasswordDto);
}