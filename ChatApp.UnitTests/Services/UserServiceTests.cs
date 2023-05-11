using ChatApp.BLL;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using ChatApp.DTO;
using Moq;
using NUnit.Framework;

namespace ChatApp.UnitTests.Services;

public class UserServiceTests
{
    private UserService _userService;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IAvatarRepository> _mockAvatarRepository;
    private Mock<IConversationsRepository> _mockConversationsRepository;
    private Mock<IParticipationRepository> _mockParticipationRepository;
    private Mock<IMessageRepository> _mockMessageRepository;
    private Mock<IUserRepository> _mockUserRepository;

    [SetUp]
    public void SetUp()
    {
        _mockAvatarRepository = new Mock<IAvatarRepository>();
        _mockConversationsRepository = new Mock<IConversationsRepository>();
        _mockParticipationRepository = new Mock<IParticipationRepository>();
        _mockMessageRepository = new Mock<IMessageRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IAvatarRepository>()).Returns(_mockAvatarRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IUserRepository>()).Returns(_mockUserRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IConversationsRepository>())
            .Returns(_mockConversationsRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IParticipationRepository>())
            .Returns(_mockParticipationRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IMessageRepository>()).Returns(_mockMessageRepository.Object);
        _userService = new UserService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetUsersAsync_WhenCalled_ReturnsPagedList()
    {
        var parameters = new AppUserParameters();
        var userList = PagedList<AppUser>.ToPagedList(new List<AppUser>(), parameters.Page, parameters.PageSize);
        _mockUserRepository.Setup(repo => repo.GetUsersAsync(parameters))
            .ReturnsAsync(userList);

        var result = await _userService.GetUsersAsync(parameters);

        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(userList, result.Value);
    }

    [Test]
    public async Task GetUserByUsernameAsync_WithUsernameNotExist_ReturnError()
    {
        var username = "test-username";
        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync(() => null);

        var result = await _userService.GetUserByUsernameAsync(username);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.IsNull(result.Value);
    }


    [Test]
    public async Task GetUserByUsernameAsync_WithUsernameExist_ReturnsUser()
    {
        var username = "test-username";
        var user = new AppUser
        {
            UserName = username,
        };
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync(user);

        var result = await _userService.GetUserByUsernameAsync(username);

        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(user, result.Value);
    }


    [Test]
    public async Task GetUserByConnectionId_WithConnectionIdNotExist_ReturnError()
    {
        var connectionId = "test-connectionId";
        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_CONNECTIONID;
        _mockUserRepository.Setup(repo => repo.GetUserByConnectionIdAsync(connectionId))
            .ReturnsAsync(() => null);

        var result = await _userService.GetUserByConnectionId(connectionId);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.IsNull(result.Value);
    }

    [Test]
    public async Task GetUserByConnectionId_WithConnectionIdExist_ReturnsUser()
    {
        var connectionId = "test-connectionId";
        var user = new AppUser
        {
            CallHubConnectionId = connectionId,
        };
        _mockUserRepository.Setup(repo => repo.GetUserByConnectionIdAsync(connectionId))
            .ReturnsAsync(user);

        var result = await _userService.GetUserByConnectionId(connectionId);

        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(user, result.Value);
    }

