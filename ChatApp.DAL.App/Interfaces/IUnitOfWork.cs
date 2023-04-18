namespace ChatApp.DAL.App.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<T> GetBaseRepository<T>() where T : class;

    public TRepository GetRepository<TRepository, TEntity>()
        where TEntity : class 
        where TRepository : IBaseRepository<TEntity>;

    Task<int> SaveChangesAsync();
}