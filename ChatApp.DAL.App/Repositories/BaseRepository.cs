using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class BaseRepository<T> : EfBaseRespository<T> where T : class
{
    protected AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Set<T>().CountAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void SetContext(AppDbContext context)
    {
        _context = context;
    }
}