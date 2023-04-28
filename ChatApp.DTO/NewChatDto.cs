using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTO;

public class NewChatDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string ChatLink { get; set; }
    [Required]
    public bool IsPrivate { get; set; }
    public List<string> Usernames { get; set; }
}