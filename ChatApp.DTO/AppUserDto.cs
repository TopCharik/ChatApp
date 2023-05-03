namespace ChatApp.DTO;

public class AppUserDto
{
    public string UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Age { get; set; }
    public bool isOnline { get; set; }
    public bool isInCall { get; set; }
    public List<AvatarDto> Avatars { get; set; }
}