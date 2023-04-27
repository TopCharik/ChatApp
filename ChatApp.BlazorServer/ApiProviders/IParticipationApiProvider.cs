namespace ChatApp.BlazorServer.ApiProviders;

public interface IParticipationApiProvider
{
    Task<HttpResponseMessage> GetChatParticipation(string chatLink);
    Task<HttpResponseMessage> JoinChat(string chatLink);
    Task<HttpResponseMessage> LeaveChat(string chatLink);
}