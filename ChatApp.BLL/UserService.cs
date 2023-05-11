using ChatApp.BLL.Helpers;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using ChatApp.DTO;

namespace ChatApp.BLL;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PagedList<AppUser>>> GetUsersAsync(AppUserParameters parameters)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        
        var result = await repo.GetUsersAsync(parameters);

        return new ServiceResult<PagedList<AppUser>>(result);
    }

    public async Task<ServiceResult<AppUser>> GetUserByUsernameAsync(string username)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var user = await repo.GetUserByUsernameAsync(username);
        if (user == null)
        {
            return new ServiceResult<AppUser>(UserServiceErrors.USER_NOT_FOUND_BY_USERNAME);
        }

        return new ServiceResult<AppUser>(user);
    }

    public async Task<ServiceResult<AppUser>> GetUserByConnectionId(string connectionId)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();

        var user = await repo.GetUserByConnectionIdAsync(connectionId);
        if (user == null)
        {
            return new ServiceResult<AppUser>(UserServiceErrors.USER_NOT_FOUND_BY_CONNECTIONID);
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
                return new ServiceResult<List<string>>(UserServiceErrors.USER_NOT_FOUND_BY_USERNAME);
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
        user.InCall = false;
        repo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResult();
    }

    public async Task<ServiceResult<AppUser>> RemoveCallHubConnectionId(string connectionId)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var user = await repo.GetUserByConnectionIdAsync(connectionId);
        if (user == null)
        {
            return new ServiceResult<AppUser>(UserServiceErrors.USER_NOT_FOUND_BY_CONNECTIONID);
        }

        user.CallHubConnectionId = null;
        user.InCall = false;
        repo.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResult<AppUser>(user);
    }

    public async Task<ServiceResult<CallParticipants>> SetInCallAsync(CallUsernamesDto callUsernamesDto, bool newValue)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();
        var callInitiator = await repo.GetUserByUsernameAsync(callUsernamesDto.callInitiatorUsername);
        var callReceiver = await repo.GetUserByUsernameAsync(callUsernamesDto.callReceiverUsername);
        
        if (callReceiver == null || callInitiator == null)
        {
            return new ServiceResult<CallParticipants>(UserServiceErrors.USER_NOT_FOUND_BY_USERNAME);
        }

        if (newValue)
        {
            if (callInitiator.InCall == newValue || callReceiver.InCall == newValue)
            {
                return new ServiceResult<CallParticipants>(UserServiceErrors.USER_IS_ALREADY_IN_CALL);
            }
        }

        callInitiator.InCall = newValue;
        callReceiver.InCall = newValue;
        
        repo.Update(callInitiator);
        repo.Update(callReceiver);

        await _unitOfWork.SaveChangesAsync();

        var callParticipants = new CallParticipants
        {
            CallInitiator = callInitiator,
            CallReceiver = callReceiver,
        };
        return new ServiceResult<CallParticipants>(callParticipants);
    }
}