using AutoMapper;
using ChatApp.API.Helpers;
using ChatApp.BLL;
using ChatApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ParticipationController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public ParticipationController(
        IChatService chatService,
        IUserService userService,
        IMapper mapper
        )
    {
        _chatService = chatService;
        _userService = userService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{chatLink}")]
    public async Task<ActionResult<ConversationParticipationDto>> GetParticipationByChatLink(string chatLink)
    {
        var username = HttpContext.User.Identity!.Name!;
        var user = await _userService.GetUserByUsernameAsync(username);
        if (!user.Succeeded)
        {
            return BadRequest(new ApiError(400, user.Errors));
        }
        
        var conversation = await _chatService.GetParticipationByChatLink(chatLink, user.Value!.Id);
        if (!conversation.Succeeded)
        {
            return BadRequest(new ApiError(400, conversation.Errors));
        }
        
        return _mapper.Map<ConversationParticipationDto>(conversation.Value);
    }

    [HttpPost]
    [Route("{chatLink}")]
    public async Task<ActionResult> JoinChat(string chatLink)
    {
        var username = HttpContext.User.Identity!.Name!;
        var user = await _userService.GetUserByUsernameAsync(username);
        if (!user.Succeeded)
        {
            return BadRequest(new ApiError(400, user.Errors));
        }

        var participation = ParticipationFactory.DefaultChatMember(user.Value!.Id);

        var result = await _chatService.JoinChat(chatLink, participation);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiError(400, result.Errors));
        }

        return Ok();
    }
    
    [HttpDelete]
    [Route("{chatLink}")]
    public async Task<ActionResult<ConversationDto>> LeaveChat(string chatLink)
    {
        var username = HttpContext.User.Identity!.Name!;
        var user = await _userService.GetUserByUsernameAsync(username);
        if (!user.Succeeded)
        {
            return BadRequest(new ApiError(400, user.Errors));
        }

        var result = await _chatService.LeaveChat(chatLink, user.Value!.Id);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiError(400, result.Errors));
        }

        return Ok();
    }

}