using ChatApp.DTO;

namespace ChatApp.BlazorServer.ApiProviders;

public interface IMessagesApiProvider
{
    Task<HttpResponseMessage> LoadMessagesAsync(int conversationId);
    Task<HttpResponseMessage> LoadMessagesAfterAsync(int conversationId, DateTime after);
    Task<HttpResponseMessage> SendMessageAsync(int conversationId, NewMessageDto newMessageDto);
}