using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;

namespace ChatApp.BLL;

public interface IMessageService
{
    Task<List<Message>> GetMessages(int conversationId);
    Task<ServiceResult> SendMessage(Message message, string senderId, int conversationId);
}