    [Test]
    public async Task GetUserIdsByUsernames_WithUsernameNotExist_ReturnsError()
    {
        var usernames = new List<string> {"username1", "username2"};
        var user1 = new AppUser {UserName = "username1"};
        AppUser? user2 = null;
        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("username1"))
            .ReturnsAsync(user1);
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("username2"))
            .ReturnsAsync(user2);

        var result = await _userService.GetUserIdsByUsernames(usernames);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.IsNull(result.Value);
    }

    [Test]
    public async Task GetUserIdsByUsernames_WithAllUsernamesExist_ReturnsUsernames()
    {
        var usernames = new List<string> {"username1", "username2"};
        var user1 = new AppUser {UserName = "username1"};
        AppUser? user2 = new AppUser {UserName = "username2"};
        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("username1"))
            .ReturnsAsync(user1);
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("username2"))
            .ReturnsAsync(user2);

        var result = await _userService.GetUserIdsByUsernames(usernames);

        Assert.IsTrue(result.Succeeded);
    }

    [Test]
    public async Task AddUserAvatar_WhenCalled_AddsAvatarToRepositoryAndSavesChanges()
    {
        var avatar = new Avatar();

        var result = await _userService.AddAvatar(avatar);

        _mockAvatarRepository.Verify(repo => repo.Create(avatar), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        Assert.That(result.Succeeded, Is.True);
    }

    [Test]
    public async Task SetCallHubConnectionId_WithUsernameNotExist_ReturnsError()
    {
        var username = "test-username";
        var hubConnectionId = "test-connectionId";
        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync(() => null);

        var result = await _userService.SetCallHubConnectionId(username, hubConnectionId);


        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        _mockUserRepository.Verify(repo => repo.Update(It.IsAny<AppUser>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task SetCallHubConnectionId_WithUsernameExist_UpdatesUser()
    {
        var username = "test-username";
        var hubConnectionId = "test-connectionId";
        var user = new AppUser
        {
            UserName = username,
        };
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(username))
            .ReturnsAsync(user);

        var result = await _userService.SetCallHubConnectionId(username, hubConnectionId);


        Assert.IsTrue(result.Succeeded);
        _mockUserRepository.Verify(repo => repo.Update(user), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task RemoveCallHubConnectionId_WithConnectionIdNotExist_ReturnError()
    {
        var connectionId = "test-connectionId";
        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_CONNECTIONID;
        _mockUserRepository.Setup(repo => repo.GetUserByConnectionIdAsync(connectionId))
            .ReturnsAsync(() => null);

        var result = await _userService.RemoveCallHubConnectionId(connectionId);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        _mockUserRepository.Verify(repo => repo.Update(It.IsAny<AppUser>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        Assert.IsNull(result.Value);
    }

    [Test]
    public async Task RemoveCallHubConnectionId_WithConnectionExist_RemovesHubConnectionId()
    {
        var connectionId = "test-connectionId";
        var user = new AppUser
        {
            CallHubConnectionId = connectionId,
        };
        _mockUserRepository.Setup(repo => repo.GetUserByConnectionIdAsync(connectionId))
            .ReturnsAsync(user);

        var result = await _userService.RemoveCallHubConnectionId(connectionId);

        Assert.IsTrue(result.Succeeded);
        _mockUserRepository.Verify(repo => repo.Update(user), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        Assert.IsNull(result.Value.CallHubConnectionId);
    }

    [TestCase(null, null)]
    [TestCase("user1", null)]
    [TestCase(null, "user2")]
    public async Task SetInCall_WithBothUsernamesNotExist1_ReturnsError(string? callInitiatorUsername, string? callReceiverUsername)
    {
        var callInitiator = callInitiatorUsername != null ? new AppUser { UserName = callInitiatorUsername } : null;
        var callReceiver = callReceiverUsername != null ? new AppUser { UserName = callReceiverUsername } : null;
        
        var callUsernames = new CallUsernamesDto
        {
            callInitiatorUsername = callInitiator?.UserName ?? "usernameNotExist",
            callReceiverUsername =  callReceiver?.UserName ?? "usernameNotExist",
        };
        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(callUsernames.callInitiatorUsername))
            .ReturnsAsync(callInitiator);
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(callUsernames.callReceiverUsername))
            .ReturnsAsync(callReceiver);

        var result = await _userService.SetInCallAsync(callUsernames, true);
        
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        _mockUserRepository.Verify(repo => repo.Update(It.IsAny<AppUser>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
    
    [Test]
    public async Task SetInCall_WithBothValueFalseSetTrue_UpdatesInCall()
    {
        AppUser? user1 = new AppUser{UserName = "username1", InCall = false};
        AppUser? user2 = new AppUser{UserName = "username2", InCall = false};
        var callUsernames = new CallUsernamesDto
        {
            callInitiatorUsername = "username1",
            callReceiverUsername = "username2",
        };
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("username1"))
            .ReturnsAsync(user1);
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync("username2"))
            .ReturnsAsync(user2);

        var result = await _userService.SetInCallAsync(callUsernames, true);
        
        Assert.IsTrue(result.Succeeded);
        _mockUserRepository.Verify(repo => repo.Update(user1), Times.Once);
        _mockUserRepository.Verify(repo => repo.Update(user2), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    
    [TestCase(true, false, true)]
    [TestCase(false, true, true)]
    [TestCase(true, true, true)]
    public async Task SetInCall_WithBothValueFalseSetTrue_ReturnsError(bool callInitiatorInCall, bool callReceiverInCall, bool updatedInCallValue)
    {
        AppUser? callInitiator = new AppUser{UserName = "username1", InCall = callInitiatorInCall};
        AppUser? callReceiver = new AppUser{UserName = "username2", InCall = callReceiverInCall};
        var callUsernames = new CallUsernamesDto
        {
            callInitiatorUsername = callInitiator.UserName,
            callReceiverUsername = callReceiver.UserName,
        };
        var expectedError = UserServiceErrors.USER_IS_ALREADY_IN_CALL;
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(callInitiator.UserName))
            .ReturnsAsync(callInitiator);
        _mockUserRepository.Setup(repo => repo.GetUserByUsernameAsync(callReceiver.UserName))
            .ReturnsAsync(callReceiver);

        var result = await _userService.SetInCallAsync(callUsernames, updatedInCallValue);
        
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
}