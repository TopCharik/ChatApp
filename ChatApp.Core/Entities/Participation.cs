using ChatApp.Core.Entities.AppUserAggregate;

namespace ChatApp.Core.Entities;

public class Participation : BaseEntity
{
    public string AspNetUserId { get; set; }

    public int ConversationId { get; set; }
    
    public bool CanWriteMessages { get; set; }

    public bool CanDeleteMessages { get; set; }

    public DateTime? MutedUntil { get; set; }

    public bool CanMuteParticipants { get; set; }

    public bool CanAddParticipants { get; set; }

    public bool CanDeleteParticipants { get; set; }

    public bool CanChangeChatAvatar { get; set; }

    public bool CanChangeChatTitle { get; set; }

    public bool CanChangePublicity { get; set; }

    public bool CanSetPermissions { get; set; }

    public bool CanDeleteConversation { get; set; }

    public AppUser AppUser { get; set; }

    public Conversation Conversation { get; set; }

    public ICollection<Message> Messages { get; }
}