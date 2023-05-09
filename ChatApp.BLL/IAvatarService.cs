using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;

namespace ChatApp.BLL;

public interface IAvatarService
{
    Task<ServiceResult> AddUserAvatar(Avatar avatar);
    Task<ServiceResult<Conversation>> AddChatAvatar(Avatar avatar, string chatLink, string uploaderId);
}   