using ChatApp.Core.Entities;
using ChatApp.DAL.App.AppContext;

namespace ChatApp.DAL.App.Repositories;

public class ParticipationRepository : BaseRepository<Participation>, IParticipationRepository
{
    public ParticipationRepository(AppDbContext context) : base(context) { }
}