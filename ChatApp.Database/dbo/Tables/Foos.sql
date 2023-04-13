CREATE TABLE [dbo].[Foos] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TextMessage] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Foos] PRIMARY KEY CLUSTERED ([Id] ASC)
);