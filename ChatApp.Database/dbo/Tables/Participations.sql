create table dbo.Participations
(
    Id                    int identity
        constraint PK_Participations
        primary key,
    AspNetUserId          nvarchar(450) not null
        constraint FK_Participations_AspNetUsers
        references dbo.AspNetUsers,
    ConversationId        int not null
        constraint FK_Participations_Conversations
            references dbo.Conversations,
    CanWriteMessages      bit not null,
    CanDeleteMessages     bit not null,
    MutedUntil            datetime,
    CanMuteParticipants   bit not null,
    CanAddParticipants    bit not null,
    CanDeleteParticipants bit not null,
    CanChangeChatAvatar   int not null,
    CanChangeChatTitle    int not null,
    CanChangePublicity    bit not null,
    CanSetPermissions     bit not null,
    CanDeleteConversation bit not null,
    constraint QK_Participations_AspNetUserId_ConversationId
        unique (AspNetUserId, ConversationId)
)