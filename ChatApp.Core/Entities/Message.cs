namespace ChatApp.Core.Entities;

public class Message : BaseEntity
{
    public int? ReplyTo { get; set; }

    public string MessageText { get; set; }

    public DateTime DateSent { get; set; }
    
    public int ParticipationId { get; set; }

    public Participation Participation { get; set; }
    
    public int ConversationId { get; set; }

    public Conversation Conversation { get; set; }

    public Message? ReplyToNavigation { get; set; }
    
    public ICollection<Message> InverseReplyToNavigation { get; }
}