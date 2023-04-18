namespace ChatApp.DAL.App.Interfaces;

public interface EfBaseRespository<T> : IBaseRepository<T>, IEfContaxtable where T : class
{
}