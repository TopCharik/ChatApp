using AutoMapper;
using ChatApp.BLL;
using ChatApp.Core.Entities;
using ChatApp.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public MessagesController(IMessageService messageService, IUserService userService, IMapper mapper)
    {
        _messageService = messageService;
        _userService = userService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{conversationId}")]
    public async Task<ActionResult<List<MessageDto>>> GetMessages(int conversationId)
    {
        var messages = await _messageService.GetMessages(conversationId);
        return _mapper.Map<List<MessageDto>>(messages);
    }

    [HttpPost]
    [Route("{conversationId}")]
    public async Task<ActionResult> SendMessage(int conversationId, [FromBody] NewMessageDto newMessageDto)
    {
        var username = HttpContext.User.Identity!.Name!;
        var sender = await _userService.GetUserByUsername(username);
        if (!sender.Succeeded)
        {
            return BadRequest(new ApiError(400, sender.Errors));
        }
        
        var message = _mapper.Map<Message>(newMessageDto);
        var result = await _messageService.SendMessage(message, sender.Value!.Id, conversationId);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
}