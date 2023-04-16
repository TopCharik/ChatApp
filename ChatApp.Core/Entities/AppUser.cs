namespace ChatApp.Core.Entities;

public class AppUser : AspNetUser
{
    public ICollection<Avatar> Avatars { get; } = new List<Avatar>();

    public ICollection<Participation> Participations { get; } = new List<Participation>();
}