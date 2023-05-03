using ChatApp.BLL;
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
        }

        await base.OnDisconnectedAsync(exception);
    }
    
}