namespace ChatApp.BlazorServer.Services.Authentication;

public interface IJwtHttpClient
{
    Task<HttpResponseMessage> GetAsync(string url, bool logoutOnUnauthorized = true);
    Task<HttpResponseMessage> PostAsync(string url, HttpContent? httpContent, bool logoutOnUnauthorized = true);
    Task<HttpResponseMessage> PutAsync(string url, HttpContent? httpContent, bool logoutOnUnauthorized = true);
    Task<HttpResponseMessage> DeleteAsync(string url, bool logoutOnUnauthorized = true);
    Task<HttpResponseMessage> PatchAsync(string url, HttpContent? httpContent, bool logoutOnUnauthorized = true);
    Task<HttpResponseMessage> PostAsJsonAsync(string url, object value, bool logoutOnUnauthorized = true);
    Task<HttpResponseMessage> PutAsJsonAsync(string url, object value, bool logoutOnUnauthorized = true);
    Task<HttpResponseMessage> PatchAsJsonAsync(string url, object value, bool logoutOnUnauthorized = true);
}