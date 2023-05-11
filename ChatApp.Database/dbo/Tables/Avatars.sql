create table Avatars
(
    Id           int identity
        constraint PK_Avatars
            primary key,
    UserId       nvarchar(450)
        constraint FK_Avatars_AspNetUsers
            references AspNetUsers
            on update cascade on delete cascade,
    ChatInfoId   int
        constraint FK_Avatars_ChatInfo
            references ChatInfos
            on update cascade on delete cascade,
    DateSet      datetime     not null,
    ImagePayload varchar(max) not null
)