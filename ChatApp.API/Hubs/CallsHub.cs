using ChatApp.BLL;
using ChatApp.DTO;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs;

public class CallsHub : Hub
{
    private readonly IUserService _userService;
    private readonly IHubContext<UsersHub> _usersHubContext;

    public CallsHub(IUserService userService, IHubContext<UsersHub> usersHubContext)
    {
        _userService = userService;
        _usersHubContext = usersHubContext;
    }

    public async Task Join(string username)
    {
        var result = await _userService.SetCallHubConnectionId(username, Context.ConnectionId);
        if (result.Succeeded)
        {
            await _usersHubContext.Clients.All.SendAsync($"{username.ToLower()}/UserInfoChanged");
        }
    }
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var result = await _userService.RemoveCallHubConnectionId(Context.ConnectionId);
        if (result.Succeeded)
        {
            await _usersHubContext.Clients.All.SendAsync($"{result.Value.UserName.ToLower()}/UserInfoChanged");
            await Clients.All.SendAsync($"{result.Value.UserName.ToLower()}/Disconnected");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task HangUp(CallUsernamesDto callUsernamesDto)
    {
        var result = await _userService.SetInCall(callUsernamesDto, false);
        if (result.Succeeded)
        {
            var connectionIds = new List<string>
            {
                result.Value.CallReceiver.CallHubConnectionId,
                result.Value.CallInitiator.CallHubConnectionId,
            };

            await Clients.Clients(connectionIds).SendAsync("CallEnded");
        }
    }

    public async Task AcceptCall(CallUsernamesDto callUsernamesDto, string peerJsId)
    {
        var callInitiator = await _userService.GetUserByUsernameAsync(callUsernamesDto.callInitiatorUsername);
        if (callInitiator is {Succeeded: true, Value: {InCall: true, CallHubConnectionId: not null}})
        {
            await Clients.Client(callInitiator.Value.CallHubConnectionId).SendAsync("CallAccepted", peerJsId);
        }
        else
        {
            HangUp(callUsernamesDto);
        }
    }
}