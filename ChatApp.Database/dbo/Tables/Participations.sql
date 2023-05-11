create table Participations
(
    Id                    int identity
        constraint PK_Participations
            primary key,
    AspNetUserId          nvarchar(450) not null
        constraint FK_Participations_AspNetUsers
            references AspNetUsers
            on update cascade on delete cascade,
    ConversationId        int           not null
        constraint FK_Participations_Conversations
            references Conversations
            on update cascade on delete cascade,
    CanWriteMessages      bit           not null,
    CanDeleteMessages     bit           not null,
    MutedUntil            datetime,
    CanMuteParticipants   bit           not null,
    CanAddParticipants    bit           not null,
    CanDeleteParticipants bit           not null,
    CanChangeChatAvatar   bit           not null,
    CanChangeChatTitle    bit           not null,
    CanChangePublicity    bit           not null,
    CanSetPermissions     bit           not null,
    CanDeleteConversation bit           not null,
    HasLeft               bit,
    constraint QK_Participations_AspNetUserId_ConversationId
        unique (AspNetUserId, ConversationId)
)