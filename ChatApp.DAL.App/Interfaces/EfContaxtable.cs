using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Interfaces;

public interface IContaxtable
{
    void SetContext(DbContext context);
}