namespace ChatApp.DTO;

public class ChatInfoQueryParams : PagingQueryParams
{
    public string? Search { get; set; }
    public string? Title { get; set; }
    public string? ChatLink { get; set; }
    public string? SortField { get; set; }
    public string? OrderBy { get; set; }
}