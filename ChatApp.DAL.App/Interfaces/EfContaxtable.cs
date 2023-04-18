using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Interfaces;

public interface IEfContaxtable
{
    void SetContext(DbContext context);
}