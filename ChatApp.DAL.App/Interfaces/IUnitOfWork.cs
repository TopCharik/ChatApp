namespace ChatApp.DAL.App.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<T> GetBaseRepository<T>() where T : class;

    public TRepository GetRepository<TRepository>() where TRepository : IEfContaxtable;

    Task<int> SaveChangesAsync();
}