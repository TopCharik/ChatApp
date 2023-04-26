using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Entities.ChatInfoAggregate;

namespace ChatApp.Core.Entities;

public class Avatar : BaseEntity
{
    public string? UserId { get; set; }

    public int? ChatInfoId { get; set; }

    public string PictureUrl { get; set; }

    public DateTime DateSet { get; set; }

    public ChatInfo? ChatInfo { get; set; }
    
    public ChatInfoView? ChatInfoView { get; set; }

    public AppUser? User { get; set; }
}