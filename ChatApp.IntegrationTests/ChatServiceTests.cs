using ChatApp.BLL;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities;
using ChatApp.DAL.App.Interfaces;
using ChatApp.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChatApp.IntegrationTests;

public class ChatServiceTests : BaseServiceTest
{
    private IChatService _chatService;
    
    public ChatServiceTests()
    {
        var unitOfWork = _serviceProvider.GetService<IUnitOfWork>() 
                      ?? throw new InvalidOperationException("IUnitOfWork is not registered in BaseServiceTestFixture");
        _chatService = new ChatService(unitOfWork);
    }

    [Test]
    public async Task JoinChat_WithChatNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var chat = ConversationsDataHelper.GenerateRandomChat();
        chat.Id = Random.Shared.Next(1000, 100000);
        
        var expectedError = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;

        //Act
        var participation = ParticipationsDataHelper.BasicParticipation;
        participation.AspNetUserId = user.Id;
        participation.ConversationId = chat.Id;
        var result = await _chatService.JoinChat(chat.ChatInfo.ChatLink, participation);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [Test]
    public async Task JoinChat_WhenUserIsAlreadyMember_ReturnsError()
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
        await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);

        var expectedError = ChatServiceErrors.USER_IS_ALLREADY_MEMBER_OF_THIS_CHAT;

        //Act
        var participation = ParticipationsDataHelper.BasicParticipation;
        participation.AspNetUserId = user.Id;
        participation.ConversationId = chat.Id;
        var result = await _chatService.JoinChat(chat.ChatInfo.ChatLink, participation);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task JoinChat_WhenPrivateChat_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var newChat = ConversationsDataHelper.GenerateRandomChat();
        newChat.ChatInfo.IsPrivate = true;
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);

        var expectedError = ChatServiceErrors.USER_CANT_JOIN_PRIVATE_CHAT;

        //Act
        var participation = ParticipationsDataHelper.BasicParticipation;
        participation.AspNetUserId = user.Id;
        participation.ConversationId = chat.Id;
        var result = await _chatService.JoinChat(chat.ChatInfo.ChatLink, participation);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task JoinChat_WithValidParticipation_AddsUserToChat()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);
        
        //Act
        var participation = ParticipationsDataHelper.BasicParticipation;
        participation.AspNetUserId = user.Id;
        participation.ConversationId = chat.Id;
        var result = await _chatService.JoinChat(chat.ChatInfo.ChatLink, participation);

        //Assert
        var addedParticipation = await _dbContext.Set<Participation>()
            .FirstAsync(x => x.AspNetUserId == user.Id && x.ConversationId == chat.Id);
            
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(participation.AspNetUserId, addedParticipation.AspNetUserId);
        Assert.AreEqual(participation.ConversationId, addedParticipation.ConversationId);
    }
        
    [Test]
    public async Task JoinChat_WhenUserWasAMemberButLeft_AddsUserToChat()
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
        await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        _dbContext.ChangeTracker.Clear();
        
        //Act
        var participation = ParticipationsDataHelper.BasicParticipation;
        participation.AspNetUserId = user.Id;
        participation.ConversationId = chat.Id;
        var result = await _chatService.JoinChat(chat.ChatInfo.ChatLink, participation);

        //Assert
        var addedParticipation = await _dbContext.Set<Participation>()
            .FirstAsync(x => x.AspNetUserId == user.Id && x.ConversationId == chat.Id);
            
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(participation.AspNetUserId, addedParticipation.AspNetUserId);
        Assert.AreEqual(participation.ConversationId, addedParticipation.ConversationId);
    }
    
    [Test]
    public async Task LeaveChat_WhenChatLinkNotExist_SetsParticipationInChatFalse()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var chat = ConversationsDataHelper.GenerateRandomChat();

        var expectedError = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;

        //Act
        var result = await _chatService.LeaveChat(chat.ChatInfo.ChatLink, user.Id);

        //Assert
            
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task LeaveChat_WhenUserWasNotAMember_SetsParticipationInChatFalse()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);

        var expectedError = ChatServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CHAT;

        //Act
        var result = await _chatService.LeaveChat(chat.ChatInfo.ChatLink, user.Id);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [Test]
    public async Task LeaveChat_WhenUserWasAMemberButAlreadyLeftChat_SetsParticipationInChatFalse()
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
        await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        var expectedError = ChatServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CHAT;
        _dbContext.ChangeTracker.Clear();
        
        
        //Act
        var result = await _chatService.LeaveChat(chat.ChatInfo.ChatLink, user.Id);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task LeaveChat_WhenUserIsMemberOfChat_SetsParticipationInChatFalse()
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
        await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
        _dbContext.ChangeTracker.Clear();
        
        //Act
        var result = await _chatService.LeaveChat(chat.ChatInfo.ChatLink, user.Id);

        //Assert
        var addedParticipation = await _dbContext.Set<Participation>()
            .FirstAsync(x => x.AspNetUserId == user.Id && x.ConversationId == chat.Id);
            
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(user.Id, addedParticipation.AspNetUserId);
        Assert.AreEqual(chat.Id, addedParticipation.ConversationId);
        Assert.IsTrue(addedParticipation.HasLeft);
    }

    [Test]
    public async Task CreateNewChat_WhenChatWithThisLinkAlreadyExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
        
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);

        var expectedError = ChatServiceErrors.CHAT_WITH_THIS_LINK_ALLREADY_EXIST;
                
        //Act
        var creationChat = ConversationsDataHelper.GenerateRandomChat();
        creationChat.ChatInfo.ChatLink = chat.ChatInfo.ChatLink;
        var result = await _chatService.CreateNewChat(creationChat);
        
        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task CreateNewChat_WhenChatWithThisLinkNotExist_CreatesNewChat()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);
                
        //Act
        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var result = await _chatService.CreateNewChat(newChat);
        
        //Assert
        var createdChat = await _dbContext.Set<Conversation>()
            .FirstAsync(x => x.ChatInfo.ChatLink == newChat.ChatInfo.ChatLink);
            
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(newChat.ChatInfo.Title, createdChat.ChatInfo.Title);
        Assert.AreEqual(newChat.ChatInfo.ChatLink, createdChat.ChatInfo.ChatLink);
    }

    [Test]
    public async Task AddAvatar_WhenChatLinkNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var chat = ConversationsDataHelper.GenerateRandomChat();

        var expectedError = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;

        //Act
        var newAvatar = AvatarDataHelper.GenerateRandomAvatar();
        var result = await _chatService.AddAvatar(newAvatar, chat.ChatInfo.ChatLink, user.Id);
        
        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task AddAvatar_WhenUserNotAMember_AddsAvatar()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var newChat = ConversationsDataHelper.GenerateRandomChat();
        var chat = await ConversationsDataHelper.InsertNewChatToDbAsync(_dbContext, newChat);

        var expectedError = ChatServiceErrors.USER_CANT_ADD_AVATAR_TO_THIS_CHAT;

        //Act
        var newAvatar = AvatarDataHelper.GenerateRandomAvatar();
        var result = await _chatService.AddAvatar(newAvatar, chat.ChatInfo.ChatLink, user.Id);
        
        //Assert
        var chatAvatars = await _dbContext.Set<Avatar>()
            .Where(x => x.ChatInfoId == chat.ChatInfo.Id).ToListAsync();
            
        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task AddAvatar_WhenUserIsAMemberButCantChangeAvatar_AddsAvatar()
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
        newParticipation.CanChangeChatAvatar = false;
        await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);

        var expectedError = ChatServiceErrors.USER_CANT_ADD_AVATAR_TO_THIS_CHAT;

        //Act
        var newAvatar = AvatarDataHelper.GenerateRandomAvatar();
        var result = await _chatService.AddAvatar(newAvatar, chat.ChatInfo.ChatLink, user.Id);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [Test]
    public async Task AddAvatar_WhenCanAddAvatar_AddsAvatar()
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
        newParticipation.CanChangeChatAvatar = true;
        await ParticipationsDataHelper.InsertNewParticipationToDb(_dbContext, newParticipation);
                
        //Act
        var newAvatar = AvatarDataHelper.GenerateRandomAvatar();
        var result = await _chatService.AddAvatar(newAvatar, chat.ChatInfo.ChatLink, user.Id);
        
        //Assert
        var chatAvatars = await _dbContext.Set<Avatar>()
            .Where(x => x.ChatInfoId == chat.ChatInfo.Id).ToListAsync();
            
        Assert.IsTrue(result.Succeeded);
        Assert.IsTrue(chatAvatars.Any(x => x.ImagePayload == newAvatar.ImagePayload));
    }
    
    
}