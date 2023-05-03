using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.BLL;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<AppUser>> GetUsersAsync(AppUserParameters parameters)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();

        return await repo.GetUsersAsync(parameters);
    }

    public async Task<ServiceResult<AppUser>> GetUserByUsernameAsync(string username)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var user = await repo.GetUserByUsernameAsync(username);
        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Get User By Username failed", $"user with username \"{username}\" not found."),
            };
            return new ServiceResult<AppUser>(errors);
        }

        return  new ServiceResult<AppUser>(user);
    }

    public async Task<ServiceResult<List<string>>> GetUserIdsByUsernames(IEnumerable<string> usernames)
    {
        var userIds = new List<string>();
        var repo = _unitOfWork.GetRepository<IUserRepository>();

        foreach (var username in usernames)
        {
            var user = await repo.GetUserByUsernameAsync(username);
            if (user == null)
            {
                var errors = new List<KeyValuePair<string, string>>
                {
                    new ("Chat creation failed", $"user with username \"{username}\" not found."),
                };
                return new ServiceResult<List<string>>(errors);
            }
            
            userIds.Add(user.Id);
        }

        return new ServiceResult<List<string>>(userIds);
    }

    public async Task<ServiceResult> SetCallHubConnectionId(string username, string? callHubConnectionId)
    {
        var userRequest = await GetUserByUsernameAsync(username);
        if (!userRequest.Succeeded)
        {
            return new ServiceResult(userRequest.Errors);
        }

        var user = userRequest.Value;
        var repo = _unitOfWork.GetRepository<IUserRepository>();

        user!.CallHubConnectionId = callHubConnectionId;
        repo.Update(user);
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult();
    }

    public async Task<ServiceResult<AppUser>> RemoveCallHubConnectionId(string contextConnectionId)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var user = await repo.GetAll().AsTracking().FirstOrDefaultAsync(x => x.CallHubConnectionId == contextConnectionId);
        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new ("Remove CallHubConnectionId failed", $"user with username \"{contextConnectionId}\" not found."),
            };
            return new ServiceResult<AppUser>(errors);
        }

        user.CallHubConnectionId = null;
        repo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResult<AppUser>(user);
    }
}