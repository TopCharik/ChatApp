using ChatApp.Core.Entities;
using ChatApp.Core.Entities.FooAggregate;

namespace ChatApp.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IFooRepository Foos { get; }
    public IBaseRepository<T> GetRepository<T>() where T : BaseEntity;

    Task<int> SaveChangesAsync();
}