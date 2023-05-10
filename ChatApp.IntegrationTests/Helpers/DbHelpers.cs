using Microsoft.EntityFrameworkCore;

namespace ChatApp.IntegrationTests;

public class DbHelpers
{
    public static async Task ClearDb(DbContext dbContext)
    {
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserClaims]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetRoleClaims]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetRoles]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUsers]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserLogins]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserRoles]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [AspNetUserTokens]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Avatars]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [ChatInfos]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Conversations]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Messages]");
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Participations]");
    }
}