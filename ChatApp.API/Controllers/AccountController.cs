using ChatApp.API.DTOs;
using ChatApp.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtTokenService jwtTokenService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null)
        {
            return Unauthorized("Wrong password or/and user name.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Wrong password or/and user name.");
        }

        return new UserDto
        {
            Email = user.Email,
            UserName = user.UserName,
            Token = _jwtTokenService.CreateToken(user),
        };
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<UserDto>> Register(UserRegisterDto registerDto)
    {
        var user = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.UserName,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return new UserDto
        {
            Email = user.Email,
            UserName = user.UserName,
            Token = _jwtTokenService.CreateToken(user),
        };
    }

    [HttpPost]
    [Route("change-password")]
    public async Task<ActionResult<UserDto>> Register(ChangePasswordDto changePasswordDto)
    {
        var user = await _userManager.FindByNameAsync(changePasswordDto.UserName);
        if (user == null || user.Email != changePasswordDto.Email)
        {
            return BadRequest("Wrong email or/and user name.");
        }

        

        var result = await _userManager.RemovePasswordAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        result = await _userManager.AddPasswordAsync(user, changePasswordDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return new UserDto
        {
            Email = user.Email,
            UserName = user.UserName,
            Token = _jwtTokenService.CreateToken(user),
        };
    }
}