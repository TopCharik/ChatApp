using ChatApp.BLL;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Moq;
using NUnit.Framework;

namespace ChatApp.UnitTests.Services;

public class MessageServiceTests
{
    private MessageService _messageService;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IAvatarRepository> _mockAvatarRepository;
    private Mock<IConversationsRepository> _mockConversationsRepository;
    private Mock<IParticipationRepository> _mockParticipationRepository;
    private Mock<IMessageRepository> _mockMessageRepository;

    [SetUp]
    public void SetUp()
    {
        _mockAvatarRepository = new Mock<IAvatarRepository>();
        _mockConversationsRepository = new Mock<IConversationsRepository>();
        _mockParticipationRepository = new Mock<IParticipationRepository>();
        _mockMessageRepository =  new Mock<IMessageRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IAvatarRepository>()).Returns(_mockAvatarRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IConversationsRepository>()).Returns(_mockConversationsRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IParticipationRepository>()).Returns(_mockParticipationRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.GetRepository<IMessageRepository>()).Returns(_mockMessageRepository.Object);
        _messageService = new MessageService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetMessages_WithChatLinkNotExist_ReturnsError()
    {
        var conversationId = 123;
        var userId = "test-id";
        var messageParameters = new MessageParameters();
        var messages = new List<Message>();
        var expectedError = MessageServiceErrors.CHAT_NOT_FOUND;
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationById(conversationId, userId))
            .ReturnsAsync(() => null);
        _mockMessageRepository.Setup(repo => repo.GetMessagesAsync(conversationId, messageParameters))
            .ReturnsAsync(messages);

