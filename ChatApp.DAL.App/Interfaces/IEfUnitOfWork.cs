namespace ChatApp.DAL.App.Interfaces;

public interface IEfUnitOfWork : IUnitOfWork
{
    public TRepository GetRepository<TRepository>() where TRepository : IEfContaxtable;
}