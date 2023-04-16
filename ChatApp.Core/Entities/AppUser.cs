namespace ChatApp.Core.Entities;

public class AppUser : AspNetUser
{
    public ICollection<Avatar> Avatars { get; }
    public ICollection<Participation> Participations { get; }
}