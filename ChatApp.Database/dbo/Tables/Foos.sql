create table dbo.Foos
(
    Id          int identity
        constraint PK_Foos
            primary key,
    TextMessage nvarchar(max) not null
)