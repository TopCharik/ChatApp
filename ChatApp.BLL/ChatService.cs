using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;

namespace ChatApp.BLL;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<Conversation>> GetChatsAsync(ChatInfoParameters parameters)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        return await repo.GetPublicChatsAsync(parameters);
    }

    public async Task<ServiceResult> CreateNewChat(Conversation conversation)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        var chat = await repo.GetChatByInviteLink(conversation.ChatInfo.InviteLink);
        if (chat != null)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Chat creation failed", "Chat with this Invite link already exist."},
            };
            return new ServiceResult(errors);
        }
        
        repo.Create(conversation);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult();
    }
}