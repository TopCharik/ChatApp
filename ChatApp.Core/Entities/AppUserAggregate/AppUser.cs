namespace ChatApp.Core.Entities.AppUserAggregate;

public class AppUser : AspNetUser
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public DateTime? Birthday { get; init; }
    public string? CallHubConnectionId { get; set; }
    public bool InCall { get; set; }
    public List<Avatar> Avatars { get; init; }
    public List<Participation> Participations { get; init;}
}