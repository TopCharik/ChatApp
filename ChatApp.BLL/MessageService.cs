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

    public async Task<ServiceResult> SendMessage(Message message, string senderId)
    {
        var participationRepo = _unitOfWork.GetRepository<IParticipationRepository>();
        var participation = await participationRepo
            .GetByCondition(x => x.AspNetUserId == senderId)
            .FirstOrDefaultAsync(x => x.Id == message.ParticipationId);

        if (participation == null)
        {
            var errors = new Dictionary<string, string>
            {
                {"Message creation failed", $"Can't find participation with ParticipationId={message.ParticipationId}"},
            };
            return new ServiceResult(errors);
        }
        
        if (!participation.CanWriteMessages)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Message creation failed", $"User can't write messages in this chat"},
            };
            return new ServiceResult(errors);
        }
        
        if (participation.MutedUntil > DateTime.Now)
        {
            var errors = new Dictionary<string, string>()
            {
                {"Message creation failed", $"User muted untill {participation.MutedUntil}"},
            };
            return new ServiceResult(errors);
        }
        
        var repo = _unitOfWork.GetRepository<IMessageRepository>();

        message.ConversationId = participation.ConversationId;
        repo.Create(message);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult();
    }
}