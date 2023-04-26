namespace ChatApp.DTO;

public class ChatInfoDto
{
    public string Type { get; set; }
    
    public string Title { get; set; }

    public string ChatLink { get; set; }

    public bool IsPrivate { get; set; }

    public int? ParticipationsCount { get; set; }
    
    public List<AvatarDto> Avatars { get; set; }
}