namespace ChatApp.Core.Entities;

public class Message : BaseEntity
{
    public string MessageText { get; set; }

    public DateTime DateSent { get; set; }
    
    public int ParticipationId { get; set; }

    public Participation Participation { get; set; }
}