CREATE TABLE [dbo].[AwardType]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [Name] NVARCHAR(50) NULL, 
    [LastMofifyTime] DATETIME NULL
)
