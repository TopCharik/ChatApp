namespace ChatApp.DTO;

public class AppUserDto
{
    public string RealName { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }
    
    public List<AvatarDto> Avatars { get; set; }
    
    public string PhoneNumber { get; set; }
}