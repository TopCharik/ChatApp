create table ChatInfos
(
    Id        int identity
        constraint PK_ChatInfo
            primary key,
    Title     nvarchar(256) not null,
    ChatLink  nvarchar(128) not null
        constraint QK_InviteLink
            unique,
    IsPrivate bit default 0 not null
)