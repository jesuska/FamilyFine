CREATE TABLE [dbo].[Test]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Description] VARCHAR(50) NOT NULL, 
    [CategoryId] INT NOT NULL, 
    CONSTRAINT [FK_Test_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id])
)
