using System.Text.Json.Serialization;

namespace ChatApp.API.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
}