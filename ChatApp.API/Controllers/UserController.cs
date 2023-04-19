using AutoMapper;
using ChatApp.BLL;
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

    public UserController(IUserService UserService, IMapper mapper)
    {
        _userService = UserService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<AppUserDto>>> GetUsers([FromQuery] AppUserQueryParams parameters)
    {
        var userParameters = _mapper.Map<AppUserParameters>(parameters);
        var users = await _userService.GetUsersAsync(userParameters);


        return _mapper.Map<PagedResponseDto<AppUserDto>>(users);
    }
}