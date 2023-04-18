namespace ChatApp.DAL.App.Interfaces;

public interface EfBaseRespository<T> : IBaseRepository<T>, EfContaxtable where T : class
{
}