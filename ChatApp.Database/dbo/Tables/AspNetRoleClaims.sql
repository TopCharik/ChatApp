create table dbo.AspNetRoleClaims
(
    Id         int identity
        constraint PK_AspNetRoleClaims
        primary key,
    RoleId     nvarchar(450) not null
        constraint FK_AspNetRoleClaims_AspNetRoles_RoleId
        references dbo.AspNetRoles
        on delete cascade,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max)
)
    GO
create index IX_AspNetRoleClaims_RoleId
    on dbo.AspNetRoleClaims (RoleId)