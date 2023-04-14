using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs;

public class FooDto
{
    [Required]
    [MaxLength(100)]
    public string Message { get; set; }
}   