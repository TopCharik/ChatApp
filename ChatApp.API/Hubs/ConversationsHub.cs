using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Hubs;

public class ConversationsHub : Hub
{
    public async Task ChatNotificationAsync(string conversationId)
    {
        await Clients.All.SendAsync($"{conversationId}/NewMessage");
    }
}