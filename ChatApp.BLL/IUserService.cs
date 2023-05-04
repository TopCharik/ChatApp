using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DTO;

namespace ChatApp.BLL;

public interface IUserService
{
    Task<PagedList<AppUser>> GetUsersAsync(AppUserParameters parameters);
    Task<ServiceResult<AppUser>> GetUserByUsernameAsync(string username);
    Task<ServiceResult<AppUser>> GetUserByConnectionId(string connectionId);
    Task<ServiceResult<List<string>>> GetUserIdsByUsernames(IEnumerable<string> usernames);
    Task<ServiceResult> SetCallHubConnectionId(string username, string? callHubConnectionId);
    Task<ServiceResult<AppUser>> RemoveCallHubConnectionId(string contextConnectionId);
    Task<ServiceResult<CallParticipants>> SetInCall(CallUsernamesDto callUsernamesDto, bool newValue);
    Task<ServiceResult<AppUser>> SetInCallByConnectionId(string contextConnectionId, bool newValue);
}