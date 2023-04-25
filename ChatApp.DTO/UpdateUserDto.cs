using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTO;

public class UpdateUserDto
{
    [EmailAddress]
    public string Email { get; set; }

    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
}