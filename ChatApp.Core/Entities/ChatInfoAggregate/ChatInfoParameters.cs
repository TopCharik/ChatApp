using ChatApp.Core.Helpers;

namespace ChatApp.Core.Entities.ChatInfoAggregate;

public class ChatInfoParameters : PagingParameters
{
    public string? Search { get; set; }
    
    public string? Title { get; set; }

    public string? ChatLink { get; set; }
    
    public ICollection<Avatar> Avatars  { get; set; }

    public Conversation? Conversation { get; set; }
}