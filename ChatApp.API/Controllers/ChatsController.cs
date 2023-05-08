using AutoMapper;
using ChatApp.API.Extensions;
using ChatApp.API.Helpers;
using ChatApp.API.Hubs;
using ChatApp.BLL;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatsController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IValidator<NewChatDto> _newChatValidator;
    private readonly IHubContext<ConversationsHub> _conversationsHubContext;

    public ChatsController(
        IChatService chatService,
        IMapper mapper,
        IUserService userService,
        IValidator<NewChatDto> newChatValidator,
        IHubContext<ConversationsHub> conversationsHubContext
            )
    {
        _chatService = chatService;
        _mapper = mapper;
        _userService = userService;
        _newChatValidator = newChatValidator;
        _conversationsHubContext = conversationsHubContext;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<ChatInfoDto>>> GetChats([FromQuery] ChatInfoQueryParams queryParams)
    {
        var parameters = _mapper.Map<ChatInfoParameters>(queryParams);
        var chats = await _chatService.GetChatsAsync(parameters);
        var chatsDto = _mapper.Map<PagedResponseDto<ChatInfoDto>>(chats);

        return chatsDto;
    }

    [HttpGet]
    [Route("{chatLink}")]
    public async Task<ActionResult<ConversationDto>> GetChatByChatLink(string chatLink)
    {
        var chat = await _chatService.GetChatByLink(chatLink);
        if (!chat.Succeeded)
        {
            return BadRequest(new ApiError(400, chat.Errors));
        }

        return _mapper.Map<ConversationDto>(chat.Value);
    }

    [HttpPost]
    public async Task<ActionResult> CreateChat([FromBody] NewChatDto newChatDto)
    {
        var validationResult = await _newChatValidator.ValidateAsync(newChatDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        var ownerName = HttpContext.User.Identity!.Name!;

        var ownerId = await _userService.GetUserByUsernameAsync(ownerName);
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

    [HttpPost]
    [Route("Avatar/")]
    public async Task<ActionResult> AddAvatar(NewChatAvatarDto newAvatarDto)
    {
        var claimsUsername = HttpContext.User.Identity!.Name!;
        var claimsUser = await _userService.GetUserByUsernameAsync(claimsUsername);
        if (!claimsUser.Succeeded)
        {
            return BadRequest(new ApiError(400, claimsUser.Errors));
        }

        var avatar = _mapper.Map<Avatar>(newAvatarDto);

        var result = await _chatService.AddAvatar(avatar, newAvatarDto.ChatLink, claimsUser.Value.Id);
        if (result.Errors != null)
        {
            return BadRequest(new ApiError(400, result.Errors));
        }

        await _conversationsHubContext.Clients.All.SendAsync($"{result.Value!.Id}/ConversationInfoChanged");

        return Ok();
    }
}