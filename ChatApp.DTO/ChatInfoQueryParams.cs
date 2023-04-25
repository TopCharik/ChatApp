namespace ChatApp.DTO;

public class ChatInfoQueryParams : PagingQueryParams
{
    public string? Title { get; set; }
    public string? ChatLink { get; set; }
}