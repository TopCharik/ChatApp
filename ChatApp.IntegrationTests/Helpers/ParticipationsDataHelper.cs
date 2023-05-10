using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.IntegrationTests.Helpers;

public class ParticipationsDataHelper
{
    public  static Participation BasicParticipation => new Participation
    {
        CanWriteMessages = true,
        CanMuteParticipants = false,
        CanDeleteMessages = false,
        CanAddParticipants = false,
        CanDeleteParticipants = false,
        CanChangePublicity = false,
        CanChangeChatAvatar = false,
        CanChangeChatTitle = false,
        CanSetPermissions = false,
        CanDeleteConversation = false,
    };
    
    public static Participation ChatOwnerParticipation = new Participation
    {
        CanWriteMessages = true,
        CanMuteParticipants = true,
        CanDeleteMessages = true,
        CanAddParticipants = true,
        CanDeleteParticipants = true,
        CanChangePublicity = true,
        CanChangeChatAvatar = true,
        CanChangeChatTitle = true,
        CanSetPermissions = true,
        CanDeleteConversation = true,
    };

    public static async Task<Participation> InsertNewParticipationToDb(DbContext dbContext , Participation participation)
    {
        dbContext.Add(participation);
        await dbContext.SaveChangesAsync();

        return dbContext.Set<Participation>()
            .First(x => x.AspNetUserId == participation.AspNetUserId &&
                        x.ConversationId == participation.ConversationId);
    }
}