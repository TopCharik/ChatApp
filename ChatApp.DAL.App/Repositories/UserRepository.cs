using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Helpers;
using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class UserRepository : BaseRepository<AppUser>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }


    public async Task<PagedList<AppUser>> GetUsersAsync(AppUserParameters parameters)
    {
        var users = GetAll();

        SortUsers(ref users, parameters.SortField, parameters.OrderBy);
        SearchByUserName(ref users, parameters.NormalizedUserName);
        SearchByFirstName(ref users, parameters.FirstName);
        SearchByLastName(ref users, parameters.LastName);
        SearchByNormalizedEmail(ref users, parameters.NormalizedEmail);
        SearchByPhoneNumber(ref users, parameters.PhoneNumber);
        SearchGlobal(ref users, parameters);

        return await PagedList<AppUser>.ToPagedList(
            users
                .Include(u => u.Avatars.OrderByDescending(a => a.DateSet))
            ,
            parameters.Page,
            parameters.PageSize
        );
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await _context.Set<AppUser>()
            .Include(x => x.Avatars)
            .FirstOrDefaultAsync(x => x.NormalizedUserName == username.Normalize());
    }

    private static void SearchGlobal(ref IQueryable<AppUser> users, AppUserParameters parameters)
    {
        var search = parameters.Search;

        users = string.IsNullOrEmpty(search)
            ? users
            : users.Where(u =>
                EF.Functions.Like(u.FirstName, $"%{search}%") ||
                EF.Functions.Like(u.LastName, $"%{search}%") ||
                EF.Functions.Like(u.NormalizedEmail, $"%{search}%") ||
                EF.Functions.Like(u.UserName, $"%{search}%") ||
                EF.Functions.Like(u.PhoneNumber, $"%{search}%")
                );
    }

    private static void SortUsers(ref IQueryable<AppUser> users, string? sortField, SortDirection? sortOrder)
    {
        var isAsc = sortOrder == SortDirection.Ascending;

        users = sortField?.ToLower() switch
        {
            "firstname" =>
                isAsc
                    ? users.OrderBy(u => u.FirstName)
                    : users.OrderByDescending(u => u.FirstName),
            "lastname" =>
                isAsc
                    ? users.OrderBy(u => u.LastName)
                    : users.OrderByDescending(u => u.LastName),
            "username" =>
                isAsc
                    ? users.OrderBy(u => u.UserName)
                    : users.OrderByDescending(u => u.UserName),
            "email" =>
                isAsc
                    ? users.OrderBy(u => u.Email)
                    : users.OrderByDescending(u => u.Email),
            "phone" =>
                isAsc
                    ? users.OrderBy(u => u.PhoneNumber)
                    : users.OrderByDescending(u => u.PhoneNumber),
            _ => isAsc
                ? users.OrderBy(u => u.UserName)
                : users.OrderByDescending(u => u.UserName),
        };
    }

    private static void SearchByFirstName(ref IQueryable<AppUser> users, string? realName)
    {
        users = string.IsNullOrEmpty(realName)
            ? users
            : users.Where(u => EF.Functions.Like(u.FirstName, $"%{realName}%"));
    }
    
    private static void SearchByLastName(ref IQueryable<AppUser> users, string? realName)
    {
        users = string.IsNullOrEmpty(realName)
            ? users
            : users.Where(u => EF.Functions.Like(u.LastName, $"%{realName}%"));
    }

    private static void SearchByUserName(ref IQueryable<AppUser> users, string? userName)
    {
        users = string.IsNullOrEmpty(userName)
            ? users
            : users.Where(u => EF.Functions.Like(u.UserName, $"%{userName}%"));
    }

    private static void SearchByNormalizedEmail(ref IQueryable<AppUser> users, string? normalizedEmail)
    {
        users = string.IsNullOrEmpty(normalizedEmail)
            ? users
            : users.Where(u => EF.Functions.Like(u.NormalizedEmail, $"%{normalizedEmail}%"));
    }

    private void SearchByPhoneNumber(ref IQueryable<AppUser> users, string? phoneNumber)
    {
        users = string.IsNullOrEmpty(phoneNumber)
            ? users
            : users.Where(u => EF.Functions.Like(u.PhoneNumber, $"%{phoneNumber}%"));
    }
}