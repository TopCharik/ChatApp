using ChatApp.Core.Entities;
using ChatApp.DAL.App.Interfaces;

namespace ChatApp.DAL.App.Repositories;

public interface IMessageRepository : IBaseRepository<Message>
{
    Task<List<Message>> GetMessages(int conversationId);
}