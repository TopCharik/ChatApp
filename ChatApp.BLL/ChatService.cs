using ChatApp.BLL.Helpers;
using ChatApp.BLL.Helpers.ServiceErrors;
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

    public async Task<ServiceResult<PagedList<ChatInfoView>>> GetChatsAsync(ChatInfoParameters parameters)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();
        
        var chats = await repo.GetChatsAsync(parameters);

        return new ServiceResult<PagedList<ChatInfoView>>(chats);
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
            return new ServiceResult(ChatServiceErrors.USER_IS_ALLREADY_MEMBER_OF_THIS_CHAT);
        }

        if (conversationInfo.Value!.ChatInfo!.IsPrivate)
        {
            return new ServiceResult(ChatServiceErrors.USER_CANT_JOIN_PRIVATE_CHAT);
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
        
        var participation = await repo.GetUserParticipationByConversationIdAsync(userId, chat.Value!.Id);

        if (participation == null || participation.HasLeft)
        {
            return new ServiceResult(ChatServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CHAT);
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
            return new ServiceResult(ChatServiceErrors.CHAT_WITH_THIS_LINK_ALLREADY_EXIST);
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
            return new ServiceResult<Conversation>(ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST);
        }

        return new ServiceResult<Conversation>(chat);
    }

    public async Task<ServiceResult<Conversation>> GetParticipationByChatLink(string chatLink, string userId)
    {
        var repo = _unitOfWork.GetRepository<IConversationsRepository>();

        var chatWithUserParticipation = await repo.GetChatWithUserParticipationByLink(chatLink, userId);
        if (chatWithUserParticipation == null)
        {
            return new ServiceResult<Conversation>(ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST);
        }

        return new ServiceResult<Conversation>(chatWithUserParticipation);
    }
    
    public async Task<ServiceResult<Conversation>> AddAvatar(Avatar avatar, string chatLink, string uploaderId)
    {
        var participationRepo = _unitOfWork.GetRepository<IConversationsRepository>();
        var chatWithUserParticipation = await participationRepo.GetChatWithUserParticipationByLink(chatLink, uploaderId);
        
        if (chatWithUserParticipation == null)
        {
            return new ServiceResult<Conversation>(ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST);
        }
        
        var currentParticipation = chatWithUserParticipation.Participations
            ?.FirstOrDefault(x => x.AspNetUserId == uploaderId);

        if (currentParticipation is not {CanChangeChatAvatar: true})
        {
            return new ServiceResult<Conversation>(ChatServiceErrors.USER_CANT_ADD_AVATAR_TO_THIS_CHAT);
        }

        var repo = _unitOfWork.GetRepository<IAvatarRepository>();

        avatar.ChatInfoId = chatWithUserParticipation.ChatInfoId;
        repo.Create(avatar);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult<Conversation>(chatWithUserParticipation);
    }
}