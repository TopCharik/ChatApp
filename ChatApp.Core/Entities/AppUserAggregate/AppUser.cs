namespace ChatApp.Core.Entities.AppUserAggregate;

public class AppUser : AspNetUser
{
    public string RealName { get; private set; }
    public ICollection<Avatar> Avatars { get; }
    public ICollection<Participation> Participations { get; }
}