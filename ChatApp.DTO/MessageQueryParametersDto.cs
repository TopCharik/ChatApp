namespace ChatApp.DTO;

public class MessageQueryParametersDto
{
    public DateTime? Before { get; set; }
    public DateTime? After { get; set; }
}