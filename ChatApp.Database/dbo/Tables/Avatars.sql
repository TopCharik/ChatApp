create table dbo.Avatars
(
    Id         int identity
        constraint PK_Avatars
        primary key,
    UserId     nvarchar(450)
        constraint FK_Avatars_AspNetUsers
        references dbo.AspNetUsers,
    ChatInfoId int
        constraint FK_Avatars_ChatInfo
            references dbo.ChatInfos,
    PictureUrl nvarchar(1024) not null,
    DateSet    datetime not null
)