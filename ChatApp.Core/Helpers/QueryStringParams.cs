namespace ChatApp.Core.Helpers;

public abstract class PagingParameters
{
    private const int MaxPageSize = 50;
    public int Page { get; set; } = 0;

    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}