using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.Helpers;

namespace ChatApp.BLL;

public interface IChatService
{
    Task<ServiceResult> CreateNewChat(Conversation conversation);
    Task<ServiceResult> JoinChat(string chatLink, Participation newParticipation);
    Task<ServiceResult> LeaveChat(string chatLink, string userId);
    Task<PagedList<ChatInfoView>> GetChatsAsync(ChatInfoParameters parameters);
    Task<ServiceResult<Conversation>> GetChatByLink(string chatLink);
    Task<ServiceResult<Conversation>> GetParticipationByChatLink(string chatLink, string userId);
    Task<ServiceResult<Conversation>> AddAvatar(Avatar avatar, string chatLink, string uploaderId);
}