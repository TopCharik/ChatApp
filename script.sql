create table dbo.AspNetRoles
(
    Id               nvarchar(450) not null
        constraint PK_AspNetRoles
            primary key,
    Name             nvarchar(256),
    NormalizedName   nvarchar(256),
    ConcurrencyStamp nvarchar(max)
)
go

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
go

create index IX_AspNetRoleClaims_RoleId
    on dbo.AspNetRoleClaims (RoleId)
go

create unique index RoleNameIndex
    on dbo.AspNetRoles (NormalizedName)
    where [NormalizedName] IS NOT NULL
go

create table dbo.AspNetUsers
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
    AccessFailedCount    int           not null
)
go

create table dbo.AspNetUserClaims
(
    Id         int identity
        constraint PK_AspNetUserClaims
            primary key,
    UserId     nvarchar(450) not null
        constraint FK_AspNetUserClaims_AspNetUsers_UserId
            references dbo.AspNetUsers
            on delete cascade,
    ClaimType  nvarchar(max),
    ClaimValue nvarchar(max)
)
go

create index IX_AspNetUserClaims_UserId
    on dbo.AspNetUserClaims (UserId)
go

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
go

create index IX_AspNetUserLogins_UserId
    on dbo.AspNetUserLogins (UserId)
go

create table dbo.AspNetUserRoles
(
    UserId nvarchar(450) not null
        constraint FK_AspNetUserRoles_AspNetUsers_UserId
            references dbo.AspNetUsers
            on delete cascade,
    RoleId nvarchar(450) not null
        constraint FK_AspNetUserRoles_AspNetRoles_RoleId
            references dbo.AspNetRoles
            on delete cascade,
    constraint PK_AspNetUserRoles
        primary key (UserId, RoleId)
)
go

create index IX_AspNetUserRoles_RoleId
    on dbo.AspNetUserRoles (RoleId)
go

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
go

create index EmailIndex
    on dbo.AspNetUsers (NormalizedEmail)
go

create unique index UserNameIndex
    on dbo.AspNetUsers (NormalizedUserName)
    where [NormalizedUserName] IS NOT NULL
go

create table dbo.ChatInfos
(
    Id         int identity
        constraint PK_ChatInfo
            primary key,
    Title      nvarchar(256) not null,
    InviteLink nvarchar(128) not null,
    IsPrivate  bit default 0 not null
)
go

create table dbo.Avatars
(
    Id         int identity
        constraint PK_Avatars
            primary key,
    UserId     nvarchar(450)
        constraint FK_Avatars_AspNetUsers
            references dbo.AspNetUsers,
    ChatInfoId int
        constraint FK_Avatars_ChatInfo
            references dbo.ChatInfos,
    PictureUrl nvarchar(1024) not null,
    DateSet    datetime       not null
)
go

create table dbo.Conversations
(
    Id         int identity
        constraint PK_Conversation
            primary key,
    ChatInfoId int
        constraint QK_Conversations_ChatInfoId
            unique
        constraint FK_Conversation_ChatInfo
            references dbo.ChatInfos
)
go

create table dbo.Participations
(
    Id                    int identity
        constraint PK_Participations
            primary key,
    AspNetUserId          nvarchar(450) not null
        constraint FK_Participations_AspNetUsers
            references dbo.AspNetUsers,
    ConversationId        int           not null
        constraint FK_Participations_Conversations
            references dbo.Conversations,
    CanWriteMessages      bit           not null,
    CanDeleteMessages     bit           not null,
    MutedUntil            datetime,
    CanMuteParticipants   bit           not null,
    CanAddParticipants    bit           not null,
    CanDeleteParticipants bit           not null,
    CanChangeChatAvatar   int           not null,
    CanChangeChatTitle    int           not null,
    CanChangePublicity    bit           not null,
    CanSetPermissions     bit           not null,
    CanDeleteConversation bit           not null,
    constraint QK_Participations_AspNetUserId_ConversationId
        unique (AspNetUserId, ConversationId)
)
go

create table dbo.Messages
(
    Id              int identity
        constraint PK_Messages
            primary key,
    ReplyTo         int
        constraint FK_Messages_Messages_ReplyTo
            references dbo.Messages,
    MessageText     nvarchar(4000) not null,
    DateSent        datetime       not null,
    ParticipationId int            not null
        constraint FK_Messages_Participations
            references dbo.Participations
)
go

