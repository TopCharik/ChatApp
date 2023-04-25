using AutoMapper;
using ChatApp.BLL;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<AppUserDto>>> GetUsers([FromQuery] AppUserQueryParams parameters)
    {
        var userParameters = _mapper.Map<AppUserParameters>(parameters);
        var users = await _userService.GetUsersAsync(userParameters);


        return _mapper.Map<PagedResponseDto<AppUserDto>>(users);
    }
    
    [HttpGet]
    [Route("{username}")]
    public async Task<ActionResult<AppUserDto>> GetUsers(string username)
    {
        var userDetails = await _userService.GetUserByUsername(username);
        
        if (userDetails == null)
        {
            var errors = new Dictionary<string, string>();
            errors.Add("User not found", "User with this username is not registered.");
            return NotFound(new ApiError(404, errors));
        }
        
        var userDetailsDto = _mapper.Map<AppUserDto>(userDetails);
        
        return userDetailsDto;
    }
}