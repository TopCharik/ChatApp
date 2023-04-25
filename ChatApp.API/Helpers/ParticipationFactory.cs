using ChatApp.Core.Entities;

namespace ChatApp.API.Helpers;

public static class ParticipationFactory
{
    public static Participation DefaultChatMember(string AppUserId)
    {
        return new Participation
        {
            AspNetUserId = AppUserId,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        };
    }
    
    public static Participation ChatOwner(string AppUserId)
    {
        return new Participation
        {
            AspNetUserId = AppUserId,
            CanWriteMessages = true,
            CanMuteParticipants = true,
            CanDeleteMessages = true,
            CanAddParticipants = true,
            CanDeleteParticipants = true,
            CanChangePublicity = true,
            CanChangeChatAvatar = true,
            CanChangeChatTitle = true,
            CanSetPermissions = true,
            CanDeleteConversation = true,
        };
    }
}