create table dbo.AspNetUsers
(
    Id                   nvarchar(450) not null
        constraint PK_AspNetUsers
        primary key,
    UserName             nvarchar(256),
    NormalizedUserName   nvarchar(256),
    FirstName            nvarchar(256),
    LastName             nvarchar(256),
    Email                nvarchar(256),
    NormalizedEmail      nvarchar(256),
    EmailConfirmed       bit not null,
    PasswordHash         nvarchar( max),
    SecurityStamp        nvarchar( max),
    ConcurrencyStamp     nvarchar( max),
    PhoneNumber          nvarchar( max),
    PhoneNumberConfirmed bit not null,
    TwoFactorEnabled     bit not null,
    LockoutEnd           datetimeoffset,
    LockoutEnabled       bit not null,
    AccessFailedCount    int not null
)
    GO
create index EmailIndex
    on dbo.AspNetUsers (NormalizedEmail)
    GO
create unique index UserNameIndex
    on dbo.AspNetUsers (NormalizedUserName) where [NormalizedUserName] IS NOT NULL