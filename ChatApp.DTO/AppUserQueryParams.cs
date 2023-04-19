namespace ChatApp.DTO;

public class AppUserQueryParams : PagingQueryParams
{
    public string? Search { get; set; }
    public string? RealName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? SortField { get; set; }
    public string? OrderBy { get; set; }
}