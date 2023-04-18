namespace ChatApp.DAL.App.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<T> GetBaseRepository<T>() where T : class;

    Task<int> SaveChangesAsync();
}