using ChatApp.Core.Helpers;

namespace ChatApp.Core.Entities.AppUserAggregate;

public class AppUserParameters : PagingParameters
{
    public string? Search { get; set; }
    public string? RealName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? SortField { get; set; }
    public string? OrderBy { get; set; }
}