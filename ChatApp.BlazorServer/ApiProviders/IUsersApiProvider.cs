namespace ChatApp.BlazorServer.ApiProviders;

public interface IUsersApiProvider
{
    public Task<HttpResponseMessage> LoadUsers(Dictionary<string, string> queryParams);
    public Task<HttpResponseMessage> LoadSingleUser(string username);
}