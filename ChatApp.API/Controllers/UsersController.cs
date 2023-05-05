using AutoMapper;
using ChatApp.API.Extensions;
using ChatApp.API.Helpers;
using ChatApp.API.Hubs;
using ChatApp.BLL;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.Identity;
using ChatApp.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<ExtendedIdentityUser> _userManager;
    private readonly SignInManager<ExtendedIdentityUser> _signInManager;
    private readonly IHubContext<UsersHub> _usersHubContext;
    private readonly IHubContext<CallsHub> _callsHub;
    private readonly IJwtTokenBuilder _jwtTokenBuilder;
    private readonly IMapper _mapper;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterAppUserDto> _registerValidator;
    private readonly IValidator<EditUserDto> _updateUserValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
    private readonly IValidator<NewUsernameDto> _newUsernameValidator;

    public UsersController(
        IUserService userService,
        UserManager<ExtendedIdentityUser> userManager,
        SignInManager<ExtendedIdentityUser> signInManager,
        IHubContext<UsersHub> usersHubContext,
        IHubContext<CallsHub> callsHub,
        IJwtTokenBuilder jwtTokenBuilder,
        IMapper mapper,
        IValidator<LoginDto> loginValidator,
        IValidator<RegisterAppUserDto> registerValidator,
        IValidator<EditUserDto> updateUserValidator,
        IValidator<ChangePasswordDto> changePasswordValidator,
        IValidator<NewUsernameDto> newUsernameValidator
    )
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _usersHubContext = usersHubContext;
        _callsHub = callsHub;
        _jwtTokenBuilder = jwtTokenBuilder;
        _mapper = mapper;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _updateUserValidator = updateUserValidator;
        _changePasswordValidator = changePasswordValidator;
        _newUsernameValidator = newUsernameValidator;
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
        var result = await _userService.GetUserByUsernameAsync(username);

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
    public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("Login failed", "Wrong password or username."),
            };
            return Unauthorized(new ApiError(401, errors));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("Login failed", "Wrong password or username."),
            };
            return Unauthorized(new ApiError(401, errors));
        }

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Register(RegisterAppUserDto registerDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        var user = _mapper.Map<ExtendedIdentityUser>(registerDto);

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPatch]
    [Route("change-password")]
    public async Task<ActionResult<string>> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(changePasswordDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        var name = HttpContext.User.Identity?.Name;
        var user = await _userManager.FindByNameAsync(name);
        if (user == null)
        {
            return Unauthorized();
        }

        var result =
            await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPatch]
    [Route("update-user/{username}")]
    public async Task<ActionResult<AppUserDto>> UpdateUser(EditUserDto editUserDto, string username)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(editUserDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        if (!string.Equals(username, HttpContext.User.Identity?.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("User update failed", "You can't change this user information."),
            };
            return Unauthorized(new ApiError(401, errors));
        }

        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("User update failed", "User with this username is not found."),
            };
            return BadRequest(new ApiError(400, errors));
        }

        user.UserName = username;
        user.FirstName = editUserDto.FirstName;
        user.LastName = editUserDto.LastName;
        user.Email = editUserDto.Email;
        user.Birthday = editUserDto.Birthday;
        user.PhoneNumber = editUserDto.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        await _usersHubContext.Clients.All.SendAsync($"{username.ToLower()}/UserInfoChanged");
        return _mapper.Map<AppUserDto>(user);
    }

    [HttpPatch]
    [Route("change-username/{username}")]
    public async Task<ActionResult<string>> ChangeUsername(string username, [FromBody] NewUsernameDto newUsernameDto)
    {
        var validationResult = await _newUsernameValidator.ValidateAsync(newUsernameDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        if (!string.Equals(username, HttpContext.User.Identity?.Name, StringComparison.CurrentCultureIgnoreCase))
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("User update failed", "You can't change this user information."),
            };
            return Unauthorized(new ApiError(401, errors));
        }


        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            var errors = new List<KeyValuePair<string, string>>
            {
                new("Username change failed", "User with this username is not registered."),
            };
            return BadRequest(new ApiError(400, errors));
        }

        user.UserName = newUsernameDto.NewUsername;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToKeyValuePairs();
            return BadRequest(new ApiError(400, errors));
        }

        await _usersHubContext.Clients.All.SendAsync($"{username.ToLower()}/UsernameChanged", newUsernameDto.NewUsername);
        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPost]
    [Route("call/")]
    public async Task<ActionResult> CallUser(CallUsernamesDto callUsernamesDto)
    {
        var callParticipants = await _userService.SetInCall(callUsernamesDto, true);
        if (!callParticipants.Succeeded)
        {
            return BadRequest(new ApiError(400, callParticipants.Errors));
        }

        await _usersHubContext.Clients.All.SendAsync($"{callUsernamesDto.callInitiatorUsername.ToLower()}/UserInfoChanged");
        await _usersHubContext.Clients.All.SendAsync($"{callUsernamesDto.callReceiverUsername.ToLower()}/UserInfoChanged");
        
        await _callsHub.Clients
            .Client(callParticipants.Value.CallReceiver.CallHubConnectionId)
            .SendAsync("IncomingCall", callUsernamesDto);

        return Ok();
    }
}