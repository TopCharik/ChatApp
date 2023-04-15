using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<T> GetRepository<T>() where T : BaseEntity;

    Task<int> SaveChangesAsync();
}