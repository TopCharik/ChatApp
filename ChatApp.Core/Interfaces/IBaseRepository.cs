using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{ 
    Task<List<T>> GetAsync();
    Task<int> CountAsync();
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}