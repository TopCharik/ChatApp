using ChatApp.Core.Entities.FooAggregate;

namespace ChatApp.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IFooRepository Foos { get; }

    Task<int> CompleteAsync();
}