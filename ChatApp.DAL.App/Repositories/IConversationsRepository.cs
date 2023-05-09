using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;

namespace ChatApp.DAL.App.Repositories;

public interface IConversationsRepository : IBaseRepository<Conversation>
{
    Task<PagedList<ChatInfoView>> GetChatsAsync(ChatInfoParameters parameters);
    Task<Conversation?> GetChatByLink(string ChatLink);
    Task<Conversation?> GetChatWithUserParticipationByLink(string chatLink, string userId);
    Task<Conversation?> GetChatWithUserParticipationById(int chatId, string userId);
}