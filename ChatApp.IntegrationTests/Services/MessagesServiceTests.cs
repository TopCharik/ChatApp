using ChatApp.BLL;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.Interfaces;
using ChatApp.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChatApp.IntegrationTests;

[TestFixture]
public class MessagesServiceTests : BaseServiceTest
{
    private readonly IMessageService _messageService;
    
    public MessagesServiceTests()
    {
        var unitOfWork = _serviceProvider.GetService<IUnitOfWork>() 
                         ?? throw new InvalidOperationException("IUnitOfWork is not registered in BaseServiceTestFixture");
        _messageService = new MessageService(unitOfWork);
    }

    [Test]
    public async Task GetMessages_WhenConversationNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var newConversation = ConversationsDataHelper.GenerateRandomChat();
        
        var parameters = new MessageParameters();
        var expectedError = MessageServiceErrors.CHAT_NOT_FOUND;

        //Act
        var result = await _messageService.GetMessages(newConversation.Id, user.Id, parameters);
        
        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task GetMessages_FromPrivateChatWhenUserNeverWasAParticipant_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        newChat.ChatInfo.IsPrivate = true;
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        var parameters = new MessageParameters();
        var expectedError = MessageServiceErrors.USER_CANT_READ_MESSAGES_FROM_THIS_CHAT;

        //Act
        var result = await _messageService.GetMessages(chat.Id, user.Id, parameters);
        
        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
        
    [Test]
    public async Task GetMessages_FromPrivateChatWhenUserWasAParticipantButLeft_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        newChat.ChatInfo.IsPrivate = true;
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = chat.Id;
        newParticipation.HasLeft = true;
        var participation = await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        
        var parameters = new MessageParameters();
        var expectedError = MessageServiceErrors.USER_CANT_READ_MESSAGES_FROM_THIS_CHAT;

