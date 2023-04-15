create table dbo.Pictures
(
    Id         int identity
        constraint PK_Pictures
            primary key,
    UserID     nvarchar(450)
        constraint FK_Pictures_AspNetUsers
            references dbo.AspNetUsers,
    ChatInfoId int
        constraint FK_Pictures_ChatInfo
            references dbo.ChatInfo,
    PictureUrl nvarchar(1024),
    DateSet    datetime
)