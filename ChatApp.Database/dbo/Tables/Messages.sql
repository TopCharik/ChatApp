create table dbo.Messages
(
    Id             int identity
        constraint PK_Messages
            primary key,
    ReplyTo        int
        constraint FK_Messages_Messages_ReplyTo
            references dbo.Messages,
    ConversationId int           not null
        constraint FK_Messages_Conversation
            references dbo.Conversation
            on delete cascade,
    UserId         nvarchar(450) not null
        constraint FK_Messages_AspNetUsers
            references dbo.AspNetUsers,
    Message        nvarchar(4000),
    DateSent       datetime
)