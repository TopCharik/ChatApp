using AutoMapper;
using ChatApp.API.Helpers;
using ChatApp.BLL;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.Identity;
using ChatApp.DTO;
using ChatApp.DTO.Validators;
using FluentValidation;
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
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterAppUserDto> _registerValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordValidator;
    private readonly IValidator<NewUsernameDto> _newUsernameValidator;

    public UsersController(
        IUserService userService,
        UserManager<ExtendedIdentityUser> userManager,
        SignInManager<ExtendedIdentityUser> signInManager,
        IJwtTokenBuilder jwtTokenBuilder,
        IMapper mapper,
        IValidator<LoginDto> loginValidator,
        IValidator<RegisterAppUserDto> registerValidator,
        IValidator<UpdateUserDto> updateUserValidator,
        IValidator<ChangePasswordDto> changePasswordValidator,
        IValidator<NewUsernameDto> newUsernameValidator
    )
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
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
    public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
            return BadRequest(new ApiError(400, errors));
        }

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
    [AllowAnonymous]
    public async Task<ActionResult<string>> Register(RegisterAppUserDto registerDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
            return BadRequest(new ApiError(400, errors));
        }

        var user = _mapper.Map<ExtendedIdentityUser>(registerDto);

        var result = await _userManager.CreateAsync(user, registerDto.Password);
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
    public async Task<ActionResult<string>> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(changePasswordDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
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
            var errors = result.Errors.ToDictionary(error => error.Code, error => error.Description);
            return BadRequest(new ApiError(400, errors));
        }

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPatch]
    [Authorize]
    [Route("update-user/{username}")]
    public async Task<ActionResult<AppUserDto>> UpdateUser(UpdateUserDto updateUserDto, string username)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(updateUserDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
            return BadRequest(new ApiError(400, errors));
        }

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
            errors.Add("User update failed", "User with this username is not found.");
            return BadRequest(new ApiError(400, errors));
        }

        user.UserName = username;
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.PhoneNumber = updateUserDto.PhoneNumber;

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
    public async Task<ActionResult<string>> ChangeUsername(string username, [FromBody] NewUsernameDto newUsernameDto)
    {
        var validationResult = await _newUsernameValidator.ValidateAsync(newUsernameDto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.ToDictionary(k => k.PropertyName, v => v.ErrorMessage);
            return BadRequest(new ApiError(400, errors));
        }

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

        user.UserName = newUsernameDto.NewUsername;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(error => error.Code, error => error.Description);
            return BadRequest(new ApiError(400, errors));
        }


        return _jwtTokenBuilder.CreateToken(user);
    }
}