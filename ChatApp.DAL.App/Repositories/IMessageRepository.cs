using ChatApp.Core.Entities;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.DAL.App.Interfaces;

namespace ChatApp.DAL.App.Repositories;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<List<Message>> GetMessages(int conversationId, MessageParameters messageParameters);
}