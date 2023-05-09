using ChatApp.Core.Entities;
using ChatApp.DAL.App.Interfaces;

namespace ChatApp.DAL.App.Repositories;

public interface IParticipationRepository : IBaseRepository<Participation>
{
    public Task<Participation?> GetUserParticipationByConversationId(string userId, int ConversationId);
}