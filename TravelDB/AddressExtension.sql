CREATE TABLE [dbo].[AddressExtension]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [AddressID] INT NULL, 
    [Description] NVARCHAR(200) NULL, 
    [LastModifyTime] DATETIME NULL
)
