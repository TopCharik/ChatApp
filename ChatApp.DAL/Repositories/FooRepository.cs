using ChatApp.Core.Entities.FooAggregate;
using ChatApp.DAL.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.Repositories;

public class FooRepository : BaseRepository<Foo>, IFooRepository
{
    public FooRepository(AppDbContext context) : base(context) { }

    public async Task<List<Foo>> GetFilteredAsync(string filter)
    {
        return await _dbSet.Where(foo => foo.TextMessage.Contains(filter)).ToListAsync();
    }
}