using ChatApp.Core.Entities;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.DTO;

namespace ChatApp.API.Helpers;

public static class ConversationFactory
{
    public static Conversation NewChat(NewChatDto newChatDto, string ownerId, List<string> userIds)
    {
        var participants = userIds
            .Select(ParticipationFactory.DefaultChatMember).ToList();

        var Avatars = new List<Avatar>();

        if (newChatDto.AvatarUrl != null)
        {
            Avatars.Add(new Avatar
            {
                PictureUrl = newChatDto.AvatarUrl,
                DateSet = DateTime.Now,
            });
        }
        
        participants.Add(ParticipationFactory.ChatOwner(ownerId));
        
        var conversation = new Conversation
        { 
            ChatInfo = new ChatInfo
            {
                Title = newChatDto.Title,
                ChatLink = newChatDto.ChatLink,
                IsPrivate = newChatDto.IsPrivate,
                Avatars = Avatars,
            },
            Participations = participants,
        };

        return conversation;
    }
}