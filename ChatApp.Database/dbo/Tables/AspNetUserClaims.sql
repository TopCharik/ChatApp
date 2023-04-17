create table dbo.AspNetUserClaims
(
    Id         int identity
        constraint PK_AspNetUserClaims
        primary key,
    UserId     nvarchar(450) not null
        constraint FK_AspNetUserClaims_AspNetUsers_UserId
        references dbo.AspNetUsers
        on delete cascade,
    ClaimType  nvarchar( max),
    ClaimValue nvarchar( max)
)
    GO
create index IX_AspNetUserClaims_UserId
    on dbo.AspNetUserClaims (UserId)