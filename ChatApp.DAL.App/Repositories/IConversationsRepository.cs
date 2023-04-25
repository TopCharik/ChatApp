using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;

namespace ChatApp.DAL.App.Repositories;

public interface IConversationsRepository : IBaseRepository<Conversation>
{
    public Task<PagedList<Conversation>> GetPublicChatsAsync(ChatInfoParameters parameters);
    public Task<Conversation?> GetChatByLink(string ChatLink);

}