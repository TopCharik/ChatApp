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

    public async Task<PagedList<ChatInfoView>> GetChatsAsync(ChatInfoParameters parameters)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        return await repo.GetPublicChatsAsync(parameters);
    }
    
    public async Task<ServiceResult> JoinChat(string chatLink, Participation participation)
    {
        var conversationInfo = await GetParticipationByChatLink(chatLink, participation.AspNetUserId);
        if (!conversationInfo.Succeeded)
        {
            return new ServiceResult(conversationInfo.Errors);
        }
        
        if (conversationInfo.Value.Participations != null 
            && conversationInfo.Value.Participations.FirstOrDefault()?.AspNetUserId == participation.AspNetUserId)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Joining chat failed", "User is already member of this chat."},
            };
            return new ServiceResult(errors);
        }

        if (conversationInfo.Value!.ChatInfo!.IsPrivate)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Joining chat failed", "This is a private chat. User can't join this chat."},
            };
            return new ServiceResult(errors);
        }

        participation.ConversationId = conversationInfo.Value.Id;

        var repo = _unitOfWork.GetRepository<IParticipationRepository>();
        repo.Create(participation);
        
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResult();
    }
    
    public async Task<ServiceResult> LeaveChat(string chatLink, string userId)
    {
        var chat = await GetChatByLink(chatLink);
        if (!chat.Succeeded)
        {
            return new ServiceResult(chat.Errors);

        }

        var repo = _unitOfWork.GetRepository<IParticipationRepository>();
        
        var participation = repo
            .GetByCondition(x => x.AspNetUserId == userId && x.ConversationId == chat.Value!.Id)
            .FirstOrDefault();

        if (participation == null)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Chat leave failed", "User is not a member of this chat."},
            };
            return new ServiceResult(errors);
        }
        
        repo.Delete(participation);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult();
    }

    public async Task<ServiceResult> CreateNewChat(Conversation conversation)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        var chat = await repo.GetChatByLink(conversation.ChatInfo.ChatLink);
        if (chat != null)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Chat creation failed", "Chat with this link already exist."},
            };
            return new ServiceResult(errors);
        }

        repo.Create(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResult();
    }

    public async Task<ServiceResult<Conversation>> GetChatByLink(string chatLink)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        var chat = await repo.GetChatByLink(chatLink);
        if (chat == null)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Chat not found", "Chat with this link doesn't exist."},
            };
            return new ServiceResult<Conversation>(errors);
        }

        return new ServiceResult<Conversation>(chat);
    }

    public async Task<ServiceResult<Conversation>> GetParticipationByChatLink(string chatLink, string userId)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        var chatWithUserParticipation = await repo.GetChatWithUserParticipationByLink(chatLink, userId);
        if (chatWithUserParticipation == null)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Chat not found", "Chat with this link doesn't exist."},
            };
            return new ServiceResult<Conversation>(errors);
        }
        
        
        return new ServiceResult<Conversation>(chatWithUserParticipation);
    }
}