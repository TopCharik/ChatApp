using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTO;

public class RegisterAppUserDto
{
    public string Email { get; set; }
    
    public string UserName { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? PhoneNumber { get; set; }

    public DateTime? Birthday { get; set; }

    public string Password { get; set; }
}