using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;

namespace ChatApp.DAL.App.Repositories;

public interface IUserRepository : IBaseRepository<AppUser>, IContaxtable
{
    Task<PagedList<AppUser>> GetUsersAsync(AppUserParameters parameters);
}