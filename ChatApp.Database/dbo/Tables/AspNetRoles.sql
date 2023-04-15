create table dbo.AspNetRoles
(
    Id               nvarchar(450) not null
        constraint PK_AspNetRoles
            primary key,
    Name             nvarchar(256),
    NormalizedName   nvarchar(256),
    ConcurrencyStamp nvarchar(max)
)
GO
create unique index RoleNameIndex
    on dbo.AspNetRoles (NormalizedName)
    where [NormalizedName] IS NOT NULL