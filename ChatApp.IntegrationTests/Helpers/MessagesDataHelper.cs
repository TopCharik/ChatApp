using Bogus;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.IntegrationTests.Helpers;

public class MessagesDataHelper
{
    private static Faker<Message> _messagesFaker = new Faker<Message>()
        .RuleFor(x => x.MessageText, _ => Guid.NewGuid().ToString())
        .RuleFor(x => x.DateSent, x => x.Date.Past(3, DateTime.Now));

    public static Message GenerateRandomMessage(int participationId) => _messagesFaker
        .RuleFor(x => x.ParticipationId, _ => participationId)
        .Generate();
    public static List<Message> GenerateRandomMessages(int participationId, int messagesCount) => _messagesFaker
        .RuleFor(x => x.ParticipationId, _ => participationId)
        .Generate(messagesCount);

    public static async Task<Message> InsertMessage(DbContext dbContext, Message message)
    {
        dbContext.Add(message);
        await dbContext.SaveChangesAsync();

        return dbContext.Set<Message>().First(x => x.MessageText == message.MessageText);
    }
    
    public static async Task InsertMessages(DbContext dbContext, List<Message> messages)
    {
        await dbContext.AddRangeAsync(messages);
        await dbContext.SaveChangesAsync();
        
    }

    public static async Task<List<Message>> InserMessagesFromNewRandomUser(
        UserManager<ExtendedIdentityUser> userManager, DbContext dbContext, int conversationId)
    {
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(userManager, newUser);
        
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = conversationId;
        newParticipation.HasLeft = true;
        var participation = await ParticipationsDataHelper.InsertNewParticipationToDb(dbContext, newParticipation);

        var newMessages = MessagesDataHelper.GenerateRandomMessages(participation.Id, Random.Shared.Next(2, 10));
        await MessagesDataHelper.InsertMessages(dbContext, newMessages);

        return await dbContext.Set<Participation>()
            .Where(x => x.ConversationId == conversationId)
            .Include(x => x.Messages)
            .SelectMany(x => x.Messages)
            .ToListAsync();
    }
}