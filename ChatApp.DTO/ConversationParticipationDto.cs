namespace ChatApp.DTO;

public class ConversationParticipationDto
{
    public int Id { get; set; }
    public ChatInfoDto? ChatInfo { get; set; }
    public ParticipationDto? Participation { get; set; }
}