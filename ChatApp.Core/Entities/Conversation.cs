namespace ChatApp.Core.Entities;

public class Conversation : BaseEntity
{
    public int? ChatInfoId { get; set; }

    public virtual ChatInfo? ChatInfo { get; set; }

    public virtual ICollection<Participation> Participations { get; } = new List<Participation>();
}