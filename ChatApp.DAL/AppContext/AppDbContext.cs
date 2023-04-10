using ChatApp.Core.Entities.FooAggregate;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.AppContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Foo> Foos { get; set; }
}