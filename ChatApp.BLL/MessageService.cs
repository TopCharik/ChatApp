using ChatApp.BLL.Helpers;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;

namespace ChatApp.BLL;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    public MessageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<List<Message>>> 
        GetMessages(int conversationId, string userId, MessageParameters messageParameters)
    {
        var conversationsRepo = _unitOfWork.GetRepository<IConversationsRepository>();

        var chatWithUserParticipation = await conversationsRepo.GetChatWithUserParticipationById(conversationId, userId);
        
        if (chatWithUserParticipation == null)
        {
            return new ServiceResult<List<Message>>(MessageServiceErrors.CHAT_NOT_FOUND);
        }
        
        var currentParticipation = chatWithUserParticipation.Participations?.FirstOrDefault(x => x.AspNetUserId == userId);
        
        if (chatWithUserParticipation.ChatInfo!.IsPrivate
            && currentParticipation is null or {HasLeft: true})
        {
            return new ServiceResult<List<Message>>(MessageServiceErrors.USER_CANT_READ_MESSAGES_FROM_THIS_CHAT);
        }
        
        var messageRepo = _unitOfWork.GetRepository<IMessageRepository>();

        var messages = await messageRepo.GetMessagesAsync(conversationId, messageParameters);

        return new ServiceResult<List<Message>>(messages);
    }

    public async Task<ServiceResult> SendMessage(Message message, string senderId, int conversationId)
    {
        var participationRepo = _unitOfWork.GetRepository<IParticipationRepository>();
        var participation = await participationRepo.GetUserParticipationByConversationIdAsync(senderId, conversationId);

        if (participation == null || participation.HasLeft)
        {
            return new ServiceResult(MessageServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION);
        }
        
        if (!participation.CanWriteMessages)
        {
            return new ServiceResult(MessageServiceErrors.USER_CANT_WRITE_MESSAGES_IN_THIS_CHAT);
        }
        
        if (participation.MutedUntil != null && participation.MutedUntil > DateTime.Now)
        {
            return new ServiceResult(MessageServiceErrors.USER_IS_MUTED_UNTIL(participation.MutedUntil));
        }
        
        var repo = _unitOfWork.GetRepository<IMessageRepository>();

        repo.Create(message);
        message.ParticipationId = participation.Id;
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult();
    }
}