using ChatApp.BlazorServer.Services.Authentication;

namespace ChatApp.BlazorServer.ApiProviders;

public abstract class BaseApiProvider
{
    protected readonly IJwtHttpClient _jwtHttpClient;
    protected string _apiUrl;

    protected BaseApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"] ?? throw new ArgumentException("Api url not found in configuration");
    }
}