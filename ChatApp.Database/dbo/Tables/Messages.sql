create table dbo.Messages
(
    Id              int identity
        constraint PK_Messages
            primary key,
    MessageText     nvarchar(max) not null,
    DateSent        datetime      not null,
    ParticipationId int
        constraint FK_Messages_Participations
            references dbo.Participations
)