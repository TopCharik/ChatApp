using ChatApp.BLL.Helpers;
using ChatApp.BLL.Helpers.ServiceErrors;
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
            return new ServiceResult<Conversation>(AvatarServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST);
        }
        
        var currentParticipation = chatWithUserParticipation.Participations
            ?.FirstOrDefault(x => x.AspNetUserId == uploaderId);

        if (currentParticipation is {CanChangeChatAvatar: false})
        {
            return new ServiceResult<Conversation>(AvatarServiceErrors.GIVED_USER_DONT_HAVE_PERMISSION_FOR_UPLOAD_AVATAR);
        }

        var repo = _unitOfWork.GetRepository<IAvatarRepository>();

        avatar.ChatInfoId = chatWithUserParticipation.ChatInfoId;
        repo.Create(avatar);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult<Conversation>(chatWithUserParticipation);
    }
}