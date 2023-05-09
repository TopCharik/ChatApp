using ChatApp.Core.Entities;
using ChatApp.DAL.App.Repositories;
using NUnit.Framework;

namespace ChatApp.UnitTests.Repositories;

public class ParticipationRepositoryTests
{
    private ParticipationRepository _repository;
    
    [OneTimeSetUp]
    public async Task SetUp()
    {
        var context = await FakeData.GetDefaultAppDbContext(); 
        _repository = new ParticipationRepository(context);
    }
    
    [Test]
    public async Task GetUserParticipationByConversationId_WithConversationIdNotExist_ReturnsNull()
    {
        var username = "nonexistingusername";
        var ConversationId = 165182;

        var result = await _repository.GetUserParticipationByConversationIdAsync(username, ConversationId);

        Assert.IsNull(result);
    }

    [Test]
    public async Task GetUserParticipationByConversationId_WithConnectionIdNotExist_ReturnsNull()
    {
        var connectionId = "nonexistinguserconnectionId";
        var ConversationId = 1;

        var result = await _repository.GetUserParticipationByConversationIdAsync(connectionId, 1);

        Assert.IsNull(result);
    }
    
    [Test]
    public async Task GetUserParticipationByConversationId_WithParticipationExist_ReturnsParticipation()
    {
        var connectionId = "00DA31F8-0619-4593-A89B-A39AA3FB88E7";
        var ConversationId = 25482;
        var expectedResult = new Participation()
        {
            Id = 1972,
            AspNetUserId = "00DA31F8-0619-4593-A89B-A39AA3FB88E7",
            ConversationId = 25482,
            CanWriteMessages = true,
            CanMuteParticipants = false,
            CanDeleteMessages = false,
            CanAddParticipants = false,
            CanDeleteParticipants = false,
            CanChangePublicity = false,
            CanChangeChatAvatar = false,
            CanChangeChatTitle = false,
            CanSetPermissions = false,
            CanDeleteConversation = false,
        };

        var result = await _repository.GetUserParticipationByConversationIdAsync(connectionId, 25482);

        Assert.NotNull(result);
        Assert.AreEqual(expectedResult.Id, result.Id);
    } 
}