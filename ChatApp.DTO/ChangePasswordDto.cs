using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.Controllers;

public class ChangePasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}