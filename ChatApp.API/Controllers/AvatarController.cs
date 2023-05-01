using AutoMapper;
using ChatApp.API.Hubs;
using ChatApp.BLL;
using ChatApp.Core.Entities;
using ChatApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AvatarController : ControllerBase
{
    private readonly IAvatarService _avatarService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IHubContext<ConversationsHub> _hubContext;

    public AvatarController(
        IAvatarService avatarService,
        IMapper mapper,
        IUserService userService,
        IHubContext<ConversationsHub> hubContext
    )
    {
        _avatarService = avatarService;
        _mapper = mapper;
        _userService = userService;
        _hubContext = hubContext;
    }


    [HttpPost]
    public async Task<ActionResult> AddAvatar(NewAvatarDto newAvatarDto)
    {
        if (newAvatarDto.Username == null && newAvatarDto.ChatLink == null)
        {
            var errors = new Dictionary<string, string>
            {
                {"Avatar upload failed", "can't add avatar both for user and chat."},
            };
            return BadRequest(new ApiError(400, errors));
        }
        if (newAvatarDto.Username != null && newAvatarDto.ChatLink != null)
        {
            var errors = new Dictionary<string, string>
            {
                {"Avatar upload failed", "Username or ChatLink is required."},
            };
            return BadRequest(new ApiError(400, errors));
        }
        
        var claimsUsername = HttpContext.User.Identity!.Name!;
        var claimsUser = await _userService.GetUserByUsername(claimsUsername);
        if (!claimsUser.Succeeded)
        {
            return BadRequest(new ApiError(400, claimsUser.Errors));
        }

        var avatar = _mapper.Map<Avatar>(newAvatarDto);
        
        if (newAvatarDto.Username != null)
        {
            if (claimsUser.Value.UserName != newAvatarDto.Username)
            {
                var errors = new Dictionary<string, string>
                {
                    {"Avatar upload failed", "You don't have permission for uploading avatar to this user"},
                };
                return Unauthorized(new ApiError(401, errors));
            }

            avatar.UserId = claimsUser.Value.Id;
            var result = await _avatarService.AddUserAvatar(avatar);
            if (result.Errors != null)
            {
                return BadRequest(new ApiError(400, result.Errors));
            }
        }
        
        if (newAvatarDto.ChatLink != null)
        {
            var result = await _avatarService.AddChatAvatar(avatar, newAvatarDto.ChatLink, claimsUser.Value.Id);
            if (result.Errors != null)
            {
                return BadRequest(new ApiError(400, result.Errors));
            }
            await _hubContext.Clients.All.SendAsync($"{result.Value!.Id}/ConversationInfoChanged");
        }

        return Ok();
    }
}