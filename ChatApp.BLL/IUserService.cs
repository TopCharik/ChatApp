using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;

namespace ChatApp.BLL;

public interface IUserService
{
    Task<PagedList<AppUser>> GetUsersAsync(AppUserParameters parameters);
    Task<ServiceResult<AppUser>> GetUserByUsername(string username);
    Task<ServiceResult<List<string>>> GetUserIdsByUsernames(IEnumerable<string> usernames);
}