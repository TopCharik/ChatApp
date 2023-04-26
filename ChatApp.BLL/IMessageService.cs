using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;

namespace ChatApp.BLL;

public interface IMessageService
{
    Task<ServiceResult> SendMessage(Message message, string senderId);
}