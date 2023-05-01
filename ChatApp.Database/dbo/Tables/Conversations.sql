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