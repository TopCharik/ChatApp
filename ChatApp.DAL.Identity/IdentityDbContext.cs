using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.Identity;

public class IdentityDbContext : IdentityDbContext<ExtendedIdentityUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

}