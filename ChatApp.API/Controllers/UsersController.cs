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
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
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
    public async Task<ActionResult<AppUserDto>> GetUser(string username)
    {
        var result = await _userService.GetUserByUsername(username);
        
        if (!result.Succeeded)
        {
            return NotFound(new ApiError(404, result.Errors));
        }
        
        var userDetailsDto = _mapper.Map<AppUserDto>(result.Value);
        
        return userDetailsDto;
    }
}