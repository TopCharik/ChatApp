using ChatApp.Core.Interfaces;

namespace ChatApp.Core.Entities.FooAggregate;

public interface IFooRepository : IBaseRepository<Foo>
{
    Task<List<Foo>> GetFilteredAsync(string filter);
}