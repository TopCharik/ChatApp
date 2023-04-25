using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.Helpers;

namespace ChatApp.BLL;

public interface IChatService
{
    public Task<PagedList<Conversation>> GetChatsAsync(ChatInfoParameters parameters);
    public Task<ServiceResult> CreateNewChat(Conversation conversation);
}