        //Act
        var result = await _messageService.GetMessages(chat.Id, user.Id, parameters);
        
        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task GetMessages_FromPrivateChatWhenUserIsAParticipant_ReturnsMessages()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        newChat.ChatInfo.IsPrivate = true;
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = chat.Id;
        newParticipation.HasLeft = false;
        await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);

        var messages = await MessagesDataHelper.InsertRandomMessagesFromNewRandomUser(_userManager, _dbContext, chat.Id);
        
        var parameters = new MessageParameters();

        //Act
        var result = await _messageService.GetMessages(chat.Id, user.Id, parameters);
        
        //Assert
        Assert.IsTrue(result.Succeeded);
        Assert.IsTrue(messages.All(m => result.Value.Any(r => r.MessageText == m.MessageText)));
    }
    
    [Test]
    public async Task GetMessages_FromPublicChatWhenUserWasNotAParticipantBefore_ReturnsMessages()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        newChat.ChatInfo.IsPrivate = false;
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);

        var messages = await MessagesDataHelper.InsertRandomMessagesFromNewRandomUser(_userManager, _dbContext, chat.Id);
        
        var parameters = new MessageParameters();

        //Act
        var result = await _messageService.GetMessages(chat.Id, user.Id, parameters);
        
        //Assert
        Assert.IsTrue(result.Succeeded);
        Assert.IsTrue(messages.All(m => result.Value.Any(r => r.MessageText == m.MessageText)));
    }
        
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetMessages_FromPublicChatWhenUserWasAParticipant_ReturnsMessages(bool hasLeft)
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        newChat.ChatInfo.IsPrivate = false;
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = chat.Id;
        newParticipation.HasLeft = hasLeft;
        var participation = await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);

        var messages = await MessagesDataHelper.InsertRandomMessagesFromNewRandomUser(_userManager, _dbContext, chat.Id);
        
        var parameters = new MessageParameters();

        //Act
        var result = await _messageService.GetMessages(chat.Id, user.Id, parameters);
        
        //Assert
        Assert.IsTrue(result.Succeeded);
        Assert.IsTrue(messages.All(m => result.Value.Any(r => r.MessageText == m.MessageText)));
    }

    [Test]
    public async Task SendMessage_WithBothUserAndConversationNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var userId = Guid.NewGuid().ToString();
        var conversationId = new Random().Next(100, 100000);
        var participationId = new Random().Next(100, 100000);
        
        var expectedError = MessageServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION;
        var expectedMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        
        //Act
        var message = MessagesDataHelper.GenerateRandomMessage(participationId);
        var result = await _messageService.SendMessage(message, userId, conversationId);
        
        //Assert
        var actualMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(expectedMessagesCount, actualMessagesCount);
    }
        
    [Test]
    public async Task SendMessage_WithUserNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var userId = Guid.NewGuid().ToString();

        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        var participationId = new Random().Next(100, 100000);
        
        var expectedError = MessageServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION;
        var expectedMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);

        //Act
        var message = MessagesDataHelper.GenerateRandomMessage(participationId);
        var result = await _messageService.SendMessage(message, userId, chat.Id);
        
        //Assert
        var actualMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(expectedMessagesCount, actualMessagesCount);
    }
    
    [Test]
    public async Task SendMessage_WithConversationNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var conversationId = new Random().Next(100, 100000);
        var participationId = new Random().Next(100, 100000);
        var expectedError = MessageServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION;
        var expectedMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        
        //Act
        var message = MessagesDataHelper.GenerateRandomMessage(participationId);
        var result = await _messageService.SendMessage(message, user.Id, conversationId);
        
        //Assert
        var actualMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(expectedMessagesCount, actualMessagesCount);
    }
        
    [Test]
    public async Task SendMessage_WithParticipantMuted_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);

        var mutedUntil = DateTime.Today.AddYears(1);
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = chat.Id;
        newParticipation.MutedUntil = mutedUntil;
        var participation = await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        
        var expectedError = MessageServiceErrors.USER_IS_MUTED_UNTIL(mutedUntil);
        var expectedMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);

        //Act
        var message = MessagesDataHelper.GenerateRandomMessage(participation.Id);
        var result = await _messageService.SendMessage(message, user.Id, chat.Id);
        
        //Assert
        var actualMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(expectedMessagesCount, actualMessagesCount);
    }
            
    [Test]
    public async Task SendMessage_WithParticipantCantWriteMessages_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = chat.Id;
        newParticipation.CanWriteMessages = false;
        var participation = await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        
        var expectedError = MessageServiceErrors.USER_CANT_WRITE_MESSAGES_IN_THIS_CHAT;        
        var expectedMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);

        //Act
        var message = MessagesDataHelper.GenerateRandomMessage(participation.Id);
        var result = await _messageService.SendMessage(message, user.Id, chat.Id);
        
        //Assert
        var actualMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(expectedMessagesCount, actualMessagesCount);
    }
                
    [Test]
    public async Task SendMessage_WhenUserWasAParticipantButLeft_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = chat.Id;
        newParticipation.HasLeft = true;
        var participation = await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        
        var expectedError = MessageServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION;        
        var expectedMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);

        //Act
        var message = MessagesDataHelper.GenerateRandomMessage(participation.Id);
        var result = await _messageService.SendMessage(message, user.Id, chat.Id);
        
        //Assert
        var actualMessagesCount = await MessagesDataHelper.GetAllMessagesCountAsync(_dbContext);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(expectedMessagesCount, actualMessagesCount);
    }
                    
    [Test]
    public async Task SendMessage_WithValidParticipation_InsertsMessageToDb()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newUser = UsersDataHelper.GenerateRandomUserIdentity(); 
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        var newParticipation = ParticipationsDataHelper.BasicParticipation;
        newParticipation.AspNetUserId = user.Id;
        newParticipation.ConversationId = chat.Id;
        newParticipation.HasLeft = false;
        var participation = await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        
        
        var message = MessagesDataHelper.GenerateRandomMessage(participation.Id);
        
        //Act
        var result = await _messageService.SendMessage(message, user.Id, chat.Id);
        
        //Assert
        var sendedMesssage = await _dbContext.Set<Message>().FirstAsync(x => x.MessageText == message.MessageText);
        
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(message.ParticipationId, sendedMesssage.ParticipationId);
        Assert.AreEqual(message.DateSent, sendedMesssage.DateSent);
    }
}