using ChatApp.BLL;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChatApp.IntegrationTests;

public class ChatServiceTests : BaseServiceTestFixture
{
    private IChatService _chatService;
    
    public ChatServiceTests()
    {
        _chatService = _serviceProvider.GetService<IChatService>() 
                       ?? throw new InvalidOperationException("IMessageService is not registered in BaseServiceTestFixture");
    }
    
    
    [Test]
    public async Task ExampleTest()
    {
        await DbHelpers.ClearDb(_dbContext);
    }
}