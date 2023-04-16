namespace ChatApp.Core.Entities;

public class AspNetUser
{
    public string Id { get; private set; }
    
    public string? UserName { get; private set; }

    public string? NormalizedUserName { get; private set; }

    public string? Email { get; private set; }

    public string? NormalizedEmail { get; private set; }

    public string? PhoneNumber { get; private set; }

    public bool PhoneNumberConfirmed { get; private set; }
}