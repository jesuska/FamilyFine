CREATE TABLE [dbo].[TotalCashes] (
    [Id]     INT      IDENTITY (1, 1) NOT NULL,
    [Amount] INT      NOT NULL,
    [Date]   DATETIME NOT NULL,
    CONSTRAINT [PK_dbo.TotalCashes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

