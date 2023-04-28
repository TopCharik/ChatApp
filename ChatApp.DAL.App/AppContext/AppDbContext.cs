using System.Reflection;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Entities.ChatInfoAggregate;
using Microsoft.EntityFrameworkCore;
using Avatar = ChatApp.Core.Entities.Avatar;
using ChatInfo = ChatApp.Core.Entities.ChatInfoAggregate.ChatInfo;
using Conversation = ChatApp.Core.Entities.Conversation;
using Message = ChatApp.Core.Entities.MessageArggregate.Message;
using Participation = ChatApp.Core.Entities.Participation;

namespace ChatApp.DAL.App.AppContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<ChatInfo> ChatInfos { get; set; }
    
    public virtual DbSet<ChatInfoView> ChatInfoViews { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

}