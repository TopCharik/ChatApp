namespace ChatApp.Core.Entities.ChatInfoAggregate;

public class ChatInfoView
{
    public int ConversationId { get; set; }
    
    public int ChatInfoId { get; set; }

    public string Title { get; set; } = null!;

    public string ChatLink { get; set; } = null!;

    public bool IsPrivate { get; set; }

    public int? ParticipationsCount { get; set; }
    
    public ICollection<Avatar>? Avatars  { get; set; }
}
