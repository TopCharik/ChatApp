namespace ChatApp.Core.Entities.AppUserAggregate;

public class AppUser : AspNetUser
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public DateTime? Birthday { get; set; }
    public string? CallHubConnectionId { get; set; }
    public bool InCall { get; set; }
    public ICollection<Avatar> Avatars { get; }
    public ICollection<Participation> Participations { get; }
}