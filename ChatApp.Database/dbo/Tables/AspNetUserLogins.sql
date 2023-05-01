create table dbo.AspNetUserLogins
(
    LoginProvider       nvarchar(450) not null,
    ProviderKey         nvarchar(450) not null,
    ProviderDisplayName nvarchar(max),
    UserId              nvarchar(450) not null
        constraint FK_AspNetUserLogins_AspNetUsers_UserId
        references dbo.AspNetUsers
        on delete cascade,
    constraint PK_AspNetUserLogins
        primary key (LoginProvider, ProviderKey)
)
    GO
create index IX_AspNetUserLogins_UserId
    on dbo.AspNetUserLogins (UserId)