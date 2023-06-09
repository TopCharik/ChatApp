using ChatApp.Core.Entities.ChatInfoAggregate;

namespace ChatApp.Core.Entities;

public class Conversation : BaseEntity
{
    public int? ChatInfoId { get; set; }

    public ChatInfo? ChatInfo { get; set; }

    public ICollection<Participation>? Participations { get; set; }
}