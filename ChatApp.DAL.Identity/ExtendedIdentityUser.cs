using Microsoft.AspNetCore.Identity;

namespace ChatApp.DAL.Identity;

public class ExtendedIdentityUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Birthday { get; set; }
}