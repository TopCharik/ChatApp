using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTO;

public class UserRegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string RealName { get; set; }
    
    [Required]
    public string Password { get; set; }
}