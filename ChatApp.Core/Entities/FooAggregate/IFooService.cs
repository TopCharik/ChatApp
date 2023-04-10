namespace ChatApp.Core.Entities.FooAggregate;

public interface IFooService
{
    Task<int> CountAsync();
    Task<Foo?> GetByIdAsync(int id);
    Task<List<Foo>> GetAsync();
    Task<List<Foo>> GetFilteredAsync(string filter);
    Task<List<Foo>> AddAsync(Foo entity);
    Task<List<Foo>> UpdateAsync(Foo entity);
    Task<List<Foo>> DeleteAsync(Foo entity);
}