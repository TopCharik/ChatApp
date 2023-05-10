using ChatApp.BLL;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChatApp.IntegrationTests;

[TestFixture]
public class MessagesServiceTests : BaseServiceTestFixture
{
    private IMessageService _messageService;
    
    public MessagesServiceTests()
    {
        _messageService = _serviceProvider.GetService<IMessageService>() 
                          ?? throw new InvalidOperationException("IMessageService is not registered in BaseServiceTestFixture");
    }

    [Test]
    public async Task ExampleTest()
    {
        await DbHelpers.ClearDb(_dbContext);
    }
}