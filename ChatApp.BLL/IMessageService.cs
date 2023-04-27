using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;

namespace ChatApp.BLL;

public interface IMessageService
{
    Task<ServiceResult<List<Message>>> GetMessages(int conversationId, string userId);
    Task<ServiceResult> SendMessage(Message message, string senderId, int conversationId);
}