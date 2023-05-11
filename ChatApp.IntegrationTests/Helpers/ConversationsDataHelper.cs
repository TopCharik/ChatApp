using Bogus;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.IntegrationTests.Helpers;

public class ConversationsDataHelper
{
    private static Faker<ChatInfo> _chatInfoFaker = new Faker<ChatInfo>()
        .RuleFor(x => x.Title, x => x.Lorem.Word())
        .RuleFor(x => x.ChatLink, x => x.Lorem.Word());

    public static Conversation GenerateRandomChat() => new Faker<Conversation>()
        .RuleFor(x => x.ChatInfo, _ => _chatInfoFaker.Generate())
        .Generate();

    public static async Task<Conversation> InsertNewChatToDbAsync(DbContext dbContext, Conversation newChat)
    {
        dbContext.Add(newChat);
        await dbContext.SaveChangesAsync();

        return dbContext.Set<Conversation>().Include(x => x.ChatInfo)
            .First(x => x.ChatInfo.ChatLink == newChat.ChatInfo.ChatLink && x.ChatInfo.Title == x.ChatInfo.Title);
    }
}