using ChatApp.BLL.Helpers;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using ChatApp.DTO;
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
                new("Get User By Username failed", $"user with username \"{username}\" not found."),
            };
            return new ServiceResult<AppUser>(errors);
        }

        return new ServiceResult<AppUser>(user);
    }

    public async Task<ServiceResult<AppUser>> GetUserByConnectionId(string connectionId)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();

        var user = await repo.GetAll().FirstOrDefaultAsync(x => x.CallHubConnectionId == connectionId);
        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("Get User By ConnectionId", "user not found"),
            };
            return new ServiceResult<AppUser>(errors);
        }

        return new ServiceResult<AppUser>(user);
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
                    new("Get User Id By Username", $"user with username \"{username}\" not found."),
                };
                return new ServiceResult<List<string>>(errors);
            }

            userIds.Add(user.Id);
        }

        return new ServiceResult<List<string>>(userIds);
    }

    public async Task<ServiceResult> AddAvatar(Avatar avatar)
    {
        var repo = _unitOfWork.GetRepository<IAvatarRepository>();
        repo.Create(avatar);
        await _unitOfWork.SaveChangesAsync();
        return new ServiceResult();
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
        user!.InCall = false;
        repo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResult();
    }

    public async Task<ServiceResult<AppUser>> RemoveCallHubConnectionId(string contextConnectionId)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var user = await repo.GetAll().FirstOrDefaultAsync(x => x.CallHubConnectionId == contextConnectionId);
        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("Remove CallHubConnectionId failed", $"user with username \"{contextConnectionId}\" not found."),
            };
            return new ServiceResult<AppUser>(errors);
        }

        user.CallHubConnectionId = null;
        user.InCall = false;
        repo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResult<AppUser>(user);
    }

    public async Task<ServiceResult<CallParticipants>> SetInCall(CallUsernamesDto callUsernamesDto, bool newValue)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var callInitiator = await repo.GetAll()
            .AsTracking()
            .FirstOrDefaultAsync(x => x.UserName == callUsernamesDto.callInitiatorUsername);
        if (callInitiator == null)
        {
            var error = new List<KeyValuePair<string, string>>
            {
                new("Call failed", $"User {callUsernamesDto.callInitiatorUsername} not found"),
            };
            return new ServiceResult<CallParticipants>(error);
        }

        var callReceiver = await repo.GetAll()
            .AsTracking()
            .FirstOrDefaultAsync(x => x.UserName == callUsernamesDto.callReceiverUsername);
        if (callReceiver == null)
        {
            var error = new List<KeyValuePair<string, string>>
            {
                new("Call failed", $"User {callUsernamesDto.callReceiverUsername} not found"),
            };
            return new ServiceResult<CallParticipants>(error);
        }

        if (newValue)
        {
            if (callInitiator.InCall == newValue)
            {
                var error = new List<KeyValuePair<string, string>>
                {
                    new("Call failed", $"{callInitiator} InCall has same value"),
                };
                return new ServiceResult<CallParticipants>(error);
            }

            if (callReceiver.InCall == newValue)
            {
                var error = new List<KeyValuePair<string, string>>
                {
                    new("Call failed", $"{callReceiver} InCall has same value"),
                };
                return new ServiceResult<CallParticipants>(error);
            }
        }

        callInitiator.InCall = newValue;
        callReceiver.InCall = newValue;

        await _unitOfWork.SaveChangesAsync();

        var callParticipants = new CallParticipants
        {
            CallInitiator = callInitiator,
            CallReceiver = callReceiver,
        };
        return new ServiceResult<CallParticipants>(callParticipants);
    }

    public async Task<ServiceResult<AppUser>> SetInCallByConnectionId(string contextConnectionId, bool newValue)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();

        var user = await repo.GetAll()
            .AsTracking()
            .FirstOrDefaultAsync(x => x.CallHubConnectionId == contextConnectionId);
        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("Set In Call failed", "user not found"),
            };
            return new ServiceResult<AppUser>(errors);
        }

        if (user.InCall == newValue)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("Set In Call failed", $"{user.UserName} InCall has same value"),
            };
            return new ServiceResult<AppUser>(errors);
        }
        
        user.InCall = newValue;
        await _unitOfWork.SaveChangesAsync();
        
        return new ServiceResult<AppUser>(user);
    }
}