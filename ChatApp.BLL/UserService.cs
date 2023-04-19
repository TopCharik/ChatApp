using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;

namespace ChatApp.BLL;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedList<AppUser>> GetUsersAsync(AppUserParameters parameters)
    {
        var repo = _unitOfWork.GetRepository<IUserRepository>();

        return await repo.GetUsersAsync(parameters);
    }
}