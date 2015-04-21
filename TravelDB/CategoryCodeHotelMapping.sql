CREATE TABLE [dbo].[CategoryCodeHotelMapping]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1), 
    [HotelId] varchar(100) NULL, 
    [SEGID] INT NULL, 
    [LastMofifyTime] DATETIME NULL
	
)
