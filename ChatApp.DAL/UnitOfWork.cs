using ChatApp.Core.Entities;
using ChatApp.Core.Entities.FooAggregate;
using ChatApp.Core.Interfaces;
using ChatApp.DAL.AppContext;
using ChatApp.DAL.Repositories;

namespace ChatApp.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private IFooRepository? _fooRepository;
    private bool _isDisposed;

    public IFooRepository Foos
    {
        get
        {
            _fooRepository ??= new FooRepository(_context);
            return _fooRepository;
        }
    }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IBaseRepository<T> GetRepository<T>() where T : BaseEntity
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(BaseRepository<>);
            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(T)),
                _context
                );
            
            _repositories.Add(type, repositoryInstance);
        }

        return (IBaseRepository<T>) _repositories[type];
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