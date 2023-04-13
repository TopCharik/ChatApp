create table dbo.AspNetUserTokens
(
    UserId        nvarchar(450) not null
        constraint FK_AspNetUserTokens_AspNetUsers_UserId
            references dbo.AspNetUsers
            on delete cascade,
    LoginProvider nvarchar(450) not null,
    Name          nvarchar(450) not null,
    Value         nvarchar(max),
    constraint PK_AspNetUserTokens
        primary key (UserId, LoginProvider, Name)
)

create index EmailIndex
    on dbo.AspNetUsers (NormalizedEmail)

create unique index UserNameIndex
    on dbo.AspNetUsers (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL
