using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.BLL;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    public MessageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Message>> GetMessages(int conversationId)
    {
        var repo = _unitOfWork.GetRepository<IMessageRepository>();

        var messages = await repo.GetMessages(conversationId);

        return messages;
    }

    public async Task<ServiceResult> SendMessage(Message message, string senderId, int conversationId)
    {
        var participationRepo = _unitOfWork.GetRepository<IParticipationRepository>();
        var participation = await participationRepo
            .GetByCondition(x => x.AspNetUserId == senderId)
            .FirstOrDefaultAsync(x => x.ConversationId == conversationId);

        if (participation == null)
        {
            var errors = new Dictionary<string, string>
            {
                {"Message creation failed", $"User is not a member of this conversation."},
            };
            return new ServiceResult(errors);
        }
        
        if (!participation.CanWriteMessages)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Message creation failed", $"User can't write messages in this chat."},
            };
            return new ServiceResult(errors);
        }
        
        if (participation.MutedUntil > DateTime.Now)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Message creation failed", $"User muted until {participation.MutedUntil}."},
            };
            return new ServiceResult(errors);
        }
        
        var repo = _unitOfWork.GetRepository<IMessageRepository>();

        repo.Create(message);
        message.ParticipationId = participation.Id;
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult();
    }
}