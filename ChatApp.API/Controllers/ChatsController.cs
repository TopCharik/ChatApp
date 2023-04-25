using AutoMapper;
using ChatApp.API.Helpers;
using ChatApp.BLL;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.Identity;
using ChatApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ChatsController(
        IChatService chatService,
        IMapper mapper,
        IUserService userService
    )
    {
        _chatService = chatService;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ConversationDto>>> GetChats([FromQuery] ChatInfoQueryParams queryParams)
    {
        var parameters = _mapper.Map<ChatInfoParameters>(queryParams);
        var chats = await _chatService.GetChatsAsync(parameters);
        var chatsDto = _mapper.Map<PagedResponseDto<ConversationDto>>(chats);

        return chatsDto;
    }
    
    [HttpPost]
    [Route("create-chat")]
    public async Task<ActionResult> CreateChat([FromBody] NewChatDto newChatDto)
    {
        var ownerName = HttpContext.User.Identity!.Name!;
        
        var ownerId = await _userService.GetUserByUsername(ownerName);
        if (!ownerId.Succeeded)
        {
            return BadRequest(new ApiError(400, ownerId.Errors));
        }
        
        var userIds = await _userService.GetUserIdsByUsernames(newChatDto.Usernames);
        if (!userIds.Succeeded)
        {
            return BadRequest(new ApiError(400, userIds.Errors));
        }

        var newConversation = ConversationFactory.NewChat(newChatDto, ownerId.Value!.Id, userIds.Value!);

        var result = await _chatService.CreateNewChat(newConversation);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiError(400, result.Errors));
        }
        
        return Ok();
    }
}