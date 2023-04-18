using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.DAL.App;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, object> _repositories = new();
    private bool _isDisposed;
    

    public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IBaseRepository<T> GetBaseRepository<T>() where T : class
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(BaseRepository<>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(T)),
                _context
                ) ?? throw new ArgumentException($"Can't make repository from {type}");
            
            _repositories.Add(type, repositoryInstance);
        }

        return (IBaseRepository<T>) _repositories[type];
    }
    
    public TRepository GetRepository<TRepository, TEntity>() 
        where TEntity : class
        where TRepository : IBaseRepository<TEntity>
    {
        var type = typeof(TRepository);

        if (!_repositories.ContainsKey(type))
        {
            var repository =  _serviceProvider.GetService<TRepository>() 
                              ?? throw new ArgumentException($"{type.Name} is not registered in DI container.");
            repository.SetContext(_context);
            _repositories.Add(type, repository);
        }
        
        return (TRepository) _repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
            if (disposing)
                _context.Dispose();
        _isDisposed = true;
    }
}