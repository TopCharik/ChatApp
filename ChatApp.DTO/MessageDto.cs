namespace ChatApp.DTO;

public class MessageDto
{
    public string MessageText { get; set; }

    public DateTime DateSent { get; set; }

    public ParticipationDto Participation { get; set; }
}