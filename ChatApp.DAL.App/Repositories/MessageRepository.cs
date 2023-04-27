using ChatApp.Core.Entities;
using ChatApp.DAL.App.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(AppDbContext context) : base(context) { }

    public async Task<List<Message>> GetMessages(int conversationId)
    {
        var messages = GetAll()
            .Include(x => x.Participation)
            .ThenInclude(x => x.AppUser)
            .ThenInclude(x => x.Avatars)
            .Where(x => x.Participation.ConversationId == conversationId);

        return await messages.ToListAsync();
    }
}