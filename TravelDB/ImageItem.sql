CREATE TABLE [dbo].[ImageItem]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [HotelId] varchar(100) NULL, 
	Category int,
    [URL] VARCHAR(200) NULL, 
    [Description] NVARCHAR(50) NULL, 
    [InvBlockCode] INT NULL, 
    [LastMofifyTime] DATETIME NULL
  
)
