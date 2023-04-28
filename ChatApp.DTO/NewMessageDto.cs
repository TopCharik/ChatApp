using System.ComponentModel.DataAnnotations;

namespace ChatApp.DTO;

public class NewMessageDto
{
    [Required]
    public string MessageText { get; set; }
}