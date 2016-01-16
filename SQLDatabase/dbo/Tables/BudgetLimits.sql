CREATE TABLE [dbo].[BudgetLimits] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [Limit]     INT      NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [IsMonthly] BIT      NOT NULL,
    CONSTRAINT [PK_dbo.BudgetLimits] PRIMARY KEY CLUSTERED ([Id] ASC)
);

