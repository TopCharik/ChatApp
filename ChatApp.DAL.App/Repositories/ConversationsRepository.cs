using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.Core.Helpers;
using ChatApp.DAL.App.AppContext;
using ChatApp.DAL.App.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DAL.App.Repositories;

public class ConversationsRepository : BaseRepository<Conversation>, IConversationsRepository
{
    public ConversationsRepository(AppDbContext context) 
        : base(context) { }

    public async Task<PagedList<ChatInfoView>> GetChatsAsync(ChatInfoParameters parameters)
    {
        var chats = _context.Set<ChatInfoView>()
            .Include(
                x => x.Avatars
                    .OrderByDescending(x => x.DateSet)
                    .Take(1)
            )
            .AsNoTracking();

        SortChats(ref chats, parameters.SortField, parameters.SortDirection);
        SearchGlobal(ref chats, parameters.Search);
        SearchByTitle(ref chats, parameters.Title);
        SearchByLink(ref chats, parameters.ChatLink);

        return await PagedList<ChatInfoView>.ToPagedListAsync(chats, parameters.Page, parameters.PageSize);
    }

    public async Task<Conversation?> GetChatByLink(string chatLink)
    {
        var chat = await GetByCondition(x => x.ChatInfoId != null)
            .Include(x => x.ChatInfo)
            .ThenInclude(x => x.Avatars.OrderByDescending(x => x.DateSet))
            .FirstOrDefaultAsync(x => EF.Functions.Like(x.ChatInfo.ChatLink, $"%{chatLink}%"));

        return chat;
    }

    public async Task<Conversation?> GetChatWithUserParticipationByLink(string chatLink, string userId)
    {
        var conversation = await GetByCondition(x => x.ChatInfoId != null)
            .Include(x => x.ChatInfo)
            .ThenInclude(x => x.Avatars.OrderByDescending(x => x.DateSet))
            .Include(x => x.Participations.Where(p => p.AspNetUserId == userId))
            .FirstOrDefaultAsync(x => x.ChatInfo.ChatLink == chatLink);

        return conversation;
    }

    public async Task<Conversation?> GetChatWithUserParticipationById(int chatId, string userId)
    {
        var conversation = await GetByCondition(x => x.ChatInfoId != null)
            .Include(x => x.ChatInfo)
            .ThenInclude(x => x.Avatars.OrderByDescending(x => x.DateSet))
            .Include(x => x.Participations.Where(p => p.AspNetUserId == userId))
            .FirstOrDefaultAsync(x => x.Id == chatId);

        return conversation;
    }

    private static void SearchGlobal(ref IQueryable<ChatInfoView> chats, string? search)
    {
        chats = string.IsNullOrEmpty(search)
            ? chats
            : chats.Where(u =>
                EF.Functions.Like(u.Title, $"%{search}%") ||
                EF.Functions.Like(u.ChatLink, $"%{search}%")
            );
    }

    private static void SortChats(ref IQueryable<ChatInfoView> users, string? sortField, SortDirection? sortOrder)
    {
        var isAsc = sortOrder == SortDirection.Ascending;

        users = sortField?.ToLower() switch
        {
            "title" =>
                isAsc
                    ? users.OrderBy(u => u.Title)
                    : users.OrderByDescending(u => u.Title),
            "chatlink" =>
                isAsc
                    ? users.OrderBy(u => u.ChatLink)
                    : users.OrderByDescending(u => u.ChatLink),
            "participationscount" =>
                isAsc
                    ? users.OrderBy(u => u.ParticipationsCount)
                    : users.OrderByDescending(u => u.ParticipationsCount),
            _ => isAsc
                ? users.OrderBy(u => u.Title)
                : users.OrderByDescending(u => u.Title),
        };
    }

    private static void SearchByTitle(ref IQueryable<ChatInfoView> chats, string? title)
    {
        chats = string.IsNullOrEmpty(title)
            ? chats
            : chats.Where(u => EF.Functions.Like(u.Title, $"%{title}%"));
    }

    private static void SearchByLink(ref IQueryable<ChatInfoView> chats, string? link)
    {
        chats = string.IsNullOrEmpty(link)
            ? chats
            : chats.Where(u => EF.Functions.Like(u.ChatLink, $"%{link}%"));
    }
}