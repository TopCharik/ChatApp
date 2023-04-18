using ChatApp.DAL.App.AppContext;

namespace ChatApp.DAL.App.Interfaces;

public interface EfContaxtable
{
    void SetContext(AppDbContext context);
}