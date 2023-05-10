namespace ChatApp.Core.Entities.AppUserAggregate;

public class AspNetUser
{
    public string Id { get;  init; }
    public string? UserName { get; init; }
    public string? NormalizedUserName { get; init; }
    public string? Email { get; init; }
    public string? NormalizedEmail { get; init; }
    public string? PhoneNumber { get; init; }
}