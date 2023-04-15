create table dbo.Participants
(
    ConversationId int           not null
        constraint FK_Participants_Conversation
            references dbo.Conversation
            on delete cascade,
    UserId         nvarchar(450) not null
        constraint FK_Participants_AspNetUsers
            references dbo.AspNetUsers
            on delete cascade
)