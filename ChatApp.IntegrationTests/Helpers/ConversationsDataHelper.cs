using Bogus;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.IntegrationTests.Helpers;

public class ConversationsDataHelper
{
    private static Faker<ChatInfo> _chatInfoFaker = new Faker<ChatInfo>()
        .RuleFor(x => x.Title, x => x.Random.AlphaNumeric(100))
        .RuleFor(x => x.ChatLink, x => x.Random.AlphaNumeric(100));

    public static Conversation GenerateRandomChat() => new Faker<Conversation>()
        .RuleFor(x => x.ChatInfo, _ => _chatInfoFaker.Generate())
        .Generate();

    public static async Task<Conversation> InsertNewChatToDbAsync(DbContext dbContext, Conversation newChat)
    {
        dbContext.Add(newChat);
        await dbContext.SaveChangesAsync();

        return await dbContext.Set<Conversation>().Include(x => x.ChatInfo)
            .FirstAsync(x => x.ChatInfo.ChatLink == newChat.ChatInfo.ChatLink && x.ChatInfo.Title == newChat.ChatInfo.Title);
    }
    
    public static async Task<ChatInfoView> GetChatInfoViewByChat(DbContext dbContext, Conversation conversation)
    {
        return await dbContext.Set<ChatInfoView>()
            .FirstAsync(x => x.ChatInfoId == conversation.ChatInfoId);
    }

    public static async Task<int> GetChatsCount(DbContext dbContext)
    {
        return await dbContext.Set<Conversation>()
            .Where(x => x.ChatInfoId != null)
            .CountAsync();
    }
}