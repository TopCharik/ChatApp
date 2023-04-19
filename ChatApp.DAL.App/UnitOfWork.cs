using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.DAL.App;

public class EfUnitOfWork : IEfUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, object> _repositories = new();
    private bool _isDisposed;
    

    public EfUnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public TRepository GetRepository<TRepository>() where TRepository: IContaxtable
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