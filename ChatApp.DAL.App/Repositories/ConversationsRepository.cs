using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class ConversationsRepository : BaseRepository<Conversation>, IConversationsRepository
{
    public ConversationsRepository(AppDbContext context) : base(context) { }

    public async Task<PagedList<Conversation>> GetPublicChatsAsync(ChatInfoParameters parameters)
    {
        var chats = GetByCondition(x => x.ChatInfoId != null)
            .Include(x => x.ChatInfo)
            .ThenInclude(x => x.Avatars.OrderBy(x => x.DateSet))
            .Where(x => EF.Functions.Like(x.ChatInfo.Title, $"%{parameters.Title}%"))
            .Where(x => EF.Functions.Like(x.ChatInfo.InviteLink, $"%{parameters.InviteLink}%"));

        return await PagedList<Conversation>.ToPagedList(chats, parameters.Page, parameters.PageSize);
    }

    public async Task<Conversation?> GetChatByInviteLink(string inviteLink)
    {
        var chat = await GetByCondition(x => x.ChatInfoId != null)
            .Include(x => x.ChatInfo)
            .ThenInclude(x => x.Avatars.OrderBy(x => x.DateSet))
            .FirstOrDefaultAsync(x => EF.Functions.Like(x.ChatInfo.InviteLink, $"%{inviteLink}%"));
        return chat;
    }

    public async Task CreateNewChat(Conversation newChat)
    {
        Create(newChat);
        await _context.SaveChangesAsync();
        return;
    }
}