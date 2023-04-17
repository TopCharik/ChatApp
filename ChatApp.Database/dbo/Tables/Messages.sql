create table dbo.Messages
(
    Id              int identity
        constraint PK_Messages
        primary key,
    ReplyTo         int
        constraint FK_Messages_Messages_ReplyTo
            references dbo.Messages,
    MessageText     nvarchar(4000) not null,
    DateSent        datetime not null,
    ParticipationId int      not null
        constraint FK_Messages_Participations
            references dbo.Participations
)