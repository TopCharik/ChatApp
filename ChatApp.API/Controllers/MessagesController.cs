using AutoMapper;
using ChatApp.API.Hubs;
using ChatApp.BLL;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IHubContext<ConversationsHub> _conversationHubContext;
    private readonly IValidator<NewMessageDto> _newMessageValidator;

    public MessagesController(
        IMessageService messageService,
        IUserService userService,
        IMapper mapper,
        IHubContext<ConversationsHub> conversationHubContext,
        IValidator<NewMessageDto> newMessageValidator 
        )
    {
        _messageService = messageService;
        _userService = userService;
        _mapper = mapper;
        _conversationHubContext = conversationHubContext;
        _newMessageValidator = newMessageValidator;
    }
    
    [HttpGet]
    [Route("{conversationId}")]
    public async Task<ActionResult<List<MessageDto>>> 
        GetMessages(int conversationId, [FromQuery] MessageQueryParametersDto? messageQueryParametersDto)
    {
        var username = HttpContext.User.Identity!.Name!;
        var user = await _userService.GetUserByUsernameAsync(username);
        if (!user.Succeeded)
        {
            return BadRequest(new ApiError(400, user.Errors));
        }

        var messageParameters = _mapper.Map<MessageParameters>(messageQueryParametersDto);
        var messages = await _messageService.GetMessages(conversationId, user.Value.Id, messageParameters);
        if (!messages.Succeeded)
        {
            return Unauthorized(new ApiError(401, messages.Errors));
        }
        
        return _mapper.Map<List<MessageDto>>(messages.Value);
    }

    [HttpPost]
    [Route("{conversationId}")]
    public async Task<ActionResult> SendMessage(int conversationId, [FromBody] NewMessageDto newMessageDto)
    {
        var validationResult = await _newMessageValidator.ValidateAsync(newMessageDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
            return BadRequest(new ApiError(400, errors));
        }
        
        var username = HttpContext.User.Identity!.Name!;
        var sender = await _userService.GetUserByUsernameAsync(username);
        if (!sender.Succeeded)
        {
            return BadRequest(new ApiError(400, sender.Errors));
        }
        
        var message = _mapper.Map<Message>(newMessageDto);
        var result = await _messageService.SendMessage(message, sender.Value!.Id, conversationId);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiError(400, result.Errors));
        }
        
        await _conversationHubContext.Clients.All.SendAsync($"{conversationId}/NewMessage");
        return Ok();
    }
}