using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTO;

public class RegisterAppUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    [Required]
    public string Password { get; set; }
}