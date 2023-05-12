using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.Core.Helpers;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Repositories;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace ChatApp.UnitTests.Repositories;

public class ConversationRepositoryTest
{
    private ConversationsRepository _repository;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        var context = await FakeData.GetDefaultAppDbContext();
        _repository = new ConversationsRepository(context);
    }

    [Test]
    public async Task GetPublicChatsAsync_WithExistingChat_ReturnsPagedList()
    {
        var parameters = new ChatInfoParameters();

        var result = await _repository.GetChatsAsync(parameters);

        Assert.AreEqual(FakeData.FakeChatInfoViews.Count, result.TotalCount);
    }

    [Test]
    public async Task GetPublicChatsAsync_WithSearchFilter_ReturnsFilteredPagedList()
    {
        var parameters = new ChatInfoParameters
        {
            Search = "0B513832CB504082945E842101768547",
        };
        var expectedResult = new List<ChatInfoView>
        {
            new()
            {
                ChatInfoId = 47364,
                ConversationId = 54321,
                Title = "Sample chat0B513832CB504082945E842101768547 info view",
                ChatLink = "Sample_chat_info_view",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
            new()
            {
                ChatInfoId = 75639,
                ConversationId = 951212,
                Title = "Sample info view",
                ChatLink = "Sample_chat0B513832CB504082945E842101768547_info_view",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
        };

        await VerifyChatInfoViewsListAsync(parameters, expectedResult);
    }

    [Test]
    public async Task GetPublicChatsAsync_WithTitleFilter_ReturnsFilteredPagedList()
    {
        var parameters = new ChatInfoParameters
        {
            Title = "C77B4C0373B04B7F90D58626A4A57130",
        };
        var expectedResult = new List<ChatInfoView>
        {
            new()
            {
                ChatInfoId = 5615,
                ConversationId = 7612,
                Title = "Sample C77B4C0373B04B7F90D58626A4A57130 chat info view",
                ChatLink = "Sample_chat_info_view",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
            new()
            {
                ChatInfoId = 1218,
                ConversationId = 13382,
                Title = "Sample info viC77B4C0373B04B7F90D58626A4A57130ew",
                ChatLink = "Sample_chat_info_view",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
        };

        await VerifyChatInfoViewsListAsync(parameters, expectedResult);
    }

    [Test]
    public async Task GetPublicChatsAsync_WithChatLinkFilter_ReturnsFilteredPagedList()
    {
        var parameters = new ChatInfoParameters
        {
            ChatLink = "8FC9455FF8834A70959376286A516A80",
        };
        var expectedResult = new List<ChatInfoView>
        {
            new()
            {
                ChatInfoId = 25486,
                ConversationId = 13845,
                Title = "Sample chat info view",
                ChatLink = "Sample_chat_8FC9455FF8834A70959376286A516A80_info_view",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
            new()
            {
                ChatInfoId = 24157,
                ConversationId = 82468,
                Title = "Sample info view",
                ChatLink = "Sample_8FC9455FF8834A70959376286A516A80_chat_info_view",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
        };

        await VerifyChatInfoViewsListAsync(parameters, expectedResult);
    }

    [Test]
    public async Task GetPublicChatsAsync_WithAllFilters_ReturnsFilteredPagedList()
    {
        var parameters = new ChatInfoParameters
        {
            Title = "42291A2FD36F4492B4625E1EFC5D57EC",
            Search = "C5088C799987455598303440E7727E0C",
            ChatLink = "31CB47FA19FA458684888912ECB95D4F",
        };
        var expectedResult = new List<ChatInfoView>
        {
            new()
            {
                ChatInfoId = 99872,
                ConversationId = 45687,
                Title = "Sample 42291A2FD36F4492B4625E1EFC5D57EC chat info view",
                ChatLink = "Sample_ch31CB47FA19FA458684888912ECB95D4Fat_info_viC5088C799987455598303440E7727E0Cew",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
            new()
            {
                ChatInfoId = 176523,
                ConversationId = 18578,
                Title = "Sample inCC5088C799987455598303440E7727E0Cfo vie42291A2FD36F4492B4625E1EFC5D57ECw",
                ChatLink = "Sample_ch31CB47FA19FA458684888912ECB95D4FCat_info_view",
                IsPrivate = false,
                ParticipationsCount = 325,
            },
        };

        await VerifyChatInfoViewsListAsync(parameters, expectedResult);
    }
    
    [TestCase("TiTLe", SortDirection.Ascending, ExpectedResult = 26134)]
    [TestCase("TiTLe", SortDirection.Descending, ExpectedResult = 12345565)]
    [TestCase("CHatLinK", SortDirection.Ascending, ExpectedResult = 73145)]
    [TestCase("ChATlInK", SortDirection.Descending, ExpectedResult = 24563)]
    [TestCase("pArTIciPatIonSCOunT", SortDirection.Ascending, ExpectedResult = 4682)]
    [TestCase("ParTiCIPatIonSCouNT", SortDirection.Descending, ExpectedResult = 26481)]
    public async Task<int> GetChatsAsync_SortByField_ReturnsSortedPagedList(string sortField, SortDirection sortDirection)
    {
        var parameters = new ChatInfoParameters
        {
            SortField = sortField,
            SortDirection = sortDirection,
        };

        var result = await _repository.GetChatsAsync(parameters);

        return result.First().ChatInfoId;
    }

    [Test]
    public async Task GetChatByLink_WithExistingChatLink_ReturnsChat()
    {
        var chatLink = "1AD31E854732405A9192519324462E34";
        var expectedResult = new Conversation
        {
            Id = 87561,
            ChatInfoId = 26481,
        };

        var result = await _repository.GetChatByLink(chatLink);
        
        Assert.NotNull(result);
        Assert.AreEqual(expectedResult.Id, result.Id);
    }
    
    [Test]
    public async Task GetChatByLink_WithExistingChatLink_IncludesChatInfo()
    {
        var chatLink = "1AD31E854732405A9192519324462E34";
        var expectedResult = new Conversation
        {
            Id = 87561,
            ChatInfoId = 26481,
            ChatInfo = new ChatInfo
            {
                Id = 26481,
                Title = "Third Chat",
                ChatLink = chatLink,
                IsPrivate = true,
            },
        };

        var result = await _repository.GetChatByLink(chatLink);
        
        Assert.NotNull(result);
        Assert.AreEqual(expectedResult.ChatInfo.Id, result.ChatInfo.Id);
    }
    
        
    [Test]
    public async Task GetChatByLink_WithExistingChatLinkWithAvatar_IncludesAvatar()
    {
        var chatLink = "1AD31E854732405A9192519324462E34";
        var expectedResult = new Conversation
        {
            Id = 87561,
            ChatInfoId = 26481,
            ChatInfo = new ChatInfo
            {
                Id = 26481,
                Title = "Third Chat",
                ChatLink = chatLink,
                IsPrivate = true,
                Avatars = new List<Avatar>
                {
                    new()
                    {
                        Id = 18645,
                        ChatInfoId = 26481,
                        DateSet = DateTime.Now,
                        ImagePayload = "D34020D2-B3DC-4B5D-B0C4-8586F239D469",   
                    },
                },
            },
        };

        var result = await _repository.GetChatByLink(chatLink);
        
        Assert.NotNull(result);
        Assert.AreEqual(expectedResult.ChatInfo.Avatars.First().ImagePayload, result.ChatInfo.Avatars.First().ImagePayload);
    }

    [Test]
    public async Task GetChatByLink_WithNotExistingChatLink_ReturnsNull()
    {
        var notExistingChatLink = "3619B3FE863D49B6BA5C691DD5917EE5";

        var result = await _repository.GetChatByLink(notExistingChatLink);
        
        Assert.IsNull(result);
    }

    [Test]
    public async Task GetChatWithUserParticipationByLink_WithExistingUserParticipation_ReturnsUserParticipation()
    {
        var chatLink = "42D422C6304C41AB82EF14FA9283D973";
        var userId = "57453247-3B50-4034-90C3-47473C333E40";
        var expectedResult = new Conversation
        {
            Id = 51348,
            ChatInfoId = 98762,
            ChatInfo = new ChatInfo
            {
                Id = 98762,
                Title = "Chat Title",
                ChatLink = chatLink,
                IsPrivate = false,
            },
            Participations = new List<Participation>
            {
                new Participation
                {
                    Id = 852,
                    AspNetUserId = userId,
                    ConversationId = 184623,
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

        var result = await _repository.GetChatWithUserParticipationByLink(chatLink, userId);
        
        Assert.AreEqual(expectedResult.Id, result.Id);
        Assert.AreEqual(expectedResult.ChatInfoId, result.ChatInfoId);
        Assert.AreEqual(expectedResult.Participations.First().Id, result.Participations.First().Id);
    }
    
    [Test]
    public async Task GetChatWithUserParticipationByLink_WithExistingUserParticipationWithAvatar_ReturnsUserParticipationWithIncludedAvatar()
    {
        var chatLink = "42D422C6304C41AB82EF14FA9283D973";
        var userId = "57453247-3B50-4034-90C3-47473C333E40";
        var expectedResult = new Conversation
        {
            Id = 51348,
            ChatInfoId = 98762,
            ChatInfo = new ChatInfo
            {
                Id = 98762,
                Title = "Chat Title",
                ChatLink = chatLink,
                IsPrivate = false,
                Avatars = new List<Avatar>
                {
                    new()
                    {
                       Id = 45682,
                       ChatInfoId = 98762,
                       ImagePayload = "84B3748352334877A3F77EE4D4BA2ED9",
                       DateSet = DateTime.Today,
                    }
                },
            },
            Participations = new List<Participation>
            {
                new Participation
                {
                    Id = 852,
                    AspNetUserId = userId,
                    ConversationId = 184623,
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

        var result = await _repository.GetChatWithUserParticipationByLink(chatLink, userId);
        
        Assert.AreEqual(expectedResult.Id, result.Id);
        Assert.AreEqual(expectedResult.ChatInfoId, result.ChatInfoId);
        Assert.AreEqual(expectedResult.ChatInfo.Avatars.First().Id, result.ChatInfo.Avatars.First().Id);
    }
    
    [Test]
    public async Task GetChatWithUserParticipationByLink_WithExistingChatLinkWithoutUserParticipation_ReturnsConversationWithoutUserParticipation()
    {
        var chatLink = "42D422C6304C41AB82EF14FA9283D973";
        var userId = "CAD0C37C-2F69-4861-9B74-9BCC7BEFC3F6";
        var expectedResult = new Conversation
        {
            Id = 51348,
            ChatInfoId = 98762,
            ChatInfo = new ChatInfo
            {
                Id = 98762,
                Title = "Chat Title",
                ChatLink = chatLink,
                IsPrivate = false,
            },
            Participations = new List<Participation>(),
        };

        var result = await _repository.GetChatWithUserParticipationByLink(chatLink, userId);
        
        Assert.AreEqual(expectedResult.Id, result.Id);
        Assert.AreEqual(expectedResult.ChatInfoId, result.ChatInfoId);
        Assert.AreEqual(0, result.Participations.Count);
    }

    [Test]
    public async Task GetChatWithUserParticipationByLink_WithNotExistingChatLink_ReturnsUserParticipation()
    {
        var notExistingChatLink = "3619B3FE863D49B6BA5C691DD5917EE5";
        var userId = "57453247-3B50-4034-90C3-47473C333E40";

        var result = await _repository.GetChatWithUserParticipationByLink(notExistingChatLink, userId);
        
        Assert.IsNull(result);
    }
    
        [Test]
    public async Task GetChatWithUserParticipationById_WithExistingUserParticipation_ReturnsUserParticipation()
    {
        var chatId = 51348;
        var userId = "57453247-3B50-4034-90C3-47473C333E40";
        var expectedResult = new Conversation
        {
            Id = chatId,
            ChatInfoId = 98762,
            ChatInfo = new ChatInfo
            {
                Id = 98762,
                Title = "Chat Title",
                ChatLink = "42D422C6304C41AB82EF14FA9283D973",
                IsPrivate = false,
            },
            Participations = new List<Participation>
            {
                new Participation
                {
                    Id = 852,
                    AspNetUserId = userId,
                    ConversationId = 184623,
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

        var result = await _repository.GetChatWithUserParticipationById(chatId, userId);
        
        Assert.AreEqual(expectedResult.Id, result.Id);
        Assert.AreEqual(expectedResult.ChatInfoId, result.ChatInfoId);
        Assert.AreEqual(expectedResult.Participations.First().Id, result.Participations.First().Id);
    }
    
    [Test]
    public async Task GetChatWithUserParticipationById_WithExistingUserParticipationWithAvatar_ReturnsUserParticipationWithIncludedAvatar()
    {
        var chatId = 51348;
        var userId = "57453247-3B50-4034-90C3-47473C333E40";
        var expectedResult = new Conversation
        {
            Id = chatId,
            ChatInfoId = 98762,
            ChatInfo = new ChatInfo
            {
                Id = 98762,
                Title = "Chat Title",
                ChatLink = "42D422C6304C41AB82EF14FA9283D973",
                IsPrivate = false,
                Avatars = new List<Avatar>
                {
                    new()
                    {
                       Id = 45682,
                       ChatInfoId = 98762,
                       ImagePayload = "84B3748352334877A3F77EE4D4BA2ED9",
                       DateSet = DateTime.Today,
                    }
                },
            },
            Participations = new List<Participation>
            {
                new Participation
                {
                    Id = 852,
                    AspNetUserId = userId,
                    ConversationId = 184623,
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

        var result = await _repository.GetChatWithUserParticipationById(chatId, userId);
        
        Assert.AreEqual(expectedResult.Id, result.Id);
        Assert.AreEqual(expectedResult.ChatInfoId, result.ChatInfoId);
        Assert.AreEqual(expectedResult.ChatInfo.Avatars.First().Id, result.ChatInfo.Avatars.First().Id);
    }
    
    [Test]
    public async Task GetChatWithUserParticipationById_WithExistingChatIdWithoutUserParticipation_ReturnsConversationWithoutUserParticipation()
    {
        var chatId = 51348;
        var userId = "CAD0C37C-2F69-4861-9B74-9BCC7BEFC3F6";
        var expectedResult = new Conversation
        {
            Id = chatId,
            ChatInfoId = 98762,
            ChatInfo = new ChatInfo
            {
                Id = 98762,
                Title = "Chat Title",
                ChatLink = "42D422C6304C41AB82EF14FA9283D973",
                IsPrivate = false,
            },
            Participations = new List<Participation>(),
        };

        var result = await _repository.GetChatWithUserParticipationById(chatId, userId);
        
        Assert.AreEqual(expectedResult.Id, result.Id);
        Assert.AreEqual(expectedResult.ChatInfoId, result.ChatInfoId);
        Assert.AreEqual(0, result.Participations.Count);
    }

    [Test]
    public async Task GetChatWithUserParticipationById_WithNotExistingChatId_ReturnsUserParticipation()
    {
        var notExistingChatId = 9182;
        var userId = "57453247-3B50-4034-90C3-47473C333E40";

        var result = await _repository.GetChatWithUserParticipationById(notExistingChatId, userId);
        
        Assert.IsNull(result);
    }

    private async Task VerifyChatInfoViewsListAsync(ChatInfoParameters parameters,
        List<ChatInfoView> expectedChatInfoViews)
    {
        var expectedResult =
            new PagedList<ChatInfoView>(expectedChatInfoViews, expectedChatInfoViews.Count, parameters.Page, parameters.PageSize);

        var result = await _repository.GetChatsAsync(parameters);

        Assert.AreEqual(expectedResult.Count, result.Count);

        Assert.True(expectedResult.All(
            expectedChatInfoView => result.FirstOrDefault(
                x => x.ChatInfoId == expectedChatInfoView.ChatInfoId
                     && x.ConversationId == expectedChatInfoView.ConversationId) != null));
    }
}