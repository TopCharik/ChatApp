using System.Linq.Expressions;

namespace ChatApp.DAL.App.Interfaces;

public interface IBaseRepository<T> : IContaxtable where T : class
{
    IQueryable<T> GetAll();
    IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}