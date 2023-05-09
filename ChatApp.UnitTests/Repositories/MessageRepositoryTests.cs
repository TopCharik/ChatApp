using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.Repositories;
using NUnit.Framework;

namespace ChatApp.UnitTests.Repositories;

public class MessageRepositoryTests
{
    private MessageRepository _repository;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        var context = await FakeData.GetDefaultAppDbContext();
        _repository = new MessageRepository(context);
    }

    [Test]
    public async Task GetMessagesAsync_WithExistingConversation_ReturnsAllMessages()
    {
        var conversationId = 184623;
        var messageParameters = new MessageParameters();
        var expectedResult = new List<Message>
        {
            new()
            {
                Id = 12648,
                ParticipationId = 1,
                MessageText = "First message",
                DateSent = new DateTime(638192609520000000),
            },
            new()
            {
                Id = 85678,
                ParticipationId = 2,
                MessageText = "Second message",
                DateSent = new DateTime(638194337520000000),
            },
            new()
            {
                Id = 1234565,
                ParticipationId = 1,
                MessageText = "Third message",
                DateSent = new DateTime(638196929520000000),
            },
        };


        await VerifyMessageListAsync(expectedResult, conversationId, messageParameters);
    }

    [Test]
    public async Task GetMessagesAsync_WithAfterParameterAndExistingAfterMessages_ReturnsAfterMessages()
    {
        var conversationId = 184623;
        var messageParameters = new MessageParameters
        {
            After = new DateTime(638192897520000000),
        };
        var expectedResult = new List<Message>
        {
            new()
            {
                Id = 85678,
                ParticipationId = 2,
                MessageText = "Second message",
                DateSent = new DateTime(638194337520000000),
            },
            new()
            {
                Id = 1234565,
                ParticipationId = 1,
                MessageText = "Third message",
                DateSent = new DateTime(638196929520000000),
            },
        };


        await VerifyMessageListAsync(expectedResult, conversationId, messageParameters);
    }

    [Test]
    public async Task GetMessagesAsync_WithAfterParameterAndNonExistingAfterMessages_ReturnsAfterMessages()
    {
        var conversationId = 184623;
        var messageParameters = new MessageParameters
        {
            After = new DateTime(648057005520000000),
        };
        var expectedResult = new List<Message>();


        await VerifyMessageListAsync(expectedResult, conversationId, messageParameters);
    }

    [Test]
    public async Task GetMessagesAsync_WithExistingConversation_ReturnsAllMessagesWithIncludedParticipation()
    {
        var conversationId = 5588;
        var messageParameters = new MessageParameters();
        var expectedResult = new List<Message>
        {
            new()
            {
                Id = 15845,
                ParticipationId = 12384,
                MessageText = "Include AppUser test message",
                DateSent = DateTime.Today,
                Participation = new Participation
                {
                    Id = 12384,
                    AspNetUserId = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
                    ConversationId = 5588,
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
                },
            },
        };

        var result = await _repository.GetMessagesAsync(conversationId, messageParameters);

        Assert.AreEqual(expectedResult.First().Participation.Id, result.First().Participation.Id);
    }

    [Test]
    public async Task GetMessagesAsync_WithExistingConversation_ReturnsAllMessagesWithIncludedAppUser()
    {
        var conversationId = 5588;
        var messageParameters = new MessageParameters();
        var expectedResult = new List<Message>
        {
            new()
            {
                Id = 15845,
                ParticipationId = 12384,
                MessageText = "Include AppUser test message",
                DateSent = DateTime.Today,
                Participation = new Participation
                {
                    Id = 12384,
                    AspNetUserId = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
                    ConversationId = 5588,
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
                    AppUser = new AppUser
                    {
                        Id = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
                        FirstName = "Jane",
                        LastName = "Doe",
                        UserName = "janedoe",
                        NormalizedUserName = "JANEDOE",
                        Email = "janedoe@example.com",
                        NormalizedEmail = "JANEDOE@EXAMPLE.COM",
                        PhoneNumber = "098-765-4321",
                    },
                },
            },
        };

        var result = await _repository.GetMessagesAsync(conversationId, messageParameters);

        Assert.AreEqual(expectedResult.FirstOrDefault().Participation.AppUser.Id,
            result.FirstOrDefault().Participation.AppUser.Id);
    }


    [Test]
    public async Task GetMessagesAsync_WithExistingConversation_ReturnsAllMessagesWithIncludedAvatars()
    {
        var conversationId = 5588;
        var messageParameters = new MessageParameters();
        var expectedResult = new List<Message>
        {
            new()
            {
                Id = 15845,
                ParticipationId = 12384,
                MessageText = "Include AppUser test message",
                DateSent = DateTime.Today,
                Participation = new Participation
                {
                    Id = 12384,
                    AspNetUserId = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
                    ConversationId = 5588,
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
                    AppUser = new AppUser
                    {
                        Id = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
                        FirstName = "Jane",
                        LastName = "Doe",
                        UserName = "janedoe",
                        NormalizedUserName = "JANEDOE",
                        Email = "janedoe@example.com",
                        NormalizedEmail = "JANEDOE@EXAMPLE.COM",
                        PhoneNumber = "098-765-4321",
                        Avatars = new List<Avatar>
                        {
                            new ()
                            {
                                Id = 438191,
                                UserId = "93F6E32F-FB4E-4A26-BA89-41437427C30D",
                                ImagePayload = "698EB7EAB6B4457891AB4CA431DF918E",
                                DateSet = DateTime.Today,
                            }
                        }
                    },
                },
            },
        };

        var result = await _repository.GetMessagesAsync(conversationId, messageParameters);

        Assert.AreEqual(expectedResult.FirstOrDefault().Participation.AppUser.Avatars.First().ImagePayload,
            result.FirstOrDefault().Participation.AppUser.Avatars.First().ImagePayload);
    }

    [Test]
    public async Task GetMessagesAsync_WithNonExistingConversation_ReturnsEmptyList()
    {
        var conversationId = -100;

        var result = await _repository.GetMessagesAsync(conversationId, new MessageParameters());

        Assert.True(result.Count == 0);
    }


    private async Task VerifyMessageListAsync(List<Message> expectedMessages, int conversationId,
        MessageParameters parameters)
    {
        var result = await _repository.GetMessagesAsync(conversationId, parameters);

        Assert.AreEqual(expectedMessages.Count, result.Count);
        Assert.True(expectedMessages.All(expectedMessage =>
            result.FirstOrDefault(x => x.Id == expectedMessage.Id) != null));
    }
}