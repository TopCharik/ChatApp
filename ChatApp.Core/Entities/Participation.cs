namespace ChatApp.Core.Entities;

public class Participation : BaseEntity
{

    public string AspNetUserId { get; set; } = null!;

    public int ConversationId { get; set; }

    public AspNetUser AspNetUser { get; set; } = null!;

    public Conversation Conversation { get; set; } = null!;

    public ICollection<Message> Messages { get; } = new List<Message>();
}