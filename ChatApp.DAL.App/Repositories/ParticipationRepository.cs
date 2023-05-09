using ChatApp.Core.Entities;
using ChatApp.DAL.App.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class ParticipationRepository : BaseRepository<Participation>, IParticipationRepository
{
    public ParticipationRepository(AppDbContext context) : base(context) { }

    public Task<Participation?> GetUserParticipationByConversationId(string userId, int ConversationId)
    {
        return GetByCondition(x => x.AspNetUserId == userId && x.ConversationId == ConversationId)
            .FirstOrDefaultAsync();
    }
}