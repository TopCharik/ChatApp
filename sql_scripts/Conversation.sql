create table dbo.Conversation
(
    Id      int identity
            constraint PK_Conversation
            primary key,
    ChatInfoId  int
                constraint FK_Conversation_ChatInfo
                references dbo.ChatInfo
)
