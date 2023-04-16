namespace ChatApp.Core.Entities;

public class Participation : BaseEntity
{
    public string AspNetUserId { get; set; } = null!;

    public int ConversationId { get; set; }
    
    public bool CanWriteMessages { get; set; }

    public bool CanDeleteMessages { get; set; }

    public DateTime? MutedUntil { get; set; }

    public bool CanMuteParticipants { get; set; }

    public bool CanAddParticipants { get; set; }

    public bool CanDeleteParticipants { get; set; }

    public int CanChangeChatAvatar { get; set; }

    public int CanChangeChatTitle { get; set; }

    public bool CanChangePublicity { get; set; }

    public bool CanSetPermissions { get; set; }

    public bool CanDeleteConversation { get; set; }

    public AppUser AppUser { get; set; } = null!;

    public Conversation Conversation { get; set; } = null!;

    public ICollection<Message> Messages { get; } = new List<Message>();
}