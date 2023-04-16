namespace ChatApp.Core.Entities;

public class ChatInfo : BaseEntity
{
    public string Title { get; set; } = null!;

    public string InviteLink { get; set; } = null!;

    public bool IsPrivate { get; set; }

    public ICollection<Avatar> Avatars { get; } = new List<Avatar>();

    public Conversation? Conversation { get; set; }
}