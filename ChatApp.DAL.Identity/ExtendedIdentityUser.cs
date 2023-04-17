using Microsoft.AspNetCore.Identity;

namespace ChatApp.DAL.Identity;

public class ExtendedIdentityUser : IdentityUser
{
    public string RealName { get; set; }
}