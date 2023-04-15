create table dbo.ChatInfo
(
    Id         int identity
        constraint PK_ChatInfo
            primary key,
    CreatorId  nvarchar(450)
        constraint FK_ChatInfo_AspNetUsers
            references dbo.AspNetUsers
            on delete cascade,
    Title      nvarchar(256),
    InviteLink nvarchar(128),
    IsPrivate  bit default 0 not null
)