using ChatApp.Core.Entities;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(AppDbContext context) : base(context) { }

    public async Task<List<Message>> GetMessages(int conversationId, MessageParameters messageParameters)
    {
        var messages = GetAll()
            .Include(x => x.Participation)
            .ThenInclude(x => x.AppUser)
            .ThenInclude(x => x.Avatars.OrderByDescending(x => x.DateSet).Take(1))
            .Where(x => x.Participation.ConversationId == conversationId);
        
        AfterTimeFilter(ref messages, messageParameters.After);

        return await messages.ToListAsync();
    }
    
    private static void AfterTimeFilter(ref IQueryable<Message> messages, DateTime? after)
    {
        messages = after == null
            ? messages
            : messages.Where(x => x.DateSent > after);
    }
}