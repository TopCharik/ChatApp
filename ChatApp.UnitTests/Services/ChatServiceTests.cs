using ChatApp.BLL;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Moq;
using NUnit.Framework;

namespace ChatApp.UnitTests.Services;

public class ChatServiceTests
{
    private ChatService _chatService;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IAvatarRepository> _mockAvatarRepository;
    private Mock<IConversationsRepository> _mockConversationsRepository;
    private Mock<IParticipationRepository> _mockParticipationRepository;

    [SetUp]
    public void SetUp()
    {
        _mockAvatarRepository = new Mock<IAvatarRepository>();
        _mockConversationsRepository = new Mock<IConversationsRepository>();
        _mockParticipationRepository = new Mock<IParticipationRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IAvatarRepository>()).Returns(_mockAvatarRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IConversationsRepository>()).Returns(_mockConversationsRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IParticipationRepository>()).Returns(_mockParticipationRepository.Object);
        _chatService = new ChatService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetChatsAsync_WhenCalled_ReturnsPagedList()
    {
        var parameters = new ChatInfoParameters();
        var chatsList = PagedList<ChatInfoView>.ToPagedList(new List<ChatInfoView>(), parameters.Page, parameters.PageSize);
        _mockConversationsRepository.Setup(repo => repo.GetChatsAsync(parameters))
            .ReturnsAsync(chatsList);

        var result = await _chatService.GetChatsAsync(parameters);
        
        _mockConversationsRepository.Verify(repo => repo.GetChatsAsync(parameters), Times.Once);
        Assert.AreSame(chatsList, result.Value);
    }


    [Test]
    public async Task JoinChat_ChatWithGivenLinkNotExist_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-user-id";
        var participation = new Participation { AspNetUserId = userId, HasLeft = false };
        var conversationInfo = new Conversation
        {
            Participations = new List<Participation> {participation},
            ChatInfo = new ChatInfo {IsPrivate = false},
        };
        var expectedErrors = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(() => null);
        

        var result = await _chatService.JoinChat(chatLink, participation);

        Assert.IsFalse(result.Succeeded);
        CollectionAssert.AreEqual(expectedErrors, result.Errors);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
    
    [Test]
    public async Task JoinChat_UserAlreadyJoined_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-user-id";
        var participation = new Participation { AspNetUserId = userId };
        participation.HasLeft = false;
        var conversationInfo = new Conversation
        {
            Participations = new List<Participation> {participation},
            ChatInfo = new ChatInfo {IsPrivate = false},
        };
        var expectedErrors = ChatServiceErrors.USER_IS_ALLREADY_MEMBER_OF_THIS_CHAT;
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(() => conversationInfo);
        

        var result = await _chatService.JoinChat(chatLink, participation);

        Assert.IsFalse(result.Succeeded);
        CollectionAssert.AreEqual(expectedErrors, result.Errors);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
    
    [Test]
    public async Task JoinChat_JoiningPrivateChat_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-user-id";
        var participation = new Participation {AspNetUserId = userId};
        var conversationInfo = new Conversation
        {
            Participations = new List<Participation>(),
            ChatInfo = new ChatInfo {IsPrivate = true},
        };
        var expectedErrors = ChatServiceErrors.USER_CANT_JOIN_PRIVATE_CHAT;
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(() => conversationInfo);
        

        var result = await _chatService.JoinChat(chatLink, participation);

        Assert.IsFalse(result.Succeeded);
        CollectionAssert.AreEqual(expectedErrors, result.Errors);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
    
    [Test]
    public async Task JoinChat_UserJoiningChatFirstTime_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-user-id";
        var participation = new Participation { AspNetUserId = userId };
        var conversationInfo = new Conversation
        {
            Participations = new List<Participation>(),
            ChatInfo = new ChatInfo {IsPrivate = false},
        };
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(() => conversationInfo);
        

        var result = await _chatService.JoinChat(chatLink, participation);

