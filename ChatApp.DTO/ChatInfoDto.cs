namespace ChatApp.DTO;

public class ChatInfoDto
{
    public string Title { get; set; }

    public string InviteLink { get; set; }

    public bool IsPrivate { get; set; }

    public List<AvatarDto> Avatars { get; set; }
}