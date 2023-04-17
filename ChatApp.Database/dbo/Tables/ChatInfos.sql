create table dbo.ChatInfos
(
    Id         int identity
        constraint PK_ChatInfo
        primary key,
    Title      nvarchar(256) not null,
    InviteLink nvarchar(128) not null,
    IsPrivate  bit default 0 not null
)