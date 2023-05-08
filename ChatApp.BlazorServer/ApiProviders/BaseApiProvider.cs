using ChatApp.BlazorServer.Services.Authentication;

namespace ChatApp.BlazorServer.ApiProviders;

public class BaseApiProvider
{
    protected readonly IJwtHttpClient _jwtHttpClient;
    protected string _apiUrl;

    public BaseApiProvider(IJwtHttpClient jwtHttpClient, IConfiguration config, string apiUrl)
    {
        _jwtHttpClient = jwtHttpClient;
        _apiUrl = config["ApiUrl"];
    }
}