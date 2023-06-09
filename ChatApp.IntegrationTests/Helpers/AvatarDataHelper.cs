using Bogus;
using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.IntegrationTests.Helpers;

public class AvatarDataHelper
{
    private static Faker<Avatar> _chatInfoFaker = new Faker<Avatar>()
        .RuleFor(x => x.ImagePayload, faker => faker.Image.PlaceImgUrl())
        .RuleFor(x => x.DateSet, _ => DateTime.Now);

    public static Avatar GenerateRandomAvatar() => _chatInfoFaker.Generate();

    public static async Task<int> GetAvatarCountAsync(DbContext dbContext)
    {
        return await dbContext.Set<Avatar>().CountAsync();
    }
}