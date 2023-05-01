using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;

namespace ChatApp.BLL;

public class AvatarService : IAvatarService
{
    private readonly IUnitOfWork _unitOfWork;

    public AvatarService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ServiceResult> AddUserAvatar(Avatar avatar)
    {
        var repo = _unitOfWork.GetRepository<IAvatarRepository>();
        repo.Create(avatar);
        await _unitOfWork.SaveChangesAsync();
        return new ServiceResult();
    }

    public async Task<ServiceResult<Conversation>> AddChatAvatar(Avatar avatar, string chatLink, string uploaderId)
    {
        var participationRepo = _unitOfWork.GetRepository<IConversationsRepository>();
        var chatWithUserParticipation = await participationRepo.GetChatWithUserParticipationByLink(chatLink, uploaderId);
        
        if (chatWithUserParticipation == null)
        {
            var errors = new Dictionary<string, string>
            {
                {"Avatar upload failed", "Chat with this link doesn't exist."},
            };
            return new ServiceResult<Conversation>(errors);
        }
        
        var currentParticipation = chatWithUserParticipation.Participations
            ?.FirstOrDefault(x => x.AspNetUserId == uploaderId);

        if (currentParticipation is {CanChangeChatAvatar: false})
        {
            var errors = new Dictionary<string, string>
            {
                {"Avatar upload failed", "You don't have permission for upload avatar to this chat"},
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