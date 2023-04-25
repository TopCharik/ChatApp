using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class ConversationsRepository : BaseRepository<Conversation>, IConversationsRepository
{
    public ConversationsRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Conversation>> GetPublicChatsAsync(ChatInfoParameters parameters)
    {
        var chats = GetByCondition(x => x.ChatInfoId != null)
            .Include(x => x.ChatInfo)
            .ThenInclude(x => x.Avatars.OrderBy(x => x.DateSet))
            .Where(x => x.ChatInfo.IsPrivate == false);

        SearchGlobal(ref chats, parameters.Search);
        SearchByTitle(ref chats, parameters.Title);
        SearchByLink(ref chats, parameters.ChatLink);

        return await PagedList<Conversation>.ToPagedList(chats, parameters.Page, parameters.PageSize);
    }

    public async Task<Conversation?> GetChatByLink(string ChatLink)
    {
        var chat = await GetByCondition(x => x.ChatInfoId != null)
            .Include(x => x.ChatInfo)
            .ThenInclude(x => x.Avatars.OrderBy(x => x.DateSet))
            .FirstOrDefaultAsync(x => EF.Functions.Like(x.ChatInfo.ChatLink, $"%{ChatLink}%"));
        return chat;
    }

    private static void SearchGlobal(ref IQueryable<Conversation> chats, string? search)
    {

        chats = string.IsNullOrEmpty(search)
            ? chats
            : chats.Where(u =>
                EF.Functions.Like(u.ChatInfo.Title, $"%{search}%") ||
                EF.Functions.Like(u.ChatInfo.ChatLink, $"%{search}%")
            );
    }

    private static void SearchByTitle(ref IQueryable<Conversation> chats, string? title)
    {
        chats = string.IsNullOrEmpty(title)
            ? chats
            : chats.Where(u => EF.Functions.Like(u.ChatInfo.Title, $"%{title}%"));
    }

    private static void SearchByLink(ref IQueryable<Conversation> chats, string? link)
    {
        chats = string.IsNullOrEmpty(link)
            ? chats
            : chats.Where(u => EF.Functions.Like(u.ChatInfo.ChatLink, $"%{link}%"));

    }
}