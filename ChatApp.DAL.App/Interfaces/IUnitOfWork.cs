namespace ChatApp.DAL.App.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public TRepository GetRepository<TRepository>() where TRepository : IContaxtable;

    Task<int> SaveChangesAsync();
}