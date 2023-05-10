using ChatApp.BLL;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChatApp.IntegrationTests;

public class UserServiceTests : BaseServiceTestFixture
{
    private IUserService _userService;
    
    public UserServiceTests()
    {
        _userService = _serviceProvider.GetService<IUserService>() 
                       ?? throw new InvalidOperationException("IMessageService is not registered in BaseServiceTestFixture");
    }
    
    
    [Test]
    public async Task ExampleTest()
    {
        await DbHelpers.ClearDb(_dbContext);
    }
}