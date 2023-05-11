using ChatApp.BLL;
using ChatApp.DAL.App.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChatApp.IntegrationTests;

public class UserServiceTests : BaseServiceTest
{
    private IUserService _userService;
    
    public UserServiceTests()
    {
        var unitOfWork = _serviceProvider.GetService<IUnitOfWork>() 
                         ?? throw new InvalidOperationException("IUnitOfWork is not registered in BaseServiceTestFixture");
        _userService = new UserService(unitOfWork);
    }
    
    
    [Test]
    public async Task ExampleTest()
    {
        await DbHelpers.ClearDb(_dbContext);
    }
}