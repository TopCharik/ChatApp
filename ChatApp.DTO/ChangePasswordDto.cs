using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.Controllers;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
}