using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Helpers;

public class ChunkList<T> : List<T>
{
    public int ChunkSize { get; private set; }
    public bool HasNext { get; set; }

    public ChunkList(List<T> items, int chunkSize, bool hasNext)
    {
        HasNext = hasNext;
        ChunkSize = chunkSize;
        AddRange(items);
    }
    
    public static async Task<ChunkList<T>> ToChunkList(IQueryable<T> query, int chunkSize)
    {
        var items = await query.Take(chunkSize + 1).ToListAsync();
        var hasNext = false;
        if (items.Count > chunkSize)
        {
            items.RemoveAt(items.Count - 1);
            hasNext = true;
        }

        return new ChunkList<T>(items, chunkSize, hasNext);
    }
}