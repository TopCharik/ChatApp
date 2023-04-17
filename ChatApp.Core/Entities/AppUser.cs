namespace ChatApp.Core.Entities;

public class AppUser : AspNetUser
{
    public string RealName { get; set; }
    public ICollection<Avatar> Avatars { get; }
    public ICollection<Participation> Participations { get; }
}