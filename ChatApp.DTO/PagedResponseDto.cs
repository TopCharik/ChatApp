namespace ChatApp.DTO;

public class PagedResponseDto<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public bool HasPrevious { get; set; } = false;
    public bool HasNext { get; set; }= false;

    public List<T> Items { get; set; } = new();
}