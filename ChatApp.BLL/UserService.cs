using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;

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

    public async Task<ServiceResult<AppUser>> GetUserByUsername(string username)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var user = await repo.GetUserByUsernameAsync(username);
        if (user == null)
        {
            var errors = new Dictionary<string, string>
            {
                {"Chat creation failed", $"user with username \"{username}\" not found."},
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
                var errors = new Dictionary<string, string>
                {
                    {"Chat creation failed", $"user with username \"{username}\" not found."},
                };
                return new ServiceResult<List<string>>(errors);
            }
            
            userIds.Add(user.Id);
        }

        return new ServiceResult<List<string>>(userIds);
    }
}