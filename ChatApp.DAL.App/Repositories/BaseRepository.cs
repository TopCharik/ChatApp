using System.Linq.Expressions;
using ChatApp.DAL.App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected DbContext _context;

    public BaseRepository(DbContext context)
    {
        _context = context;
    }
    
    public void SetContext(DbContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll()
    {
        return _context.Set<T>()
            .AsNoTracking();
    }

    public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>()
            .Where(expression)
            .AsNoTracking();
    }

    public void Create(T entity)
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
}