using ChatApp.Core.Entities;
using ChatApp.DAL.App.AppContext;

namespace ChatApp.DAL.App.Repositories;

public class AvatarRepository : BaseRepository<Avatar>, IAvatarRepository
{
    public AvatarRepository(AppDbContext context) : base(context) { }
}