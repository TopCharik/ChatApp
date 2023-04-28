using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.MessageArggregate;

namespace ChatApp.BLL;

public interface IMessageService
{
    Task<ServiceResult<List<Message>>> GetMessages(int conversationId, string userId, MessageParameters messageParameters);
    Task<ServiceResult> SendMessage(Message message, string senderId, int conversationId);
}