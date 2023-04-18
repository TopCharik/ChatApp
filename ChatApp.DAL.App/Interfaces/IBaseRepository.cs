using ChatApp.DAL.App.AppContext;

namespace ChatApp.DAL.App.Interfaces;

public interface IBaseRepository<T> where T : class
{ 
    Task<List<T>> GetAsync();
    Task<int> CountAsync();
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void SetContext(AppDbContext context);
}