CREATE TABLE [dbo].[Transactions] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [Amount]              INT            NOT NULL,
    [CreateDate]          DATETIME       NOT NULL,
    [AffectsMonthlyLimit] BIT            NOT NULL,
    [AffectsSpecialLimit] BIT            NOT NULL,
    [CategoryId]          INT            NULL,
    [Comment]             NVARCHAR (100) NULL,
    CONSTRAINT [PK_dbo.Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Transactions_dbo.Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_CategoryId]
    ON [dbo].[Transactions]([CategoryId] ASC);

