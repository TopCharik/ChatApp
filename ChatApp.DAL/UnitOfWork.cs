using ChatApp.Core.Entities.FooAggregate;
using ChatApp.Core.Interfaces;
using ChatApp.DAL.AppContext;
using ChatApp.DAL.Repositories;

namespace ChatApp.DAL;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private bool _isDisposed;

    public IFooRepository Foos { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Foos = new FooRepository(_context);
    }

    public async Task<int> CompleteAsync()
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