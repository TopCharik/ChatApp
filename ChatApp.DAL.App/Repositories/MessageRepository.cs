using ChatApp.Core.Entities;
using ChatApp.DAL.App.AppContext;

namespace ChatApp.DAL.App.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(AppDbContext context) : base(context) { }
}