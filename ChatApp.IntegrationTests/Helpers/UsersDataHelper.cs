using System.Data.Common;
using Bogus;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.IntegrationTests.Helpers;

public class UsersDataHelper
{
    private static Faker<AppUser> _userFaker = new Faker<AppUser>()
        .RuleFor(x => x.Id, _ => Guid.NewGuid().ToString())
        .RuleFor(x => x.UserName, x => x.Person.UserName)
        .RuleFor(x => x.FirstName, x => x.Person.FirstName)
        .RuleFor(x => x.LastName, x => x.Person.LastName)
        .RuleFor(x => x.Email, x => x.Person.Email)
        .RuleFor(x => x.PhoneNumber, x => x.Person.Phone)
        .RuleFor(x => x.Birthday, _ => DateTime.Today.AddDays(Random.Shared.Next(-120, -10)));
    
    private static Faker<ExtendedIdentityUser> _userIdentityFaker = new Faker<ExtendedIdentityUser>()
        .RuleFor(x => x.UserName, x => x.Person.UserName)
        .RuleFor(x => x.FirstName, x => x.Person.FirstName)
        .RuleFor(x => x.LastName, x => x.Person.LastName)
        .RuleFor(x => x.Email, x => x.Person.Email)
        .RuleFor(x => x.PhoneNumber, x => x.Person.Phone)
        .RuleFor(x => x.Birthday, _ => DateTime.Today.AddDays(Random.Shared.Next(-120, -10)));

    public static AppUser GenerateRandomUser() => _userFaker.Generate();
    public static ExtendedIdentityUser GenerateRandomUserIdentity() => _userIdentityFaker.Generate();

    public static async Task<AppUser> RegisterNewUserAsync(UserManager<ExtendedIdentityUser> userManager, ExtendedIdentityUser newUserIdentity)
    {
        var result = await userManager.CreateAsync(newUserIdentity);
        if (!result.Succeeded)
        {
            throw new Exception("New user creation failed");
        }

        var createdUser = await userManager.FindByNameAsync(newUserIdentity.UserName);
            
        return new AppUser
        {
            Id = createdUser.Id,
            UserName = createdUser.UserName,
            NormalizedUserName = createdUser.NormalizedUserName,
            Email = createdUser.Email,
            NormalizedEmail = createdUser.NormalizedEmail,
            PhoneNumber = createdUser.PhoneNumber,
            FirstName = createdUser.FirstName,
            LastName = createdUser.LastName,
            Birthday = createdUser.Birthday,
        };
    }

    public static async Task<AppUser> UpdateUserAsync(DbContext dbContext, AppUser user)
    {
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();

        return await dbContext.Set<AppUser>().FirstAsync(x => x.Id == user.Id);
    }
}