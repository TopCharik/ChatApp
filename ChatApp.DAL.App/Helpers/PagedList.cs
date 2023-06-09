using ChatApp.Core.Entities.ChatInfoAggregate;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Helpers;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items.Skip(pageNumber * pageSize).Take(pageSize).ToList());
    }
    
    public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> query, int pageNumber, int pageSize)
    {
        var count = query.Count();
        var items = await query.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}