CREATE TABLE [dbo].[RefPoint]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [HotelID] varchar(100) NULL, 
    [Distance] DECIMAL(18, 2) NULL, 
    [UOMID] INT NULL, 
    [Name] NVARCHAR(50) NULL, 
    [REFINT] INT NULL, 
    [LastMofifyTime] DATETIME NULL
)
