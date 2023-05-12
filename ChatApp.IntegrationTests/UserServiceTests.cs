using System.Globalization;
using ChatApp.BLL;
using ChatApp.BLL.Helpers;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Helpers;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DTO;
using ChatApp.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChatApp.IntegrationTests;

public class UserServiceTests : BaseServiceTest
{
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        var unitOfWork = _serviceProvider.GetService<IUnitOfWork>()
                         ?? throw new InvalidOperationException(
                             "IUnitOfWork is not registered in BaseServiceTestFixture");
        _userService = new UserService(unitOfWork);
    }


    [Test]
    public async Task GetUserByUsernameAsync_WhenUsernameNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();

        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;

        //Act
        var result = await _userService.GetUserByUsernameAsync(newUser.UserName);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [Test]
    public async Task GetUserByUsernameAsync_WhenUsernameExist_ReturnsUser()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        //Act
        var result = await _userService.GetUserByUsernameAsync(user.UserName);

        //Assert
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(user.Id, result.Value.Id);
    }


    [Test]
    public async Task GetUserIdsByUsernames_WhenNotAllUsernamesExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var users = new List<AppUser>();
        for (var i = 0; i < 10; i++)
        {
            users.Add(
                await UsersDataHelper.RegisterNewUserAsync(_userManager, UsersDataHelper.GenerateRandomUserIdentity())
            );
        }

        var usernames = users.Select(x => x.UserName).ToList();

        usernames.Add(Guid.NewGuid().ToString());

        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;

        //Act
        var result = await _userService.GetUserIdsByUsernames(usernames);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [Test]
    public async Task GetUserIdsByUsernames_WhenAllUsernamesExist_ReturnsUserIds()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var users = new List<AppUser>();
        for (var i = 0; i < 10; i++)
        {
            users.Add(
                await UsersDataHelper.RegisterNewUserAsync(_userManager, UsersDataHelper.GenerateRandomUserIdentity())
            );
        }

        var usernames = users.Select(x => x.UserName).ToList();

        //Act
        var result = await _userService.GetUserIdsByUsernames(usernames);

        //Assert
        Assert.IsTrue(result.Succeeded);
        Assert.IsTrue(users.All(requestedUser => result.Value.Any(resultUserId => requestedUser.Id == resultUserId)));
    }


    [Test]
    public async Task GetUserByConnectionId_WhenCallHubConnectionIdNotExist_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_CONNECTIONID;

        //Act
        var result = await _userService.RemoveCallHubConnectionId(Guid.NewGuid().ToString());

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [Test]
    public async Task AddAvatar_WhenCalledWithInvalidUserId_ThrowsException()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var avatar = AvatarDataHelper.GenerateRandomAvatar();
        avatar.UserId = Guid.NewGuid().ToString();

        //Act
        var action = async () => await _userService.AddAvatar(avatar);

        //Assert
        Assert.That(action,
            Throws.Exception.TypeOf<DbUpdateException>());
    }

    [Test]
    public async Task AddAvatar_WhenValidCalled_AddsAvatar()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        var avatar = AvatarDataHelper.GenerateRandomAvatar();
        avatar.UserId = user.Id;

        //Act
        var result = await _userService.AddAvatar(avatar);

        //Assert
        var addedAvatar = await _dbContext.Set<Avatar>()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x =>
                x.UserId == avatar.UserId
                && x.ImagePayload == avatar.ImagePayload
                && x.DateSet == avatar.DateSet);

        Assert.IsTrue(result.Succeeded);
        Assert.NotNull(addedAvatar);
        Assert.AreEqual(avatar.ImagePayload, addedAvatar.ImagePayload);
        Assert.AreEqual(avatar.DateSet, addedAvatar.DateSet);
    }

    [TestCase("11BA4049684F46ADA8D496E2D2E00F26")]
    [TestCase(null)]
    public async Task SetCallHubConnectionId_WithNotExistingUsername_ReturnsError(string? callHubConnectionId)
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();

        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;

        //Act
        var result = await _userService.SetCallHubConnectionId(newUser.UserName, callHubConnectionId);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [TestCase("11BA4049684F46ADA8D496E2D2E00F26")]
    [TestCase(null)]
    public async Task SetCallHubConnectionId_WithExistingUsername_UpdatesCallHubConnectionId(
        string? callHubConnectionId)
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);

        //Act
        var result = await _userService.SetCallHubConnectionId(user.UserName, callHubConnectionId);

        //Assert
        var updatedUser = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == user.Id);
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(updatedUser.CallHubConnectionId, callHubConnectionId);
    }

    [Test]
    public async Task RemoveCallHubConnectionId_WithNotExistingConnectionId_ReturnsError()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var callHubConnectionId = Guid.NewGuid().ToString();

        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_CONNECTIONID;

        //Act
        var result = await _userService.RemoveCallHubConnectionId(callHubConnectionId);

        //Assert
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }

    [Test]
    public async Task RemoveCallHubConnectionId_WithExistingConnectionId_RemovesCallHubConnectionId()
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newUser = UsersDataHelper.GenerateRandomUserIdentity();
        var user = await UsersDataHelper.RegisterNewUserAsync(_userManager, newUser);
        user.CallHubConnectionId = Guid.NewGuid().ToString();
        user = await UsersDataHelper.UpdateUserAsync(_dbContext, user);
        _dbContext.ChangeTracker.Clear();

        //Act
        var result = await _userService.RemoveCallHubConnectionId(user.CallHubConnectionId);

        //Assert
        var updatedUser = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == user.Id);
        Assert.IsTrue(result.Succeeded);
        Assert.IsNull(updatedUser.CallHubConnectionId);
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(true, true)]
    public async Task SetInCall_WhenCallReceiverNotExist_ReturnsError(bool callInitiatorInCallValue,
        bool newInCallValue)
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newCallInitiator = UsersDataHelper.GenerateRandomUserIdentity();
        var callInitiator = await UsersDataHelper.RegisterNewUserAsync(_userManager, newCallInitiator);
        callInitiator.InCall = callInitiatorInCallValue;
        callInitiator = await UsersDataHelper.UpdateUserAsync(_dbContext, callInitiator);

        var newCallReceiver = UsersDataHelper.GenerateRandomUser();

        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;

        _dbContext.ChangeTracker.Clear();

        //Act
        var callUsernamesDto = new CallUsernamesDto
        {
            callInitiatorUsername = callInitiator.UserName,
            callReceiverUsername = newCallReceiver.UserName,
        };
        var result = await _userService.SetInCallAsync(callUsernamesDto, newInCallValue);

        //Assert
        var updatedCallInitiator = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == callInitiator.Id);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(callInitiator.InCall, updatedCallInitiator.InCall);
    }

    [TestCase(true, true)]
    [TestCase(false, true)]
    [TestCase(true, false)]
    [TestCase(true, true)]
    [NonParallelizable]
    public async Task SetInCall_WhenCallInitiatorNotExist_ReturnsError(bool callReceiverInCallValue,
        bool newInCallValue)
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newCallInitiator = UsersDataHelper.GenerateRandomUser();

        var newCallReceiver = UsersDataHelper.GenerateRandomUserIdentity();
        var callReceiver = await UsersDataHelper.RegisterNewUserAsync(_userManager, newCallReceiver);
        callReceiver.InCall = callReceiverInCallValue;
        callReceiver = await UsersDataHelper.UpdateUserAsync(_dbContext, callReceiver);

        var expectedError = UserServiceErrors.USER_NOT_FOUND_BY_USERNAME;

        _dbContext.ChangeTracker.Clear();

        //Act
        var callUsernamesDto = new CallUsernamesDto
        {
            callInitiatorUsername = newCallInitiator.UserName,
            callReceiverUsername = callReceiver.UserName,
        };
        var result = await _userService.SetInCallAsync(callUsernamesDto, newInCallValue);

        //Assert
        var updatedCallReceiver = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == callReceiver.Id);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(callReceiver.InCall, updatedCallReceiver.InCall);
    }

    [TestCase(true, true, true)]
    [TestCase(true, false, true)]
    [TestCase(false, true, true)]
    [NonParallelizable]
    public async Task SetInCall_WhenUserAlreadyInCall_ReturnsError(
        bool callInitiatorInCallValue, bool callReceiverInCallValue, bool newInCallValue)
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newCallInitiator = UsersDataHelper.GenerateRandomUserIdentity();
        var callInitiator = await UsersDataHelper.RegisterNewUserAsync(_userManager, newCallInitiator);
        callInitiator.InCall = callInitiatorInCallValue;
        callInitiator = await UsersDataHelper.UpdateUserAsync(_dbContext, callInitiator);

        var newCallReceiver = UsersDataHelper.GenerateRandomUserIdentity();
        var callReceiver = await UsersDataHelper.RegisterNewUserAsync(_userManager, newCallReceiver);
        callReceiver.InCall = callReceiverInCallValue;
        callReceiver = await UsersDataHelper.UpdateUserAsync(_dbContext, callReceiver);

        var expectedError = UserServiceErrors.USER_IS_ALREADY_IN_CALL;

        _dbContext.ChangeTracker.Clear();

        //Act
        var callUsernamesDto = new CallUsernamesDto
        {
            callInitiatorUsername = callInitiator.UserName,
            callReceiverUsername = callReceiver.UserName,
        };
        var result = await _userService.SetInCallAsync(callUsernamesDto, newInCallValue);

        //Assert
        var updatedCallInitiator = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == callInitiator.Id);
        var updatedCallReceiver = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == callReceiver.Id);
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(callInitiator.InCall, updatedCallInitiator.InCall);
        Assert.AreEqual(callReceiver.InCall, updatedCallReceiver.InCall);
    }

    [TestCase(false, false, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(true, true, false)]
    [NonParallelizable]
    public async Task SetInCall_WithValidUsers_UpdatesInCall(
        bool callInitiatorInCallValue, bool callReceiverInCallValue, bool newInCallValue)
    {
        //Arrange
        await DbHelpers.ClearDb(_dbContext);

        var newCallInitiator = UsersDataHelper.GenerateRandomUserIdentity();
        var callInitiator = await UsersDataHelper.RegisterNewUserAsync(_userManager, newCallInitiator);
        callInitiator.InCall = callInitiatorInCallValue;
        callInitiator = await UsersDataHelper.UpdateUserAsync(_dbContext, callInitiator);

        var newCallReceiver = UsersDataHelper.GenerateRandomUserIdentity();
        var callReceiver = await UsersDataHelper.RegisterNewUserAsync(_userManager, newCallReceiver);
        callReceiver.InCall = callReceiverInCallValue;
        callReceiver = await UsersDataHelper.UpdateUserAsync(_dbContext, callReceiver);

        _dbContext.ChangeTracker.Clear();

        //Act
        var callUsernamesDto = new CallUsernamesDto
        {
            callInitiatorUsername = callInitiator.UserName,
            callReceiverUsername = callReceiver.UserName,
        };
        var result = await _userService.SetInCallAsync(callUsernamesDto, newInCallValue);

        //Assert
        var updatedCallInitiator = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == callInitiator.Id);
        var updatedCallReceiver = await _dbContext.Set<AppUser>().FirstAsync(x => x.Id == callReceiver.Id);
        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(newInCallValue, updatedCallInitiator.InCall);
        Assert.AreEqual(newInCallValue, updatedCallReceiver.InCall);
    }
    
    [Test]
    public async Task GetUsersAsync_SortsUsersByPropertyAndDirection_ReturnsSortedPagedList()
    {
        // Arrange
        await DbHelpers.ClearDb(_dbContext);

        var users = new List<AppUser>();
        for (var i = 0; i < Random.Shared.Next(5, 100); i++)
        {
            users.Add(await UsersDataHelper.RegisterNewUserAsync(_userManager,
                UsersDataHelper.GenerateRandomUserIdentity()));
        }

        var parameters = new AppUserParameters
        {
            Page = 0,
            PageSize = 10,
        };

        // Act
        var sortProperties = new[] {"UserName", "Email", "FirstName", "LastName", "PhoneNumber"};
        var sortDirections = new[] {SortDirection.Ascending, SortDirection.Descending};

        foreach (var property in sortProperties)
        {
            foreach (var direction in sortDirections)
            {
                parameters.SortField = property;
                parameters.SortDirection = direction;

                var expectedOrder = direction == SortDirection.Ascending
                    ? users.OrderBy(u => u.GetType().GetProperty(property)?.GetValue(u).ToString()).ToList()
                    : users.OrderByDescending(u => u.GetType().GetProperty(property)?.GetValue(u).ToString()).ToList();

                var expectedResult = PagedList<AppUser>
                    .ToPagedList(expectedOrder, parameters.Page, parameters.PageSize, users.Count);

                var result = await _userService.GetUsersAsync(parameters);

                // Assert
                Assert.IsTrue(result.Succeeded);
                Assert.AreEqual(users.Count, result.Value.TotalCount);
                Assert.AreEqual(parameters.PageSize, result.Value.PageSize);
                Assert.AreEqual(expectedResult.Count, result.Value.Count);


                for (int i = 0; i < expectedResult.Count; i++)
                {
                    Assert.AreEqual(expectedResult[i].Id, result.Value[i].Id);
                }
            }
        }
    }
}