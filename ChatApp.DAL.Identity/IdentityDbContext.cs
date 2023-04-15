using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.Identity;

public class IdentityDbContext : IdentityDbContext<IdentityUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

}