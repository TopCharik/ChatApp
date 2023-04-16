namespace ChatApp.Core.Entities;

public class Avatar : BaseEntity
{
    public string? UserId { get; set; }

    public int? ChatInfoId { get; set; }

    public string PictureUrl { get; set; } = null!;

    public DateTime DateSet { get; set; }

    public ChatInfo? ChatInfo { get; set; }

    public AspNetUser? User { get; set; }
}