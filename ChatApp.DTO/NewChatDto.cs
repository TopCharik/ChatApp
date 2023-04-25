namespace ChatApp.DTO;

public class NewChatDto
{
    public string Title { get; set; }
    public string ChatLink { get; set; }
    public bool IsPrivate { get; set; }
    public string? AvatarUrl { get; set; }
    public string[]? AppUserIds { get; set; }
}