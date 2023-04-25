namespace ChatApp.Core.Entities;

public class ChatInfo : BaseEntity
{
    public string Title { get; set; }

    public string ChatLink { get; set; }

    public bool IsPrivate { get; set; }

    public ICollection<Avatar> Avatars  { get; set; }

    public Conversation? Conversation { get; set; }
}