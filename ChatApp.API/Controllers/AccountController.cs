using AutoMapper;
using ChatApp.API.Helpers;
using ChatApp.DAL.Identity;
using ChatApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<ExtendedIdentityUser> _userManager;
    private readonly SignInManager<ExtendedIdentityUser> _signInManager;
    private readonly IJwtTokenBuilder _jwtTokenBuilder;
    private readonly IMapper _mapper;

    public AccountController(
        UserManager<ExtendedIdentityUser> userManager,
        SignInManager<ExtendedIdentityUser> signInManager,
        IJwtTokenBuilder jwtTokenBuilder,
        IMapper mapper
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenBuilder = jwtTokenBuilder;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
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

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<string>> Register(UserRegisterDto registerDto)
    {
        var user = _mapper.Map<ExtendedIdentityUser>(registerDto);

        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return _jwtTokenBuilder.CreateToken(user);
    }

    [HttpPost]
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
            return BadRequest(result.Errors);
        }

        return _jwtTokenBuilder.CreateToken(user);
    }
}