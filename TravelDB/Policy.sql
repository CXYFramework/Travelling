CREATE TABLE [dbo].[Policy]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [HotelID] varchar(100) NULL, 
    [Key] VARCHAR(50) NULL, 
    [Value] NVARCHAR(500) NULL, 
    [LastMofifyTime] DATETIME NULL
	
)
