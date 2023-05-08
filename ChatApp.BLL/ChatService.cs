using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<ServiceResult> JoinChat(string chatLink, Participation newParticipation)
    {
        var conversationInfo = await GetParticipationByChatLink(chatLink, newParticipation.AspNetUserId);
        if (!conversationInfo.Succeeded)
        {
            return new ServiceResult(conversationInfo.Errors);
        }

        var currentParticipation = conversationInfo.Value!.Participations
            ?.FirstOrDefault(x => x.AspNetUserId == newParticipation.AspNetUserId);
        
        if (conversationInfo.Value.Participations != null 
            && currentParticipation?.HasLeft == false)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Joining chat failed", "User is already member of this chat."),
            };
            return new ServiceResult(errors);
        }

        if (conversationInfo.Value!.ChatInfo!.IsPrivate)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Joining chat failed", "This is a private chat. User can't join this chat."),
            };
            return new ServiceResult(errors);
        }

        var repo = _unitOfWork.GetRepository<IParticipationRepository>();
        if (currentParticipation == null)
        {
            newParticipation.ConversationId = conversationInfo.Value.Id;
            repo.Create(newParticipation);
        }
        else if (currentParticipation.HasLeft)
        {
            currentParticipation.HasLeft = false;
            repo.Update(currentParticipation);
            await _unitOfWork.SaveChangesAsync();
        }

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
        
        var participation = await repo
            .GetByCondition(x => x.AspNetUserId == userId && x.ConversationId == chat.Value!.Id)
            .FirstOrDefaultAsync();

        if (participation == null || participation.HasLeft)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Chat leave failed", "User is not a member of this chat."),
            };
            return new ServiceResult(errors);
        }

        participation.HasLeft = true;
        
        repo.Update(participation);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult();
    }

    public async Task<ServiceResult> CreateNewChat(Conversation conversation)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        var chat = await repo.GetChatByLink(conversation.ChatInfo.ChatLink);
        if (chat != null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Chat creation failed", "Chat with this link already exist."),
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
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Chat not found", "Chat with this link doesn't exist."),
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
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Chat not found", "Chat with this link doesn't exist."),
            };
            return new ServiceResult<Conversation>(errors);
        }
        
        
        return new ServiceResult<Conversation>(chatWithUserParticipation);
    }
    
    public async Task<ServiceResult<Conversation>> AddAvatar(Avatar avatar, string chatLink, string uploaderId)
    {
        var participationRepo = _unitOfWork.GetRepository<IConversationsRepository>();
        var chatWithUserParticipation = await participationRepo.GetChatWithUserParticipationByLink(chatLink, uploaderId);
        
        if (chatWithUserParticipation == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Avatar upload failed", "Chat with this link doesn't exist."),
            };
            return new ServiceResult<Conversation>(errors);
        }
        
        var currentParticipation = chatWithUserParticipation.Participations
            ?.FirstOrDefault(x => x.AspNetUserId == uploaderId);

        if (currentParticipation is not {CanChangeChatAvatar: true})
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Avatar upload failed", "You don't have permission for upload avatar to this chat"),
            };
            return new ServiceResult<Conversation>(errors);
        }

        var repo = _unitOfWork.GetRepository<IAvatarRepository>();

        avatar.ChatInfoId = chatWithUserParticipation.ChatInfoId;
        repo.Create(avatar);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult<Conversation>(chatWithUserParticipation);
    }
}