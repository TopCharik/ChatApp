using ChatApp.BLL;
using ChatApp.BLL.Helpers;
using ChatApp.BLL.Helpers.ServiceErrors;
using ChatApp.Core.Entities;
using ChatApp.DAL.App.Interfaces;
using ChatApp.DAL.App.Repositories;
using Moq;
using NUnit.Framework;

namespace ChatApp.UnitTests.Services;

[TestFixture]
public class AvatarServiceTests
{
        private AvatarService _avatarService;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IAvatarRepository> _mockAvatarRepository;
        private Mock<IConversationsRepository> _mockConversationsRepository;

        [SetUp]
        public void SetUp()
        {
            _mockAvatarRepository = new Mock<IAvatarRepository>();
            _mockConversationsRepository = new Mock<IConversationsRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IAvatarRepository>()).Returns(_mockAvatarRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<IConversationsRepository>()).Returns(_mockConversationsRepository.Object);
            _avatarService = new AvatarService(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task AddUserAvatar_WhenCalled_AddsAvatarToRepositoryAndSavesChanges()
        {
            var avatar = new Avatar();

            var result = await _avatarService.AddUserAvatar(avatar);

            _mockAvatarRepository.Verify(repo => repo.Create(avatar), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            Assert.That(result.Succeeded, Is.True);
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
            
            var result = await _avatarService.AddChatAvatar(avatar, chatLink, uploaderId);

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
            _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, uploaderId))
                .ReturnsAsync(() => null);

            var result = await _avatarService.AddChatAvatar(avatar, chatLink, uploaderId);

            _mockAvatarRepository.Verify(repo => repo.Create(It.IsAny<Avatar>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.AreEqual(result.Errors.First().Key,
                AvatarServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST.First().Key);
            Assert.AreEqual(result.Errors.First().Value,
                AvatarServiceErrors.CHAT_WITH_THIS_LINK_DOESNT_EXIST.First().Value);
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
            _mockConversationsRepository.Setup(repo => repo.GetChatWithUserParticipationByLink(chatLink, uploaderId))
                .ReturnsAsync(() => chatWithUserParticipation);
    
            var result = await _avatarService.AddChatAvatar(avatar, chatLink, uploaderId);

            Assert.IsInstanceOf<ServiceResult<Conversation>>(result);
            Assert.AreEqual(result.Errors.Count, 1);
            Assert.AreEqual(result.Errors.First().Key,
                AvatarServiceErrors.GIVED_USER_DONT_HAVE_PERMISSION_FOR_UPLOAD_AVATAR.First().Key);
            Assert.AreEqual(result.Errors.First().Value, 
                AvatarServiceErrors.GIVED_USER_DONT_HAVE_PERMISSION_FOR_UPLOAD_AVATAR.First().Value);
        }
}