using System.Net;

namespace ChatApp.BlazorServer.Services.Authentication;

public class JwtHttpClient : IJwtHttpClient
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IHttpClientFactory _httpClientFactory;

    public JwtHttpClient(
        IJwtProvider jwtProvider,
        IHttpClientFactory httpClientFactory
    )
    {
        _jwtProvider = jwtProvider;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<HttpResponseMessage> GetAsync(string url, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.GetAsync(url);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent? httpContent, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.PostAsync(url, httpContent);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    public async Task<HttpResponseMessage> PutAsync(string url, HttpContent? httpContent, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.PutAsync(url, httpContent);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.DeleteAsync(url);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    public async Task<HttpResponseMessage> PatchAsync(string url, HttpContent? httpContent, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.PatchAsync(url, httpContent);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync(string url, object value, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.PostAsJsonAsync(url, value);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    public async Task<HttpResponseMessage> PutAsJsonAsync(string url, object value, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.GetAsync(url);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    public async Task<HttpResponseMessage> PatchAsJsonAsync(string url, object value, bool logoutOnUnauthorized = true)
    {
        var client = await Before();
        
        var response = await client.PatchAsJsonAsync(url, value);

        await AfterHttpRequest(response, logoutOnUnauthorized);
        return response;
    }

    private async Task<HttpClient> Before()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var token = await _jwtProvider.GetTokenAsync();
        

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");


        return httpClient;
    }
    
    private async Task<HttpClient> BeforeHttpRequest()
    {
        var httpClient = _httpClientFactory.CreateClient();
        var token = await _jwtProvider.GetTokenAsync();
        

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");


        return httpClient;
    }


    private async Task AfterHttpRequest(HttpResponseMessage response, bool logoutOnUnauthorized)
    {
        if (logoutOnUnauthorized && response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _jwtProvider.DeleteTokenAsync();
        }
    }
}