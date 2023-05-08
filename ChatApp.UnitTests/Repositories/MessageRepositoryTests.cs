using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.Repositories;
using NUnit.Framework;

namespace ChatApp.UnitTests.Repositories;

public class MessageRepositoryTests
{
    private MessageRepository _repository;
    private bool _firstSetUp = true;
    
    [SetUp]
    public async Task SetUp()
    {
        if (_firstSetUp)
        {
            var context = await FakeData.GetDefaultAppDbContext();
            _repository = new MessageRepository(context);   
        }

        _firstSetUp = false;
    }
    
    [Test]
    public async Task GetMessagesAsync_WithExistingConversation_ReturnsAllMessages()
    {
        var conversationId = 1234565;
        var messageParameters = new MessageParameters();
        var expectedResult = new List<Message>
        {
            new()
            {
                Id = 12648,
                ParticipationId = 1, 
                MessageText = "First message",
                DateSent = new DateTime(638191744920000000),
            },
            new()
            {
                Id = 85678,
                ParticipationId = 2,
                MessageText = "Second message",
                DateSent = new DateTime(638191746120000000),
            },
            new()
            {
                Id = 1234565,
                ParticipationId = 1,
                MessageText = "Third message",
                DateSent = new DateTime(638191746200000000),
            },
        };


        await VerifyMessageListAsync(expectedResult, conversationId, messageParameters);
    }
    
    [Test]
    public async Task GetMessagesAsync_WithNonExistingConversation_ReturnsEmptyList()
    {
        var conversationId = -100;

        var result = await _repository.GetMessagesAsync(conversationId, new MessageParameters());

        Assert.True(result.Count == 0);
    }
    
        
    private async Task VerifyMessageListAsync(List<Message> expectedMessages, int conversationId, MessageParameters parameters)
    {

        var result = await _repository.GetMessagesAsync(conversationId, parameters);

        Assert.AreEqual(expectedMessages.Count, result.Count);
        Assert.True(expectedMessages.All(expectedMessage => result.FirstOrDefault(x => x.Id == expectedMessage.Id) != null));
    }
}