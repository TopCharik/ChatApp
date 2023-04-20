using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
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

        SearchGlobal(ref users, parameters);
        SortUsers(ref users, parameters.SortField, parameters.OrderBy);
        SearchByUserName(ref users, parameters.NormalizedUserName);
        SearchByRealName(ref users, parameters.RealName);
        SearchByNormalizedEmail(ref users, parameters.NormalizedEmail);
        users = SearchByPhoneNumber(users, parameters.PhoneNumber);

        return await PagedList<AppUser>.ToPagedList(
            users
                .Include(u => u.Avatars.OrderByDescending(a => a.DateSet))
            ,
            parameters.Page,
            parameters.PageSize
        );
    }

    private static void SearchGlobal(ref IQueryable<AppUser> users, AppUserParameters parameters)
    {
        var search = parameters.Search;

        users = string.IsNullOrEmpty(search)
            ? users
            : users.Where(u =>
                u.RealName.Contains(search) ||
                u.NormalizedEmail.Contains(search) ||
                u.UserName.Contains(search) ||
                u.PhoneNumber.Contains(search)
            );
    }

    private static void SortUsers(ref IQueryable<AppUser> users, string? sortField, string? sortOrder)
    {
        var isAsc = sortOrder == "asc";

        users = sortField switch
        {
            "realname" =>
                isAsc
                    ? users.OrderBy(u => u.RealName)
                    : users.OrderByDescending(u => u.RealName),
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

    private static void SearchByRealName(ref IQueryable<AppUser> users, string? realName)
    {
        users = string.IsNullOrEmpty(realName)
            ? users
            : users.Where(u => u.RealName.Contains(realName));
    }

    private static void SearchByUserName(ref IQueryable<AppUser> users, string? userName)
    {
        users = string.IsNullOrEmpty(userName)
            ? users
            : users.Where(u => u.UserName.Contains(userName));
    }

    private static void SearchByNormalizedEmail(ref IQueryable<AppUser> users, string? normalizedEmail)
    {
        users = string.IsNullOrEmpty(normalizedEmail)
            ? users
            : users.Where(u => u.NormalizedEmail.Contains(normalizedEmail));
    }

    private IQueryable<AppUser> SearchByPhoneNumber(IQueryable<AppUser> users, string? phoneNumber)
    {
        return string.IsNullOrEmpty(phoneNumber)
            ? users
            : users.Where(u => u.PhoneNumber.Contains(phoneNumber));
    }
}