        Assert.True(result.Succeeded);
        _mockParticipationRepository.Verify(p => p.Create(participation), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
        
    [Test]
    public async Task JoinChat_UserAlreadyJoinedChatButLeft_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-user-id";
        var participation = new Participation { AspNetUserId = userId };
        participation.HasLeft = true;
        var conversationInfo = new Conversation
        {
            Participations = new List<Participation> {participation},
            ChatInfo = new ChatInfo {IsPrivate = false},
        };
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(() => conversationInfo);
        

        var result = await _chatService.JoinChat(chatLink, participation);

        Assert.True(result.Succeeded);
        _mockParticipationRepository.Verify(p => p.Update(participation), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task LeaveChat_WithChatLinkNotExist_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-user-id";
        var expectedErrors = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(() => null);
        

        var result = await _chatService.LeaveChat(chatLink, userId);

        Assert.IsFalse(result.Succeeded);
        CollectionAssert.AreEqual(expectedErrors, result.Errors);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
    
    [Test]
    public async Task LeaveChat_WhenUserNeverJoinedChatBefore_ReturnsError()
    {
        var chatLink = "test-link";
        var conversationId = 123;
        var userId = "test-user-id";
        var chat = new Conversation
        {
            Id = conversationId,
        };
        
        var expectedErrors = ChatServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CHAT;

        _mockConversationsRepository.Setup(repo => repo.GetChatByLink(chatLink))
            .ReturnsAsync(chat);
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(userId, conversationId))
            .ReturnsAsync(() => null);
        

        var result = await _chatService.LeaveChat(chatLink, userId);

        Assert.IsFalse(result.Succeeded);
        CollectionAssert.AreEqual(expectedErrors, result.Errors);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
        
    [Test]
    public async Task LeaveChat_WhenUserJoinedChatBeforeButLeft_ReturnsError()
    {
        var chatLink = "test-link";
        var conversationId = 123;
        var userId = "test-user-id";
        var chat = new Conversation
        {
            Id = conversationId,
        };
        var participation = new Participation
        {
            HasLeft = true,
        };
        
        var expectedErrors = ChatServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CHAT;

        _mockConversationsRepository.Setup(repo => repo.GetChatByLink(chatLink))
            .ReturnsAsync(chat);
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(userId, conversationId))
            .ReturnsAsync(participation);
        

        var result = await _chatService.LeaveChat(chatLink, userId);

        Assert.IsFalse(result.Succeeded);
        CollectionAssert.AreEqual(expectedErrors, result.Errors);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
            
    [Test]
    public async Task LeaveChat_WhenUserJoinedChatBefore_ReturnsError()
    {
        var chatLink = "test-link";
        var conversationId = 123;
        var userId = "test-user-id";
        var chat = new Conversation
        {
            Id = conversationId,
        };
        var participation = new Participation
        {
            HasLeft = false,
        };
        

        _mockConversationsRepository.Setup(repo => repo.GetChatByLink(chatLink))
            .ReturnsAsync(chat);
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(userId, conversationId))
            .ReturnsAsync(participation);
        

        var result = await _chatService.LeaveChat(chatLink, userId);

        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(participation.HasLeft, true);
        _mockParticipationRepository.Verify(repo => repo.Update(participation), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task CreateNewChat_WhenChatLinkAlreadyExist_ReturnsError()
    {
        var chat = new Conversation
        {
            ChatInfo = new ChatInfo
            {
                ChatLink = "test-link",
            },
        };
        _mockConversationsRepository.Setup(repo => repo.GetChatByLink(chat.ChatInfo.ChatLink))
            .ReturnsAsync(chat);

        var result = await _chatService.CreateNewChat(chat);
        
        Assert.IsFalse(result.Succeeded);
        _mockConversationsRepository.Verify(repo => repo.Create(It.IsAny<Conversation>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
    
    
    [Test]
    public async Task CreateNewChat_WhenChatLinkNotExist_ReturnsError()
    {
        
        var chat = new Conversation
        {
            ChatInfo = new ChatInfo
            {
                ChatLink = "test-link",
            },
        };
        _mockConversationsRepository.Setup(repo => repo.GetChatByLink(chat.ChatInfo.ChatLink))
            .ReturnsAsync(() => null);

        var result = await _chatService.CreateNewChat(chat);
        
        Assert.IsTrue(result.Succeeded);
        _mockConversationsRepository.Verify(repo => repo.Create(chat), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task GetChatByLink_WhenChatLinkNotExist_ReturnsError()
    {
        var chatLink = "test-link";
        _mockConversationsRepository.Setup(repo => repo.GetChatByLink(chatLink))
            .ReturnsAsync(() => null);
        var expectedError = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;

        var result = await _chatService.GetChatByLink(chatLink);
        
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task GetChatByLink_WhenChatLinkExist_ReturnsChat()
    {
        var chatLink = "test-link";
        var chat = new Conversation();
        _mockConversationsRepository.Setup(repo => repo.GetChatByLink(chatLink))
            .ReturnsAsync(chat);

        var result = await _chatService.GetChatByLink(chatLink);
        
        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(chat, result.Value);
    }

    [Test]
    public async Task GetParticipationByChatLink_WhenChatLinkNotExist_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-id";
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(() => null);
        var expectedError = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;

        var result = await _chatService.GetParticipationByChatLink(chatLink, userId);
        
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
    }
    
    [Test]
    public async Task GetParticipationByChatLink_WhenChatLinkExist_ReturnsError()
    {
        var chatLink = "test-link";
        var userId = "test-id";
        var participation = new Conversation();
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, userId))
            .ReturnsAsync(participation);

        var result = await _chatService.GetParticipationByChatLink(chatLink, userId);
        
        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(participation, result.Value);
    }
    
    [Test]
    public async Task AddChatAvatar_WhenChatWithLinkExistAndUserHasAccess_AddsAvatarToRepositoryAndSavesChange()
        {
            var avatar = new Avatar();
            var chatLink = "test";
            var uploaderId = "1";
            var chatWithUserParticipation = new Conversation
            {
                ChatInfoId = 1,
                Participations = new List<Participation>
                {
                    new()
                    {
                        AspNetUserId = uploaderId,
                        CanChangeChatAvatar = true,
                    },
                }
            };
            _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, uploaderId))
                .ReturnsAsync(() => chatWithUserParticipation);
            
            var result = await _chatService.AddAvatar(avatar, chatLink, uploaderId);

            _mockAvatarRepository.Verify(repo => repo.Create(avatar), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            Assert.That(result.Succeeded, Is.True);
        }

        [Test]
        public async Task AddChatAvatar_WhenChatWithLinkDoesNotExist_ReturnsServiceResultWithError()
        {
            var avatar = new Avatar();
            var chatLink = "e78d9f2fab8b42d298dad78b4b9fae03";
            var uploaderId = "aa4ed3959dc34f628db939e2876aa63a";
            var expectedError = ChatServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST;
            _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, uploaderId))
                .ReturnsAsync(() => null);

            var result = await _chatService.AddAvatar(avatar, chatLink, uploaderId);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(expectedError, result.Errors);
            _mockAvatarRepository.Verify(repo => repo.Create(avatar), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
        
        [Test]
        public async Task AddChatAvatar_WhenUserDoesNotHavePermission_ReturnsServiceResultWithError()
        {
            var avatar = new Avatar();
            var chatLink = "test";
            var uploaderId = "1";
            var chatWithUserParticipation = new Conversation
            {
                ChatInfoId = 1,
                Participations = new List<Participation>()
                {
                    new()
                    {
                        AspNetUserId = uploaderId,
                        CanChangeChatAvatar = false,
                    },
                }
            };
            var expectedError = ChatServiceErrors.USER_CANT_ADD_AVATAR_TO_THIS_CHAT;
            _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, uploaderId))
                .ReturnsAsync(() => chatWithUserParticipation);
    
            var result = await _chatService.AddAvatar(avatar, chatLink, uploaderId);
            
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(expectedError, result.Errors);
            _mockAvatarRepository.Verify(repo => repo.Create(avatar), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
}