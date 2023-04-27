using AutoMapper;
using ChatApp.API.Helpers;
using ChatApp.BLL;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.Identity;
using ChatApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<ExtendedIdentityUser> _userManager;
    private readonly SignInManager<ExtendedIdentityUser> _signInManager;
    private readonly IJwtTokenBuilder _jwtTokenBuilder;
    private readonly IMapper _mapper;

    public UsersController(
        IUserService userService,
        UserManager<ExtendedIdentityUser> userManager,
        SignInManager<ExtendedIdentityUser> signInManager,
        IJwtTokenBuilder jwtTokenBuilder,
        IMapper mapper)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenBuilder = jwtTokenBuilder;
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
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null)
        {
            var errors = new Dictionary<string, string>();
            errors.Add("Login failed", "Wrong password or username.");
            return Unauthorized(new ApiError(401, errors));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, string>();
            errors.Add("Login failed", "Wrong password or username.");
            return Unauthorized(new ApiError(401, errors));
        }

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<string>> Register(RegisterAppUserDto dto)
    {
        var user = _mapper.Map<ExtendedIdentityUser>(dto);

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(error => error.Code, error => error.Description);
            return BadRequest(new ApiError(400, errors));
        }

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPatch]
    [Authorize]
    [Route("change-password")]
    public async Task<ActionResult<string>> Register(ChangePasswordDto changePasswordDto)
    {
        var name = HttpContext.User.Identity?.Name;
        var user = await _userManager.FindByNameAsync(name);
        if (user == null)
        {
            return BadRequest();
        }

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(error => error.Code, error => error.Description);
            return BadRequest(new ApiError(400, errors));
        }

        return _jwtTokenBuilder.CreateToken(user);
    }
    
    [HttpPatch]
    [Authorize]
    [Route("update-user/{username}")]
    public async Task<ActionResult<AppUserDto>> Register(UpdateUserDto appUserDto, string username)
    {
        if (!string.Equals(username, HttpContext.User.Identity?.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            var errors = new Dictionary<string, string>();
            errors.Add("User update failed", "You can't change this user information.");
            return Unauthorized(new ApiError(401, errors));
        }
        
        var user = await _userManager.FindByNameAsync(username);
        
        if (user == null)
        {
            var errors = new Dictionary<string, string>();
            errors.Add("User update failed", "User with this username is not registered.");
            return BadRequest(new ApiError(400, errors));
        }

        user.UserName = username;
        user.FirstName = appUserDto.FirstName;
        user.LastName = appUserDto.LastName;
        user.Email = appUserDto.Email;
        user.PhoneNumber = appUserDto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(error => error.Code, error => error.Description);
            return BadRequest(new ApiError(400, errors));
        }
        
        return _mapper.Map<AppUserDto>(user);
    }
    
    [HttpPatch]
    [Authorize]
    [Route("change-username/{username}")]
    public async Task<ActionResult<string>> ChangeUsername(string username,[FromBody] string newUsername)
    {
        if (!string.Equals(username, HttpContext.User.Identity?.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            var errors = new Dictionary<string, string>();
            errors.Add("User update failed", "You can't change this user information.");
            return Unauthorized(new ApiError(401, errors));
        }

        
        var user = await _userManager.FindByNameAsync(username);
        
        if (user == null)
        {
            var errors = new Dictionary<string, string>();
            errors.Add("Username change failed", "User with this username is not registered.");
            return BadRequest(new ApiError(400, errors));
        }
        
        user.UserName = newUsername;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(error => error.Code, error => error.Description);
            return BadRequest(new ApiError(400, errors));
        }
        
        
        
        return _jwtTokenBuilder.CreateToken(user);
    }
}