﻿create table AspNetUsers
(
    Id                   nvarchar(450) not null
        constraint PK_AspNetUsers
            primary key,
    UserName             nvarchar(256),
    NormalizedUserName   nvarchar(256),
    Email                nvarchar(256),
    NormalizedEmail      nvarchar(256),
    EmailConfirmed       bit           not null,
    PasswordHash         nvarchar(max),
    SecurityStamp        nvarchar(max),
    ConcurrencyStamp     nvarchar(max),
    PhoneNumber          nvarchar(max),
    PhoneNumberConfirmed bit           not null,
    TwoFactorEnabled     bit           not null,
    LockoutEnd           datetimeoffset,
    LockoutEnabled       bit           not null,
    AccessFailedCount    int           not null,
    FirstName            varchar(256),
    LastName             varchar(256),
    Birthday             datetime,
    CallHubConnectionId  nvarchar(450),
    InCall               bit default 0 not null
)
    GO
create index EmailIndex
    on AspNetUsers (NormalizedEmail)
    GO
create unique index UserNameIndex
    on AspNetUsers (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL