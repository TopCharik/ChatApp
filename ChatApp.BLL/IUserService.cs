using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;

namespace ChatApp.BLL;

public interface IUserService
{
    Task<PagedList<AppUser>> GetUsersAsync(AppUserParameters parameters);
    Task<AppUser?> GetUserByUsername(string username);
}