        var result = await _messageService.GetMessages(conversationId, userId, messageParameters);
        
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(null, result.Value);
    }
    
    [Test]
    public async Task GetMessages_FromPrivateChatWithoutUserParticipation_ReturnsError()
    {
        var conversationId = 123;
        var userId = "test-id";
        var messageParameters = new MessageParameters();
        var messages = new List<Message>();
        var conversation = new Conversation
        {
            Id = conversationId,
            ChatInfo = new ChatInfo
            {
                IsPrivate = true,
            },
            Participations =  null,
        };
        var expectedError = MessageServiceErrors.USER_CANT_READ_MESSAGES_FROM_THIS_CHAT;
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationById(conversationId, userId))
            .ReturnsAsync(conversation);
        _mockMessageRepository.Setup(repo => repo.GetMessagesAsync(conversationId, messageParameters))
            .ReturnsAsync(messages);

        var result = await _messageService.GetMessages(conversationId, userId, messageParameters);
        
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(null, result.Value);
    }
        
    [Test]
    public async Task GetMessages_FromPrivateChatWithUserLeftParticipation_ReturnsError()
    {
        var conversationId = 123;
        var userId = "test-id";
        var messageParameters = new MessageParameters();
        var messages = new List<Message>();
        var conversation = new Conversation
        {
            Id = conversationId,
            ChatInfo = new ChatInfo
            {
                IsPrivate = true,
            },
            Participations = new List<Participation>
            {
                new ()
                {
                    HasLeft = true,
                },
            },
        };
        var expectedError = MessageServiceErrors.USER_CANT_READ_MESSAGES_FROM_THIS_CHAT;
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationById(conversationId, userId))
            .ReturnsAsync(conversation);
        _mockMessageRepository.Setup(repo => repo.GetMessagesAsync(conversationId, messageParameters))
            .ReturnsAsync(messages);

        var result = await _messageService.GetMessages(conversationId, userId, messageParameters);
        
        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual(expectedError, result.Errors);
        Assert.AreEqual(null, result.Value);
    }
    
    [Test]
    public async Task GetMessages_FromPrivateChatWithUserParticipation_ReturnsMessages()
    {
        var conversationId = 123;
        var userId = "test-id";
        var messageParameters = new MessageParameters();
        var messages = new List<Message>();
        var conversation = new Conversation
        {
            Id = conversationId,
            ChatInfo = new ChatInfo
            {
                IsPrivate = true,
            },
            Participations = new List<Participation>
            {
                new ()
                {
                    AspNetUserId = userId,
                    HasLeft = false,
                },
            },
        };
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationById(conversationId, userId))
            .ReturnsAsync(conversation);
        _mockMessageRepository.Setup(repo => repo.GetMessagesAsync(conversationId, messageParameters))
            .ReturnsAsync(messages);

        var result = await _messageService.GetMessages(conversationId, userId, messageParameters);
        
        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(messages, result.Value);
    }
            
    [Test]
    public async Task GetMessages_FromPublicChatWithoutUserParticipation_ReturnsMessages()
    {
        var conversationId = 123;
        var userId = "test-id";
        var messageParameters = new MessageParameters();
        var messages = new List<Message>();
        var conversation = new Conversation
        {
            Id = conversationId,
            ChatInfo = new ChatInfo
            {
                IsPrivate = false,
            },
            Participations = null,
        };
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationById(conversationId, userId))
            .ReturnsAsync(conversation);
        _mockMessageRepository.Setup(repo => repo.GetMessagesAsync(conversationId, messageParameters))
            .ReturnsAsync(messages);

        var result = await _messageService.GetMessages(conversationId, userId, messageParameters);
        
        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(messages, result.Value);
    }
        
    [Test]
    public async Task GetMessages_FromPublicChatWithUserParticipation_ReturnsMessages()
    {
        var conversationId = 123;
        var userId = "test-id";
        var messageParameters = new MessageParameters();
        var messages = new List<Message>();
        var conversation = new Conversation
        {
            Id = conversationId,
            ChatInfo = new ChatInfo
            {
                IsPrivate = false,
            },
            Participations = new List<Participation>
            {
                new ()
                {
                    AspNetUserId = userId,
                    HasLeft = false,
                },
            },
        };
        
        _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationById(conversationId, userId))
            .ReturnsAsync(conversation);
        _mockMessageRepository.Setup(repo => repo.GetMessagesAsync(conversationId, messageParameters))
            .ReturnsAsync(messages);

        var result = await _messageService.GetMessages(conversationId, userId, messageParameters);
        
        Assert.IsTrue(result.Succeeded);
        Assert.AreSame(messages, result.Value);
    }

    [Test]
    public async Task SendMessage_WhenUserIsNotParticipant_ReturnsError()
    {
        var senderId = "test-id";
        var conversationId = 1230;
        var message = new Message();
        var expectedError = MessageServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION;
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(senderId, conversationId))
            .ReturnsAsync(() => null);

        var result = await _messageService.SendMessage(message, senderId, conversationId);
        
        Assert.IsFalse(result.Succeeded);
        _mockMessageRepository.Verify(repo => repo.Create(It.IsAny<Message>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
    
    [Test]
    public async Task SendMessage_WhenUserWasAParticipantButLeft_ReturnsError()
    {
        var senderId = "test-id";
        var conversationId = 1230;
        var participation = new Participation
        {
            AspNetUserId = senderId,
            HasLeft = true,
        };
        var message = new Message();
        var expectedError = MessageServiceErrors.USER_IS_NOT_A_MEMBER_OF_THIS_CONVERSATION;
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(senderId, conversationId))
            .ReturnsAsync(participation);

        var result = await _messageService.SendMessage(message, senderId, conversationId);
        
        Assert.IsFalse(result.Succeeded);
        _mockMessageRepository.Verify(repo => repo.Create(It.IsAny<Message>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
        
    [Test]
    public async Task SendMessage_WhenUserIsAParticipantButCantWriteMessages_ReturnsError()
    {
        var senderId = "test-id";
        var conversationId = 1230;
        var participation = new Participation
        {
            AspNetUserId = senderId,
            HasLeft = false,
            CanWriteMessages = false,
        };
        var message = new Message();
        var expectedError = MessageServiceErrors.USER_CANT_WRITE_MESSAGES_IN_THIS_CHAT;
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(senderId, conversationId))
            .ReturnsAsync(participation);

        var result = await _messageService.SendMessage(message, senderId, conversationId);
        
        Assert.IsFalse(result.Succeeded);
        _mockMessageRepository.Verify(repo => repo.Create(It.IsAny<Message>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
            
    [Test]
    public async Task SendMessage_WhenUserIsAParticipantButMuted_ReturnsError()
    {
        var senderId = "test-id";
        var conversationId = 1230;
        var participation = new Participation
        {
            AspNetUserId = senderId,
            HasLeft = false,
            CanWriteMessages = true,
            MutedUntil = DateTime.Today.AddDays(1),
        };
        var message = new Message();
        var expectedError = MessageServiceErrors.USER_IS_MUTED_UNTIL(DateTime.Today.AddDays(1));
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(senderId, conversationId))
            .ReturnsAsync(participation);

        var result = await _messageService.SendMessage(message, senderId, conversationId);
        
        Assert.IsFalse(result.Succeeded);
        _mockMessageRepository.Verify(repo => repo.Create(It.IsAny<Message>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
        
    [Test]
    public async Task SendMessage_WhenUserIsAParticipantAndNotMuted_CreatesMessage()
    {
        var senderId = "test-id";
        var conversationId = 1230;
        var participation = new Participation
        {
            AspNetUserId = senderId,
            HasLeft = false,
            CanWriteMessages = true,
            MutedUntil = null,
        };
        var message = new Message();
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(senderId, conversationId))
            .ReturnsAsync(participation);

        var result = await _messageService.SendMessage(message, senderId, conversationId);
        
        Assert.IsTrue(result.Succeeded);
        _mockMessageRepository.Verify(repo => repo.Create(message), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    
    [Test]
    public async Task SendMessage_WhenUserIsAParticipantMutedTimePast_CreatesMessage()
    {
        var senderId = "test-id";
        var conversationId = 1230;
        var participation = new Participation
        {
            AspNetUserId = senderId,
            HasLeft = false,
            CanWriteMessages = true,
            MutedUntil = DateTime.Today.AddDays(-1),
        };
        var message = new Message();
        _mockParticipationRepository.Setup(repo => repo.GetUserParticipationByConversationIdAsync(senderId, conversationId))
            .ReturnsAsync(participation);

        var result = await _messageService.SendMessage(message, senderId, conversationId);
        
        Assert.IsTrue(result.Succeeded);
        _mockMessageRepository.Verify(repo => repo.Create(message), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}